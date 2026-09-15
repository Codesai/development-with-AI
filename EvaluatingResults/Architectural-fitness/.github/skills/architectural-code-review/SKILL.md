---
name: architectural-code-review
description: use this skill when the user wants to perform an architecture code review 
---

The project contain three namespaces: Controllers, Domain and Repository

The following dependencies are no allowed:
  - Controllers cannot depend on Repository
  - Controllers cannot depend on Domain
  - Domain cannot depend on Controllers
  - Repository cannot depend on Controllers

Show a result with all the places in the code that violates this rules