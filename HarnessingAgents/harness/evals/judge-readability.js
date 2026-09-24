#!/usr/bin/env node
//
// judge-readability.js - LLM-as-judge grader for exercise 05. Takes one
// trial directory (as produced by run-trial.js) and asks a *second*,
// independent non-interactive `copilot -p` call to judge whether the diff is
// genuinely readable without comments - not just comment-free, which is all
// a script like check-no-comments.js can tell you.
//
// The judge call is run from an empty scratch directory, not the trial
// directory itself, with the diff inlined into the prompt - it has no
// reason to touch any tool, so nothing it could do there matters.

import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import { spawnSync } from 'node:child_process';
import { isMainModule } from '../lib/paths.js';
import { runGit } from '../lib/git.js';
import { requireCommandOnPath } from '../lib/system.js';

// `git diff HEAD` alone misses brand-new files (they're untracked, not
// modified) - stage everything first so the diff includes them too. This
// only touches the throwaway trial repo's index, never the real project.
function stageAndDiff(trialDir) {
  runGit(['add', '-A'], trialDir);
  return runGit(['diff', '--cached', 'HEAD'], trialDir);
}

function buildReadabilityPrompt(diff) {
  return `You are grading a code change for readability, not for correctness. The
project's guideline is to write code without explanatory comments and rely on
clear names and small functions instead. Judge whether this diff actually
achieves that: would a reader unfamiliar with the change understand the
confirmation-code format and logic just from the code, with no comments to
lean on?

Answer with exactly one line, nothing else: "READABLE" or "NOT-READABLE",
then a dash, then one sentence of reasoning.

Diff:
${diff}`;
}

function runJudgeCopilot(prompt) {
  const scratchDir = fs.mkdtempSync(path.join(os.tmpdir(), 'harnessingagents-judge-'));
  try {
    const result = spawnSync('copilot', ['-p', prompt, '-s', '--allow-all-tools'], { cwd: scratchDir, encoding: 'utf8' });
    return result.stdout || '';
  } finally {
    fs.rmSync(scratchDir, { recursive: true, force: true });
  }
}

// The judge is asked for one line, but -s output can carry trailing blank
// lines - drop those first, then split the last real line on " - " into
// verdict and reasoning.
function parseVerdictLine(copilotOutput) {
  const lines = copilotOutput.split('\n').filter((line) => line.trim() !== '');
  const lastLine = (lines[lines.length - 1] || '').trim();
  const separatorIndex = lastLine.indexOf(' - ');
  if (separatorIndex === -1) return { verdict: lastLine, reasoning: '' };
  return {
    verdict: lastLine.slice(0, separatorIndex).trim(),
    reasoning: lastLine.slice(separatorIndex + 3).trim(),
  };
}

export async function judgeDiff(trialDir) {
  const diff = stageAndDiff(trialDir);
  if (!diff.trim()) {
    return { verdict: 'NOT-READABLE', reasoning: 'no changes were made' };
  }
  return parseVerdictLine(runJudgeCopilot(buildReadabilityPrompt(diff)));
}

export function formatVerdict({ verdict, reasoning }) {
  return reasoning ? `${verdict} - ${reasoning}` : verdict;
}

function resolveTrialDir(requestedDir) {
  if (!requestedDir) {
    process.stderr.write('usage: judge-readability.js <trial-dir>\n');
    process.exit(2);
  }
  const trialDir = path.resolve(requestedDir);
  if (!fs.existsSync(path.join(trialDir, '.git'))) {
    process.stderr.write(`judge-readability: ${requestedDir} is not a trial directory\n`);
    process.exit(1);
  }
  return trialDir;
}

async function main() {
  requireCommandOnPath('judge-readability', 'copilot');
  const trialDir = resolveTrialDir(process.argv[2]);
  process.stdout.write(formatVerdict(await judgeDiff(trialDir)) + '\n');
}

if (isMainModule(import.meta.url)) main();
