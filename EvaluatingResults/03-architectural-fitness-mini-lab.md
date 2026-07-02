# Exercise 3 - Architectural Fitness Mini-Lab

## Goal

Translate architectural intent into executable rules.

## Duration

20 minutes

## Setup

Use the prepared architecture fitness branches. Students do not need to copy or create the code manually.

Start from the generated architectural violation:

```bash
git switch evaluation/architecture/01-finding-violation
```

Available branches:

- `evaluation/architecture/01-finding-violation`: generated code with an architectural violation.
- `evaluation/architecture/02-fitness-test-red`: same code with a failing architecture fitness test.
- `evaluation/architecture/03-fitness-test-green`: corrected design with the architecture fitness test passing.

## Scenario

The project follows this layered architecture:

```text
api -> application
application -> domain
application -> infrastructure
domain -> no framework dependencies
```

## Student Tasks

- Identify the architectural violation.
- Explain why it is dangerous.
- Propose a better design.
- Write one natural-language architectural rule.
- Decide whether the rule belongs in:
  - `AGENTS.md`
  - code review checklist
  - CI architecture test
  - all of the above
- Optionally express it as an ArchUnit-style rule.




