# Project guidelines

## Instruction priority

When instructions conflict, the agent should follow them in this order. If instructions conflict, consider if there is way to satisfy both:

1. User requests
2. The guidance in this file (`AGENTS.md`)
3. Other repository guidance
4. General defaults and environment settings

## Scope

Everything needed for any task in this project is inside this directory. Do not read, list, or search files or folders outside it.

Do not run git commands or inspect git history, status, diffs, or commit messages. Version control is handled outside this session.

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

