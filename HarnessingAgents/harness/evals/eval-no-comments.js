#!/usr/bin/env node
//
// eval-no-comments.js - exercise 04's eval. Runs the confirmation-code
// prompt N times (default 3) via run-trial.js, and grades each run
// deterministically by calling exercise 02's guardrail directly on the
// resulting diff - no hook, no live session, just the finished result.
//
// Usage: eval-no-comments.js [N]

import fs from 'node:fs';
import path from 'node:path';
import { isMainModule } from '../lib/paths.js';
import { runTrial } from './run-trial.js';
import { gradeTrial, formatFailureMessage } from '../guardrails/check-no-comments.js';

async function runOneTrial(trialNumber, totalTrials) {
  process.stderr.write(`\n=== Trial ${trialNumber}/${totalTrials}: agent run (prefixed [${trialNumber}/${totalTrials}] below) ===\n`);
  const trialDir = await runTrial({ label: `${trialNumber}/${totalTrials}` });
  const { hits } = gradeTrial(trialDir);

  if (hits.length === 0) {
    process.stderr.write(`--- Trial ${trialNumber}/${totalTrials}: PASS ---\n`);
    fs.rmSync(trialDir, { recursive: true, force: true });
    return { passed: true };
  }

  const message = formatFailureMessage(hits);
  fs.writeFileSync(path.join(trialDir, 'grade.log'), message);
  process.stderr.write(`--- Trial ${trialNumber}/${totalTrials}: FAIL (kept at ${trialDir}) ---\n`);
  process.stderr.write(message.split('\n').map((line) => `    ${line}`).join('\n') + '\n');
  return { passed: false, trialDir };
}

async function main() {
  const totalTrials = parseInt(process.argv[2] || '3', 10);
  let passedCount = 0;
  const failedDirs = [];

  for (let trialNumber = 1; trialNumber <= totalTrials; trialNumber++) {
    const result = await runOneTrial(trialNumber, totalTrials);
    if (result.passed) passedCount++;
    else failedDirs.push(result.trialDir);
  }

  process.stdout.write(`\n${passedCount}/${totalTrials} passed\n`);
  if (failedDirs.length > 0) {
    process.stdout.write('\nFailed trials (see <dir>/grade.log for the flagged comments, <dir>/agent.log for the run):\n');
    failedDirs.forEach((dir) => process.stdout.write(`  ${dir}\n`));
  }
}

if (isMainModule(import.meta.url)) main();
