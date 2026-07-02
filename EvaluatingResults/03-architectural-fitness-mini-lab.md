# Exercise 3 - Architectural Fitness Mini-Lab

## Goal

Translate architectural intent into executable rules.

## Duration

20 minutes

## Setup

Use the prepared folders in `main`. Students do not need to copy or create the code manually.

## Steps

- `03-Architecture/Step1`: generated code with an architectural violation. Read `03-Architecture/Step1/README.md`.
- `03-Architecture/Step2`: same code with a failing architecture fitness test. Read `03-Architecture/Step2/README.md`.
- `03-Architecture/Step3`: corrected design with the architecture fitness test passing. Read `03-Architecture/Step3/README.md`.

## Scenario

The project follows this layered architecture:

```text
api -> application
application -> domain
domain -> no framework dependencies
```
