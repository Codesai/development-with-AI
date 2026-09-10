#!/usr/bin/env bash
set -euo pipefail

# Guardrail for the HarnessingAgents "Guardrails" exercise.
#
# postToolUse hook: reports comments inside the functions/methods the agent
# changed since HEAD. Scope is per-function, not per-file - an untouched method
# in an edited file is left alone, but a pre-existing comment in a method the
# agent modified is reported. postToolUse runs after the edit, so this only
# nudges (via additionalContext); it cannot block.
#
# Heuristic, not a parser: enclosing-function detection is brace-based, so
# expression-bodied members, top-level-statement files, and unusual layouts may
# be missed. Strings are tracked well enough that a "//" inside a normal string
# literal is not flagged. Covers .cs .js .ts.

fail() { printf 'check-no-comments: %s\n' "$*" >&2; exit 1; }

command -v git  >/dev/null 2>&1 || fail "git is required"
command -v node >/dev/null 2>&1 || fail "node is required"

cd "${1:-$PWD}" || fail "cannot cd into ${1:-$PWD}"
git rev-parse --show-toplevel >/dev/null 2>&1 || fail "not a git working tree"

exec node - <<'NODE'
'use strict';
const { execSync } = require('child_process');
const fs = require('fs');

const MAX = parseInt(process.env.MAX_REPORT || '20', 10);
const GLOBS = ['*.cs', '*.js', '*.ts'];

const git = (a) => {
  try { return execSync('git ' + a, { encoding: 'utf8' }); } catch { return ''; }
};

// --- 1. which new-file lines changed, per file ---------------------------------
const changed = new Map();
const diff = git('diff --unified=0 --relative HEAD -- ' + GLOBS.join(' '));
let file = null, ln = 0;
for (const raw of diff.split('\n')) {
  if (raw.startsWith('+++ ')) {
    const p = raw.slice(4).replace(/^b\//, '').trim();
    file = (p === '/dev/null') ? null : p;
    if (file && !changed.has(file)) changed.set(file, new Set());
  } else if (raw.startsWith('@@')) {
    const m = raw.match(/\+(\d+)/);
    ln = m ? parseInt(m[1], 10) : 0;
  } else if (raw.startsWith('+') && !raw.startsWith('+++')) {
    if (file) changed.get(file).add(ln);
    ln++;
  }
}
// new files git diff does not show yet: treat every line as changed
for (const p of git('ls-files --others --exclude-standard -- ' + GLOBS.join(' ')).split('\n')) {
  if (!p || !fs.existsSync(p)) continue;
  const total = fs.readFileSync(p, 'utf8').split('\n').length;
  const all = new Set();
  for (let i = 1; i <= total; i++) all.add(i);
  changed.set(p, all);
}

// --- 2. one stateful pass: code with strings/comments blanked, + comment lines -
function analyze(lines) {
  const code = [];
  const commentLines = new Set();
  const st = { str: null, block: false };
  for (let idx = 0; idx < lines.length; idx++) {
    const line = lines[idx];
    let out = '';
    let hasComment = st.block;
    for (let i = 0; i < line.length;) {
      const two = line.substr(i, 2);
      if (st.block) { hasComment = true; if (two === '*/') { st.block = false; i += 2; } else i++; continue; }
      if (st.str) {
        if (line[i] === '\\') { i += 2; continue; }
        if (line[i] === st.str) st.str = null;
        i++; continue;
      }
      if (two === '//') { hasComment = true; break; }
      if (two === '/*') { hasComment = true; st.block = true; i += 2; continue; }
      if (line[i] === '"' || line[i] === "'" || line[i] === '`') { st.str = line[i]; i++; continue; }
      out += line[i++];
    }
    code.push(out);
    if (hasComment) commentLines.add(idx + 1);
  }
  return { code, commentLines };
}

// --- 3. [start,end] (1-based, inclusive) of every function/method body --------
const CTRL = /\b(if|for|foreach|while|switch|catch|using|lock|fixed|do|else|try|finally)$/;
function functionRanges(code, lines, commentLines) {
  const ranges = [];
  const stack = [];
  for (let i = 0; i < code.length; i++) {
    for (let k = 0; k < code[i].length; k++) {
      const ch = code[i][k];
      if (ch === '{') {
        let s = i;
        while (s > 0) {
          const p = lines[s - 1].trim();
          // stop at the first real code boundary above the signature, but walk
          // through any leading comment lines so a method's doc block is in range
          if (p === '' || (/[{};]$/.test(p) && !commentLines.has(s))) break;
          s--;
        }
        let sig = '';
        for (let t = s; t < i; t++) sig += ' ' + code[t];
        sig += ' ' + code[i].slice(0, k);
        sig = sig.trim();
        const head = sig.replace(/\s*\([^()]*\)\s*(=>\s*)?$/, '').trim();
        const looksFn = /\)\s*(=>\s*)?$/.test(sig) && !CTRL.test(head);
        stack.push(looksFn ? { start: s } : null);
      } else if (ch === '}') {
        const top = stack.pop();
        if (top) ranges.push([top.start + 1, i + 1]);
      }
    }
  }
  return ranges;
}

// --- 4. flag comment lines inside changed functions --------------------------
const hits = [];
for (const [path, lineSet] of changed) {
  if (!fs.existsSync(path)) continue;
  const lines = fs.readFileSync(path, 'utf8').split('\n');
  const { code, commentLines } = analyze(lines);
  const ranges = functionRanges(code, lines, commentLines).filter(([a, b]) => {
    for (const n of lineSet) if (n >= a && n <= b) return true;
    return false;
  });
  const seen = new Set();
  for (const [a, b] of ranges) {
    for (let n = a; n <= b && n <= lines.length; n++) {
      if (!seen.has(n) && commentLines.has(n)) {
        seen.add(n);
        hits.push(`${path}:${n}: ${lines[n - 1].trim()}`);
      }
    }
  }
}

if (hits.length === 0) process.exit(0);

const message =
  `The no-comments guardrail found ${hits.length} comment(s) inside functions you just changed:\n\n` +
  hits.slice(0, MAX).join('\n') + '\n\n' +
  'Remove every comment from those functions, including ones that were already there. ' +
  'Rely on descriptive names and small functions instead; do not add explanatory comments to the code.';

if (process.stdout.isTTY) {
  process.stderr.write(message + '\n');
  process.exit(2);
}
process.stdout.write(JSON.stringify({ additionalContext: message }));
process.exit(0);
NODE
