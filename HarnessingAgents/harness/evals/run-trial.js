#!/usr/bin/env node
//
// run-trial.js - one independent, non-interactive Copilot CLI run of the
// exercises 01/02 confirmation-code prompt, shared by the eval harness in
// exercises 04 and 05.
//
// Copies app/ (as it currently stands - including whatever the student has
// done to AGENTS.md) into a fresh throwaway git repo under results/, so a
// grader that reads `git diff` works unmodified, then runs `copilot -p ... -s`
// there non-interactively.
//
// Trial repos live under results/ (next to this script), not /tmp - the
// agent's changes never touch the real app/ or this repo's own git state.
// results/ is gitignored except for a placeholder, so trial data stays out
// of version control without needing you to remember to clean it up before a
// commit.
//
// This function does not delete the trial directory - the caller grades it
// and decides whether to keep it around for inspection or remove it.
//
// The agent's output streams live to stderr (and into <trial-dir>/agent.log,
// unprefixed) as it runs, so a slow trial shows what the agent is doing
// instead of sitting silent.

import fs from 'node:fs';
import path from 'node:path';
import { spawn } from 'node:child_process';
import { moduleDir, isMainModule } from '../lib/paths.js';
import { runGit } from '../lib/git.js';
import { requireCommandOnPath } from '../lib/system.js';

const PROMPT = 'Add a registration confirmation code to this project. Implement the feature end to end: generate the code when a registration is saved, store it in `interests.txt`, return it in the API response, and show it in the frontend confirmation message. The code format is `AAA-YYYYMMDD-NNN-C` (course prefix, UTC date, daily per-course sequence, check character). Make the smallest change that satisfies this - do not refactor or touch unrelated code. Do not build, run, or otherwise validate the app (no build, no server start, no curl, no manual testing) - just make the code change and stop.';

const scriptDir = moduleDir(import.meta.url);
const appDir = path.join(scriptDir, '..', '..', 'app');
const resultsDir = path.join(scriptDir, 'results');

function createTrialDirectory() {
  fs.mkdirSync(resultsDir, { recursive: true });
  const trialDir = fs.mkdtempSync(path.join(resultsDir, 'trial-'));
  fs.cpSync(appDir, trialDir, { recursive: true });
  return trialDir;
}

function commitBaseline(trialDir) {
  runGit(['init', '-q'], trialDir);
  runGit(['add', '-A'], trialDir);
  runGit(['-c', 'user.email=eval@localhost', '-c', 'user.name=eval', 'commit', '-q', '-m', 'baseline'], trialDir);
}

function buildCopilotArgs({ share, transcriptPath }) {
  const args = ['-p', PROMPT, '-s', '--stream', 'on', '--allow-all-tools'];
  if (share) args.push(`--share=${transcriptPath}`);
  return args;
}

// Line-buffers a child stream, writing each finished line raw to the log
// file and again to our own stderr (optionally "[label] "-prefixed), so a
// slow trial shows live progress instead of sitting silent.
function relayLines(readable, { label, logStream }) {
  let buffer = '';
  const flushLine = (line) => {
    logStream.write(line + '\n');
    process.stderr.write(label ? `[${label}] ${line}\n` : `${line}\n`);
  };

  readable.on('data', (chunk) => {
    buffer += chunk.toString();
    const lines = buffer.split('\n');
    buffer = lines.pop();
    lines.forEach(flushLine);
  });

  return new Promise((resolve) => {
    readable.on('end', () => {
      if (buffer) flushLine(buffer);
      resolve();
    });
  });
}

// Non-interactive: the agent runs to completion or Copilot's own limits kick
// in, then this resolves. A nonzero exit (e.g. the agent gave up) is not
// treated as a failure here - an incomplete trial is still a trial, and the
// grader will simply see whatever diff resulted (possibly none).
function runCopilot(trialDir, args, { label }) {
  return new Promise((resolve) => {
    const logStream = fs.createWriteStream(path.join(trialDir, 'agent.log'));
    const child = spawn('copilot', args, { cwd: trialDir });

    const stdoutRelayed = relayLines(child.stdout, { label, logStream });
    const stderrRelayed = relayLines(child.stderr, { label, logStream });

    child.on('close', async () => {
      await Promise.all([stdoutRelayed, stderrRelayed]);
      logStream.end(resolve);
    });
  });
}

export async function runTrial({ label, share = false } = {}) {
  requireCommandOnPath('run-trial', 'copilot');

  const trialDir = createTrialDirectory();
  commitBaseline(trialDir);

  const transcriptPath = path.join(trialDir, 'transcript.md');
  await runCopilot(trialDir, buildCopilotArgs({ share, transcriptPath }), { label });

  return trialDir;
}

async function main() {
  const share = process.argv.includes('--share');
  const trialDir = await runTrial({ label: process.env.RUN_TRIAL_LABEL, share });
  process.stdout.write(trialDir + '\n');
}

if (isMainModule(import.meta.url)) main();
