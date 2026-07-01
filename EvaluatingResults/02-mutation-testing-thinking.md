# Exercise 2 - Mutation Testing Thinking Exercise

## Goal

Understand why green tests can be weak.

## Duration

15 minutes

## Instructions

For each mutation, decide whether the current tests would fail.

| Mutation | Would tests fail? |
| --- | --- |
| Change `weightKg <= 5.0` to `weightKg < 5.0` | ? |
| Change `weightKg <= 20.0` to `weightKg < 20.0` | ? |
| Change `return 9.99` to `return 4.99` | ? |
| Change `isPremium || weightKg <= 5.0` to `isPremium && weightKg <= 5.0` | ? |
| Change `return 4.99` to `return 0.0` | ? |

## Expected Answers

| Mutation | Would tests fail? | Meaning |
| --- | --- | --- |
| Change `weightKg <= 5.0` to `weightKg < 5.0` | No | Boundary not tested |
| Change `weightKg <= 20.0` to `weightKg < 20.0` | No | Boundary not tested |
| Change `return 9.99` to `return 4.99` | Yes | Heavy package behavior tested |
| Change `isPremium || weightKg <= 5.0` to `isPremium && weightKg <= 5.0` | Yes | Premium behavior tested |
| Change `return 4.99` to `return 0.0` | Yes | Standard shipping behavior tested |

## Expected Learning

Coverage is not enough.

A line can be executed without the test being strong enough to detect faults.

Mutation testing becomes especially relevant when AI writes both the implementation and the tests.
