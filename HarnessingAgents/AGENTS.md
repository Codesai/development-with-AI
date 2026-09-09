# Project guidelines

## Coding style

- Optimise for the reader, not the writer. Someone new to the project should be able to open any file and understand what the code does and why.
- Prefer clarity over cleverness. A longer, obvious solution beats a short, dense one.
- Make intent explicit. Do not leave the reasoning behind a non-obvious choice for the reader to reconstruct.
- Keep functions small and focused, and name things so their purpose is clear.
- When a value or a rule is not self-evident (magic numbers, string formats, edge cases), make sure its meaning is captured close to where it matters.
- Stay consistent with the style already present in the codebase.

## Process

- Implement features end to end: domain, storage, API response, and frontend.
- Keep existing behaviour identical when refactoring.
