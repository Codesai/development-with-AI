#!/usr/bin/env node
//
// eval-readability.js - exercise 05's eval. Same independent trials as
// eval-no-comments.js, graded by judge-readability.js's LLM-as-judge
// instead of a script. Trial directories are kept (not deleted) so you can
// re-judge or inspect them afterward - clean them up yourself when done
// (rm -rf results/trial-*).
//
// Usage:
//   eval-readability.js [N]
//   eval-readability.js --repeat-judge <trial-dir> [M]
//
// --repeat-judge holds one trial's code fixed and re-runs only the judge M
// times (default 3), to isolate judge-side non-determinism from agent-side
// non-determinism.

import { isMainModule } from '../lib/paths.js';
import { runTrial } from './run-trial.js';
import { judgeDiff, formatVerdict } from './judge-readability.js';

function parseArgs(argv) {
  if (argv[0] === '--repeat-judge') {
    const trialDir = argv[1];
    if (!trialDir) {
      process.stderr.write('usage: eval-readability.js --repeat-judge <trial-dir> [M]\n');
      process.exit(2);
    }
    return { mode: 'repeat-judge', trialDir, count: parseInt(argv[2] || '3', 10) };
  }
  return { mode: 'trials', count: parseInt(argv[0] || '3', 10) };
}

async function repeatJudge(trialDir, repeatCount) {
  for (let i = 1; i <= repeatCount; i++) {
    process.stdout.write(`judge run ${i}/${repeatCount}: `);
    process.stdout.write(formatVerdict(await judgeDiff(trialDir)) + '\n');
  }
}

async function runTrials(totalTrials) {
  let readableCount = 0;
  const trialDirs = [];

  for (let trialNumber = 1; trialNumber <= totalTrials; trialNumber++) {
    process.stderr.write(`\n=== Trial ${trialNumber}/${totalTrials}: agent run (prefixed [${trialNumber}/${totalTrials}] below) ===\n`);
    const trialDir = await runTrial({ label: `${trialNumber}/${totalTrials}` });
    trialDirs.push(trialDir);

    const verdict = await judgeDiff(trialDir);
    process.stderr.write(`--- Trial ${trialNumber}/${totalTrials}: ${formatVerdict(verdict)} ---\n`);
    if (verdict.verdict === 'READABLE') readableCount++;
  }

  process.stdout.write(`\n${readableCount}/${totalTrials} judged readable\n`);
  process.stdout.write('\nTrial directories (kept - re-run judge-readability.js on any of them, or use --repeat-judge):\n');
  trialDirs.forEach((dir) => process.stdout.write(`  ${dir}\n`));
}

async function main() {
  const args = parseArgs(process.argv.slice(2));
  if (args.mode === 'repeat-judge') return repeatJudge(args.trialDir, args.count);
  return runTrials(args.count);
}

if (isMainModule(import.meta.url)) main();
