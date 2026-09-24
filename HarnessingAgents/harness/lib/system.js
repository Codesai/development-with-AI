import fs from 'node:fs';
import path from 'node:path';

export function commandExistsOnPath(name) {
  const pathDirs = (process.env.PATH || '').split(path.delimiter);
  return pathDirs.some((dir) => {
    try {
      fs.accessSync(path.join(dir, name), fs.constants.X_OK);
      return true;
    } catch {
      return false;
    }
  });
}

export function requireCommandOnPath(scriptName, commandName) {
  if (!commandExistsOnPath(commandName)) {
    process.stderr.write(`${scriptName}: ${commandName} is required\n`);
    process.exit(1);
  }
}
