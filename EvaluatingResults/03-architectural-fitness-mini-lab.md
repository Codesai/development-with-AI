# Exercise 3 - Architectural Fitness Mini-Lab

## Goal

Translate architectural intent into executable rules.

## Duration

20 minutes

## Setup

Use the prepared folders in `main`. Students do not need to copy or create the code manually.

## Steps

- `Architecture/Step1`: generated code with an architectural violation. Read `Architecture/Step1/README.md`.
- `Architecture/Step2`: same code with a failing architecture fitness test. Read `Architecture/Step2/README.md`.
- `Architecture/Step3`: corrected design with the architecture fitness test passing. Read `Architecture/Step3/README.md`.

## Scenario

The project follows this layered architecture:

```text
api -> application
application -> domain
application -> infrastructure
domain -> no framework dependencies
```
