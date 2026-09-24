#!/usr/bin/env node
//
// The gateway for the HarnessingAgents "Gateways" exercise.
//
// Run this from HarnessingAgents/app instead of `copilot`. It launches Copilot
// CLI with the raw filesystem tools removed:
//   - the native view / glob / grep tools are excluded (the model never sees them)
//   - every shell read / list / search command is denied
//   - the only sanctioned file access is `files-gateway` (on PATH via this script)
//
// The flags are per-session, so this launcher IS the gateway - a plain
// `copilot` has none of it. Anything after the script name is passed straight
// to copilot.

import fs from 'node:fs';
import path from 'node:path';
import { spawnSync } from 'node:child_process';
import { moduleDir } from '../lib/paths.js';
import { requireCommandOnPath } from '../lib/system.js';

const EXCLUDED_TOOLS = ['view', 'glob', 'grep'];

const DENIED_SHELL_COMMANDS = [
  'cat', 'less', 'more',
  'head', 'tail', 'nl', 'tac',
  'ls', 'dir', 'vdir',
  'find', 'fd', 'tree',
  'grep', 'egrep', 'fgrep', 'rg',
  'sed', 'awk', 'cut',
  'xxd', 'od', 'strings',
  'dd', 'cp', 'install', 'wget',
  'python', 'python3', 'node', 'nodejs',
  'perl', 'ruby', 'php', 'lua',
  // Copilot CLI is subcommand-aware for git: denying the bare name alone
  // does not catch `git log` etc., so both forms are needed.
  'git:*', 'git',
];

const ALLOWED_SHELL_COMMANDS = ['files-gateway', 'make', 'docker', 'curl'];

function shellDenyFlags(commandNames) {
  return commandNames.map((name) => `--deny-tool=shell(${name})`);
}

function shellAllowFlags(commandNames) {
  return commandNames.map((name) => `--allow-tool=shell(${name})`);
}

function buildCopilotArgs(extraArgs) {
  return [
    '--allow-all-tools',
    `--excluded-tools=${EXCLUDED_TOOLS.join(',')}`,
    ...shellDenyFlags(DENIED_SHELL_COMMANDS),
    ...shellAllowFlags(ALLOWED_SHELL_COMMANDS),
    ...extraArgs,
  ];
}

// Drops a symlink to files-gateway right in the launch directory, on top of
// putting it on PATH. AGENTS.md tells the agent to reach for `files-gateway`
// once its usual read tools are denied, and this makes that tool something
// it can actually see sitting next to its work instead of something it has
// to know to expect on PATH. Removed again on exit so it never lingers into
// other exercises that share this same app/ directory.
function createGatewaySymlink(gatewayDir, launchDir) {
  const target = path.join(gatewayDir, 'files-gateway');
  const symlinkPath = path.join(launchDir, 'files-gateway');
  fs.rmSync(symlinkPath, { force: true });
  fs.symlinkSync(target, symlinkPath);
  return symlinkPath;
}

function main() {
  requireCommandOnPath('copilot-gated', 'copilot');

  const gatewayDir = moduleDir(import.meta.url);
  const launchDir = process.cwd();
  const symlinkPath = createGatewaySymlink(gatewayDir, launchDir);

  process.on('exit', () => fs.rmSync(symlinkPath, { force: true }));
  process.on('SIGINT', () => process.exit(130));
  process.on('SIGTERM', () => process.exit(143));

  const env = {
    ...process.env,
    PATH: `${gatewayDir}${path.delimiter}${process.env.PATH ?? ''}`,
    FILES_GATEWAY_ROOT: launchDir,
  };

  const result = spawnSync('copilot', buildCopilotArgs(process.argv.slice(2)), { stdio: 'inherit', env });
  process.exit(result.status ?? 1);
}

main();
