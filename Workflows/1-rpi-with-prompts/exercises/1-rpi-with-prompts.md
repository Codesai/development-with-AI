# Workflows – Exercise 1 – Simple prompt vs. structured workflow

## Learning goal

See how explicit research, planning, implementation, and validation phases change an agent's outcome, even for a small feature request.

## Part 1

- Go to the `../app` directory,
- Run `copilot` and enter this prompt:
```text
Add a comments field to the registration form, with validation to reject characters that could corrupt the file where registrations are stored.
``` 

**Opción en español:**
```text
Añade un campo de comentarios al formulario de inscripción, con validación para rechazar caracteres que puedan corromper el fichero donde se guardan las inscripciones.
```

- Save in a file to compare later: agent log, changed files, added checks, and commands run.

## Part 2

- Revert every change made in Part 1.
- In a new session, launch `copilot` and paste the following prompt:

```text
Add a comments field to the registration form, with validation to reject characters that could corrupt the file where registrations are stored.

Workflow:
1. Research the application’s architecture and conventions, and clarify any uncertainty about the feature with me.
2. Wait for my approval.
3. Propose an implementation and testing plan.
4. Wait for my approval.
5. Implement the approved plan.
6. Run the validation checks.
7. Fix any issues found.
8. Report a concise summary, including the relevant details, changed files, and validation results.
```

**Opción en español:**
```text
Añade un campo de comentarios al formulario de inscripción, con validación para rechazar caracteres que puedan corromper el fichero donde se guardan las inscripciones.

Flujo de trabajo:
1. Investiga la arquitectura y las convenciones de la aplicación, y aclara conmigo cualquier duda sobre la funcionalidad.
2. Espera mi aprobación.
3. Propón un plan de implementación y pruebas.
4. Espera mi aprobación.
5. Implementa el plan aprobado.
6. Ejecuta las comprobaciones de validación.
7. Corrige los problemas encontrados.
8. Presenta un resumen conciso con los detalles relevantes, los ficheros modificados y los resultados de la validación.
```

- When Copilot pauses, review its response and approve it if you agree.

## Success and reflection

Both implementations should let visitors submit comments with their registration, reject characters that could corrupt the storage file, and preserve the existing registration behavior.

- Compare how Copilot worked with the simple prompt and the structured prompt. Identify which differences came from the workflow.
- How complete were the implemented validation checks?
