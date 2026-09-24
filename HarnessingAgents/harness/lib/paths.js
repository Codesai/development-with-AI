import { fileURLToPath } from 'node:url';
import path from 'node:path';

export function moduleDir(importMetaUrl) {
  return path.dirname(fileURLToPath(importMetaUrl));
}

// True when this module was run directly (`node script.js` or `./script.js`)
// rather than imported by another module - the usual guard for a CLI entry point.
export function isMainModule(importMetaUrl) {
  return process.argv[1] === fileURLToPath(importMetaUrl);
}
