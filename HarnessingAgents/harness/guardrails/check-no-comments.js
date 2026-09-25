#!/usr/bin/env node
//
// Guardrail for the HarnessingAgents "Guardrails" exercise.
//
// postToolUse hook: reports comments inside the functions/methods the agent
// changed since HEAD. Scope is per-function, not per-file - an untouched
// method in an edited file is left alone, but a pre-existing comment in a
// method the agent modified is reported. postToolUse runs after the edit, so
// this only nudges (via additionalContext); it cannot block.
//
// Heuristic, not a parser: enclosing-function detection is brace-based, so
// expression-bodied members, top-level-statement files, and unusual layouts
// may be missed. Strings are tracked well enough that a "//" inside a normal
// string literal is not flagged. Covers .cs .js .ts.

import fs from 'node:fs';
import path from 'node:path';
import { execFileSync } from 'node:child_process';
import { fileURLToPath } from 'node:url';

const TRACKED_GLOBS = ['*.cs', '*.js', '*.ts'];
const CONTROL_KEYWORDS_RE = /\b(if|for|foreach|while|switch|catch|using|lock|fixed|do|else|try|finally)$/;
const MAX_REPORTED_HITS = parseInt(process.env.MAX_REPORT || '20', 10);

function tryGit(args, cwd) {
  try {
    return execFileSync('git', args, { cwd, encoding: 'utf8' });
  } catch {
    return '';
  }
}

function isGitWorkingTree(cwd) {
  try {
    execFileSync('git', ['rev-parse', '--show-toplevel'], { cwd, stdio: 'ignore' });
    return true;
  } catch {
    return false;
  }
}

// Map of relative file path -> set of 1-based line numbers touched since HEAD.
function findChangedLines(repoDir) {
  const changed = new Map();

  const diffOutput = tryGit(['diff', '--unified=0', '--relative', 'HEAD', '--', ...TRACKED_GLOBS], repoDir);
  let currentFile = null;
  let lineNumber = 0;
  for (const rawLine of diffOutput.split('\n')) {
    if (rawLine.startsWith('+++ ')) {
      const filePath = rawLine.slice(4).replace(/^b\//, '').trim();
      currentFile = filePath === '/dev/null' ? null : filePath;
      if (currentFile && !changed.has(currentFile)) changed.set(currentFile, new Set());
    } else if (rawLine.startsWith('@@')) {
      const match = rawLine.match(/\+(\d+)/);
      lineNumber = match ? parseInt(match[1], 10) : 0;
    } else if (rawLine.startsWith('+') && !rawLine.startsWith('+++')) {
      if (currentFile) changed.get(currentFile).add(lineNumber);
      lineNumber++;
    }
  }

  // Brand-new files don't show up in `git diff` against HEAD - treat every
  // line in them as changed.
  const untrackedFiles = tryGit(['ls-files', '--others', '--exclude-standard', '--', ...TRACKED_GLOBS], repoDir).split('\n');
  for (const relativePath of untrackedFiles) {
    if (!relativePath) continue;
    const absolutePath = path.join(repoDir, relativePath);
    if (!fs.existsSync(absolutePath)) continue;
    const totalLines = fs.readFileSync(absolutePath, 'utf8').split('\n').length;
    const allLines = new Set(Array.from({ length: totalLines }, (_, i) => i + 1));
    changed.set(relativePath, allLines);
  }

  return changed;
}

// Walks each line character by character, blanking out string/comment
// contents so bracket-matching later never gets confused by a "{" inside a
// string or comment. Returns the blanked-out code plus which lines contain
// a comment.
function stripStringsAndComments(lines) {
  const code = [];
  const commentLines = new Set();
  const state = { stringDelimiter: null, inBlockComment: false };

  lines.forEach((line, index) => {
    let strippedLine = '';
    let lineHasComment = state.inBlockComment;

    for (let i = 0; i < line.length;) {
      const twoChars = line.substr(i, 2);

      if (state.inBlockComment) {
        lineHasComment = true;
        if (twoChars === '*/') { state.inBlockComment = false; i += 2; } else i++;
        continue;
      }
      if (state.stringDelimiter) {
        if (line[i] === '\\') { i += 2; continue; }
        if (line[i] === state.stringDelimiter) state.stringDelimiter = null;
        i++;
        continue;
      }
      if (twoChars === '//') { lineHasComment = true; break; }
      if (twoChars === '/*') { lineHasComment = true; state.inBlockComment = true; i += 2; continue; }
      if (line[i] === '"' || line[i] === "'" || line[i] === '`') { state.stringDelimiter = line[i]; i++; continue; }
      strippedLine += line[i];
      i++;
    }

    code.push(strippedLine);
    if (lineHasComment) commentLines.add(index + 1);
  });

  return { code, commentLines };
}

// Walks up from a "{" to the start of its signature, stopping at the
// nearest code boundary above (blank line, or a line ending in { } ; that
// isn't itself a comment line - so a doc-comment block right above a method
// stays part of its signature).
function findSignatureStart(lines, commentLines, braceLine) {
  let signatureStart = braceLine;
  while (signatureStart > 0) {
    const previousLineTrimmed = lines[signatureStart - 1].trim();
    const isCodeBoundary = previousLineTrimmed === ''
      || (/[{};]$/.test(previousLineTrimmed) && !commentLines.has(signatureStart));
    if (isCodeBoundary) break;
    signatureStart--;
  }
  return signatureStart;
}

function looksLikeFunctionSignature(signature) {
  const signatureHead = signature.replace(/\s*\([^()]*\)\s*(=>\s*)?$/, '').trim();
  return /\)\s*(=>\s*)?$/.test(signature) && !CONTROL_KEYWORDS_RE.test(signatureHead);
}

// [start, end] (1-based, inclusive) of every function/method body in the file.
function findFunctionRanges(code, lines, commentLines) {
  const ranges = [];
  const openBraceFunctionStarts = [];

  for (let lineIndex = 0; lineIndex < code.length; lineIndex++) {
    for (let charIndex = 0; charIndex < code[lineIndex].length; charIndex++) {
      const ch = code[lineIndex][charIndex];

      if (ch === '{') {
        const signatureStart = findSignatureStart(lines, commentLines, lineIndex);
        let signature = '';
        for (let line = signatureStart; line < lineIndex; line++) signature += ' ' + code[line];
        signature += ' ' + code[lineIndex].slice(0, charIndex);
        signature = signature.trim();
        openBraceFunctionStarts.push(looksLikeFunctionSignature(signature) ? signatureStart : null);
      } else if (ch === '}') {
        const functionStart = openBraceFunctionStarts.pop();
        if (functionStart !== null) ranges.push([functionStart + 1, lineIndex + 1]);
      }
    }
  }

  return ranges;
}

function rangeOverlapsAny(start, end, lineNumbers) {
  for (const lineNumber of lineNumbers) {
    if (lineNumber >= start && lineNumber <= end) return true;
  }
  return false;
}

function findCommentedLinesInChangedFunctions(repoDir) {
  const changed = findChangedLines(repoDir);
  const hits = [];

  for (const [relativePath, changedLineNumbers] of changed) {
    const absolutePath = path.join(repoDir, relativePath);
    if (!fs.existsSync(absolutePath)) continue;

    const lines = fs.readFileSync(absolutePath, 'utf8').split('\n');
    const { code, commentLines } = stripStringsAndComments(lines);
    const touchedRanges = findFunctionRanges(code, lines, commentLines)
      .filter(([start, end]) => rangeOverlapsAny(start, end, changedLineNumbers));

    const alreadyReported = new Set();
    for (const [start, end] of touchedRanges) {
      for (let lineNumber = start; lineNumber <= end && lineNumber <= lines.length; lineNumber++) {
        if (alreadyReported.has(lineNumber) || !commentLines.has(lineNumber)) continue;
        alreadyReported.add(lineNumber);
        hits.push(`${relativePath}:${lineNumber}: ${lines[lineNumber - 1].trim()}`);
      }
    }
  }

  return hits;
}

export function gradeTrial(repoDir) {
  return { hits: findCommentedLinesInChangedFunctions(repoDir) };
}

export function formatFailureMessage(hits) {
  return `The no-comments guardrail found ${hits.length} comment(s) inside functions you just changed:\n\n`
    + hits.slice(0, MAX_REPORTED_HITS).join('\n') + '\n\n'
    + 'Remove every comment from those functions, including ones that were already there. '
    + 'Rely on descriptive names and small functions instead; do not add explanatory comments to the code.';
}

function resolveRepoDir(requestedDir) {
  const repoDir = path.resolve(requestedDir || process.cwd());
  if (!fs.existsSync(repoDir) || !fs.statSync(repoDir).isDirectory()) {
    process.stderr.write(`check-no-comments: cannot cd into ${requestedDir}\n`);
    process.exit(1);
  }
  if (!isGitWorkingTree(repoDir)) {
    process.stderr.write('check-no-comments: not a git working tree\n');
    process.exit(1);
  }
  return repoDir;
}

// Dual-mode CLI output, matched to how this script is called:
//   - interactively (a TTY): human-readable message on stderr, exit 2.
//   - as a postToolUse hook (stdout piped): a `{additionalContext}` JSON
//     blob on stdout, exit 0 - the shape Copilot CLI's hook contract expects.
function main() {
  const repoDir = resolveRepoDir(process.argv[2]);

  const { hits } = gradeTrial(repoDir);
  if (hits.length === 0) return;

  const message = formatFailureMessage(hits);
  if (process.stdout.isTTY) {
    process.stderr.write(message + '\n');
    process.exit(2);
  }
  process.stdout.write(JSON.stringify({ additionalContext: message }));
}

if (process.argv[1] === fileURLToPath(import.meta.url)) main();
