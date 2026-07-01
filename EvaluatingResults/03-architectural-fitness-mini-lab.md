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
api -> application -> domain
infrastructure -> application
domain -> no framework dependencies
```

Generated code:

```kotlin
package com.acme.domain.order

import org.springframework.web.client.RestTemplate

class OrderRiskPolicy(
    private val restTemplate: RestTemplate
) {
    fun isRisky(orderId: String): Boolean {
        return restTemplate.getForObject(
            "https://risk.example.com/orders/$orderId",
            Boolean::class.java
        ) ?: false
    }
}
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

## Expected Answer

The domain layer now depends on Spring and an external HTTP client.

That couples domain logic to infrastructure and makes the domain harder to test, reuse, and reason about.

Possible rule:

> Classes in `..domain..` must not depend on Spring, HTTP clients, persistence frameworks, or infrastructure packages.

Example ArchUnit-style rule:

```java
@ArchTest
static final ArchRule domain_should_not_depend_on_spring =
    noClasses().that().resideInAPackage("..domain..")
        .should().dependOnClassesThat().resideInAnyPackage("org.springframework..");
```

## Expected Learning

Putting the rule in `AGENTS.md` is useful, but it is not deterministic.

If the rule is architecturally important, it should also become an executable check.

The strongest version is:

- document the rule for humans and agents
- review for it manually
- enforce it through CI
