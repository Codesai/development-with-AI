# Project guidelines

## Instruction priority

When instructions conflict, consider if there is way to satisfy both. If they are irreconciliable the agent should follow them in this order:

1. User requests
2. The guidance in this file (`AGENTS.md`)
3. Other repository guidance
4. General defaults and environment settings

## Scope

Treat the current working directory shown by the shell as the repository root for this task.

- Use only relative paths for file reads, searches, edits, and commands.
- Never construct or use absolute paths, even if the environment reports another repository path.
- Do not access parent directories or sibling directories.

## Available Tools

- Do not spawn subagents or delegate work. Perform investigation and implementation directly in the main session.
- If the usual file-reading tools (reading, listing, or searching files) are unavailable or denied, check whether a `files-gateway` command exists on `PATH` or in the current directory before giving up or asking the user for something you might otherwise be able to get yourself. It offers the same operations under different names: `files-gateway ls [dir]`, `files-gateway read <file>`, `files-gateway grep <pattern> [path]`.

## Coding style

- Optimise for the reader, not the writer. Someone new to the project should be able to open any file and understand what the code does and why.
- Prefer clarity over cleverness.
- Make intent and behaviour explicit.
- Make sure meaning is captured close to where it matters.
- For non-obvious domain rules, invariants, formats, and edge cases, make the
  relevant meaning clear in the design and implementation where it applies.
- The reader should not have to infer behaviour from low level implementations
- Stay consistent with the style already present in the codebase.
- All these rules apply throughout the codebase, no matter if the code is public or private.

## Process

- Implement features end to end: domain, storage, API response, and frontend.
- Keep existing behaviour identical when refactoring.

## Hook feedback

When a tool hook reports feedback, tell the user as soon as it is received, before continuing work. State:

- That the hook was encountered and whether it ran before or after the edit.
- The hook's concise feedback.
- Whether the feedback was acted on.
- The reasoning for acting or not acting on it.

Also include the same information in the final response when relevant.




