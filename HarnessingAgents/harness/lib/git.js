import { execFileSync } from 'node:child_process';

export function runGit(args, cwd) {
  return execFileSync('git', args, { cwd, encoding: 'utf8' });
}

// Same as runGit, but swallows failure and returns '' - for the many git
// queries here that are just "is there anything?" rather than "must succeed".
export function tryGit(args, cwd) {
  try {
    return runGit(args, cwd);
  } catch {
    return '';
  }
}

export function isGitWorkingTree(cwd) {
  try {
    execFileSync('git', ['rev-parse', '--show-toplevel'], { cwd, stdio: 'ignore' });
    return true;
  } catch {
    return false;
  }
}
