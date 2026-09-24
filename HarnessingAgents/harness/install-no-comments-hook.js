#!/usr/bin/env node
//
// Installs the no-comments guardrail as a user-level GitHub Copilot CLI
// postToolUse hook: copies the check script into ~/.copilot/hooks/ and writes
// the hook config next to it, so nothing about the guardrail enters the
// project the agent reads. Run once, then start a fresh `copilot` session from
// HarnessingAgents/app so the hook loads.
//
//   ./install-no-comments-hook.js            install (default)
//   ./install-no-comments-hook.js uninstall  remove it

import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import { moduleDir, isMainModule } from './lib/paths.js';

const harnessDir = moduleDir(import.meta.url);
const hooksDir = path.join(os.homedir(), '.copilot', 'hooks');
const scriptSrc = path.join(harnessDir, 'guardrails', 'check-no-comments.js');
const scriptDst = path.join(hooksDir, 'check-no-comments.js');
const configDst = path.join(hooksDir, 'no-comments.json');

const HOOK_CONFIG = {
  version: 1,
  hooks: {
    postToolUse: [
      {
        type: 'command',
        matcher: 'edit|create|apply_patch',
        bash: '~/.copilot/hooks/check-no-comments.js',
        timeoutSec: 30,
      },
    ],
  },
};

function fail(message) {
  process.stderr.write(`install-no-comments-hook: ${message}\n`);
  process.exit(1);
}

function uninstallHook() {
  fs.rmSync(scriptDst, { force: true });
  fs.rmSync(configDst, { force: true });
  process.stdout.write(
    `Removed:\n  ${scriptDst}\n  ${configDst}\nStart a fresh copilot session for the change to take effect.\n`,
  );
}

function installHook() {
  if (!fs.existsSync(scriptSrc)) fail(`guardrail script not found at ${scriptSrc}`);

  fs.mkdirSync(hooksDir, { recursive: true });
  fs.copyFileSync(scriptSrc, scriptDst);
  fs.chmodSync(scriptDst, 0o755);
  fs.writeFileSync(configDst, JSON.stringify(HOOK_CONFIG, null, 2) + '\n');

  process.stdout.write(
    `Installed:\n  ${scriptDst}\n  ${configDst}\n\n`
    + 'Start a fresh copilot session from HarnessingAgents/app so the hook loads.\n'
    + `Remove it later with: ${process.argv[1]} uninstall\n`,
  );
}

function main() {
  const action = process.argv[2] || 'install';
  if (action === 'uninstall') return uninstallHook();
  if (action !== 'install') fail(`usage: ${path.basename(process.argv[1])} [install|uninstall]`);
  installHook();
}

if (isMainModule(import.meta.url)) main();
