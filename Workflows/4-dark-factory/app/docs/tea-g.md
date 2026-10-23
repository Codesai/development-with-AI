# Guía del profesorado — Ejercicio 3: Complete a local Dark Factory

## Propósito de la sesión

Este ejercicio enseña que una fábrica de software autónoma necesita algo más que un agente capaz de modificar código. Para cerrar el ciclo hacen falta dos mecanismos de control explícitos:

1. una revisión independiente que compare el resultado con la intención y produzca evidencia;
2. un controlador que seleccione la siguiente tarea a partir de estado durable y sepa cuándo detenerse.

El resultado esperado es este bucle:

```mermaid
flowchart TD
    T["Seleccionar siguiente tarea"]
    G["Leer objetivo y criterios de aceptación"]
    A["Investigar, planificar e implementar"]
    F["Ejecutar sensores de feedback"]
    E{"Decisión del coordinador"}
    X["Fix separado"]
    H["STOP y decisión humana"]
    I["Registrar DONE en main"]

    T --> G
    G --> A
    A --> F
    F --> E
    E -- FIX --> X
    X --> F
    E -- STOP --> H
    E -- GO --> I
    I -- Releer estado --> T
```

```mermaid
flowchart TD
    E{"Coordinator evaluates"}

    E -- "GO" --> L["Document DONE on main"]
    L --> T["Re-read state and select next task"]

    E -- "FIX" --> X["Separate fixer"]
    X --> V["Validate"]
    V --> Q["Fresh review"]
    Q --> E

    E -- "STOP: ambiguity / risk / exhausted fixes" --> H["Human review and re-steer"]
```

La revisión no sustituye a los tests y los tests no sustituyen a la revisión. `make validate` responde si el sistema satisface los sensores automatizados existentes. La revisión responde si el cambio actual satisface la tarea actual y si es razonable darlo por completado.


| Diagrama | Dark Factory |
| :--- | :--- |
| Select next task | Task Selection Phase |
| Define Goal / Acceptance criteria | Archivo `.dark-factory/tasks/NNN-*.md` |
| Agent Plan and Act | Research → Plan → Implement |
| Change system | Implementación directamente en `main` |
| Run feedback sensors | `make validate` + revisión independiente |
| Evaluate | Review Phase and Decision Gate del coordinador |
| Correction needed | Decisión FIX → fixer → validación → nueva revisión |
| Uncertain / high-risk | Decisión STOP y solicitud de dirección humana |
| Done condition met | GO → documentación → DONE en `main` |
| Complete task → Next task | Releer repositorio/run log y seleccionar la siguiente tarea |


## Preparación del profesorado

Desde `app/`:

```bash
make factory-preflight
git status --short
```

Antes de comenzar, compruebe:

- que el preflight termina con código `0`;
- que `main` está limpio;
- que `.dark-factory/config` contiene límites pequeños para el aula;
- que `.dark-factory/run-log.md` no contiene resultados de una ejecución anterior;
- que las tareas de `.dark-factory/tasks/` están disponibles y ordenadas léxicamente.

La configuración inicial prevista es:

```text
MAX_TASKS=3
MAX_FIX_ROUNDS=2
```

Durante el ejercicio se cambia y se prueba el propio workflow.

## Resultado conceptual esperado

El alumnado debe distinguir las siguientes responsabilidades:

| Rol | Puede escribir código | Puede operar Git | Puede decidir | Responsabilidad |
|---|---:|---:|---:|---|
| Researcher | No | No | No | Recopilar contexto y evidencia |
| Planner | No | No | No | Proponer el cambio mínimo y sus riesgos |
| Implementer | Sí | No | No | Implementar código y checks |
| Validator | No | No | No | Ejecutar `make validate` y reportar el resultado |
| Reviewer | No | No | No | Inspeccionar y clasificar findings |
| Fixer | Sí | No | No | Corregir findings requeridos |
| Coordinator | No edita producto | Sí | Sí | Controlar fases y elegir `GO`, `FIX` o `STOP` |

El coordinador es responsable de la decisión, pero no puede convertir incertidumbre en permiso. Una decisión relevante de producto o arquitectura que no esté documentada conduce a `STOP` y devuelve el control a una persona.

## Parte 1 — Solución de la fase de revisión

### Tabla de decisión de referencia

La política debe poder ejecutarse sin interpretación adicional. Esta tabla es una posible solución:

| Estado observado | Rondas disponibles | Decisión | Acción siguiente |
|---|---:|---|---|
| Revisión limpia o solo `SUGGESTION` | Cualquiera | `GO` | Registrar DONE en `main` |
| Algún `BLOCKING` o `IMPORTANT` corregible | Sí | `FIX` | Fixer → validator → reviewer nuevo |
| Algún `BLOCKING` o `IMPORTANT` corregible | No | `STOP` | Registrar evidencia y detener la fábrica |
| Requisito o decisión de producto/arquitectura no clara | Cualquiera | `STOP` | Pedir decisión humana |
| Operación insegura o destructiva necesaria | Cualquiera | `STOP` | Pedir decisión humana |

Una `SUGGESTION` se registra, pero nunca fuerza `FIX`. Un `IMPORTANT` sí lo fuerza: permitir que se marque DONE convertiría la clasificación en una recomendación opcional.

### Qué buscar durante la puesta en común

- El reviewer recibe una perspectiva fresca y es de solo lectura.
- Cada finding incluye clasificación, ubicación o comportamiento afectado y evidencia concreta.
- El reviewer informa; no corrige ni decide la finalización.
- El coordinador aplica la tabla de decisión de forma mecánica.
- Después de un fix se repiten los dos sensores: validación y revisión fresca.
- El número de rondas se limita con `MAX_FIX_ROUNDS`.

## Parte 2 — Demostración controlada

### Secuencia docente

1. Pedir la implementación de Task 001 hasta validación, sin revisión ni registro de finalización.
2. Confirmar que `make validate` pasa en `main`.
3. Cambiar temporalmente el criterio de `{ "status": "ok" }` a `{ "status": "ready" }`.
4. Ejecutar únicamente la revisión y la puerta de decisión.
5. Confirmar que aparece un finding requerido y que la decisión es `FIX`.
6. Restaurar `{ "status": "ok" }` sin guardar el cambio temporal en un commit.
7. Ejecutar una revisión fresca y confirmar `GO`.
8. Terminar Task 001 registrando su finalización en `main`, sin iniciar Task 002.

### Evidencia esperada

Una respuesta válida al probe debe ser equivalente a:

```text
Validation: PASS — make validate (exit 0)
Finding: IMPORTANT — .dark-factory/tasks/001-health-endpoint.md exige
{ "status": "ready" }, pero el endpoint y sus tests usan { "status": "ok" }.
Decision: FIX
```

La validación continúa verde porque los tests y la implementación coinciden entre sí; ambos conservan la expectativa antigua. La revisión detecta que ese conjunto coherente ya no coincide con el criterio de aceptación modificado.

No debe aceptarse como resultado correcto:

- que el reviewer edite el código;
- que el coordinador elija `GO` porque los tests pasan;
- que se cambie el test para ocultar la discrepancia;
- que el cambio temporal de la tarea llegue a un commit;
- que comience Task 002 antes de terminar la prueba controlada.

## Parte 3 — Solución del controlador

El controlador no debe construir una cola una sola vez y consumirla en memoria. Después de cada tarea completada vuelve a leer:

- `.dark-factory/config`;
- `.dark-factory/run-log.md`;
- `.dark-factory/tasks/`;
- el estado de `main` y la tarea activa.

El log es el registro de finalización. Un commit existente no prueba que una tarea haya terminado: puede proceder de un intento interrumpido.

### Algoritmo de referencia

```text
attempted_this_run = 0

loop:
  releer configuración, repositorio, tareas y run log

  si existe una tarea con resultado terminal STOPPED:
    terminar toda la ejecución

  si existe una tarea activa no terminal:
    continuar únicamente esa tarea o detenerse si su estado es ambiguo

  si attempted_this_run == MAX_TASKS:
    terminar por límite configurado

  ordenar léxicamente los archivos de tarea
  seleccionar el primero sin resultado terminal DONE o STOPPED

  si no existe:
    terminar porque la cola está vacía

  attempted_this_run += 1
  ejecutar el workflow completo de esa tarea

  si termina STOPPED:
    terminar toda la ejecución

  si termina DONE:
    volver al principio y releer todo el estado
```

Los contadores del informe describen la ejecución actual. Deben derivarse de los resultados observados durante ella, conservando en el log la evidencia durable de cada tarea.

## Solución propuesta para `app/AGENTS.md`

El siguiente contenido es una solución de referencia completa. No es necesario exigir una redacción idéntica, pero sí todos sus invariantes.

```markdown
# Local Dark Factory guidance

## Rules

Before starting, read `.dark-factory/config`, `.dark-factory/run-log.md`, this file, the selected task, and the current repository state. Work directly on `main` using trunk-based development. Start each task from a clean working tree and keep only one task active at a time.

The coordinator alone commits and updates `.dark-factory/run-log.md`. All implementation, fixes, and evidence commits happen directly on `main`.

The available roles are read-only researcher, read-only planner, implementer, non-editing validator, fresh read-only reviewer, and fixer. Editors, validators, and Git operations must never overlap.

Run the authoritative `make validate` command. Do not delete, skip, weaken, or bypass tests, checks, assertions, architecture rules, formatting, or compiler errors to obtain a pass.

Use Conventional Commits, for example `feat(TASK-NNN): add health endpoint`, `fix(TASK-NNN): reject empty fields`. Keep commands, exit results, findings, fixes, implementation commit and completion result observable in Git and the run log.

## Factory controller

At the start of the run and after every completed task, the coordinator must reread `.dark-factory/config`, `.dark-factory/run-log.md`, the task directory, the repository status and active task state on `main`. Never continue from an initial in-memory task list.

Read `MAX_TASKS` and `MAX_FIX_ROUNDS` from `.dark-factory/config` as non-negative integers. Stop if either value is missing or invalid. Counters apply to the current factory run. A task becomes attempted when implementation begins on `main` or when the run resumes that task as its single active non-terminal task.

Discover regular task files in `.dark-factory/tasks/` and order their paths lexically. For each task, use the latest corresponding section in `.dark-factory/run-log.md` as its durable outcome. `DONE` and `STOPPED` are terminal. Do not edit task definitions to mark progress and never infer completion from a commit, working tree, or chat transcript alone.

Before selecting work, if the run log contains a terminal `STOPPED` task, stop the whole run and report it. If one non-terminal task is active, do not select another: resume only that task when its state and next phase are unambiguous; otherwise stop and request human direction. If more than one task appears active, stop because the single-active-task invariant is broken.

If the number attempted in this run has reached `MAX_TASKS`, finish. Otherwise select the first lexically ordered task without a terminal `DONE` or `STOPPED` entry. If none exists, finish because the queue is empty. Execute the complete per-task workflow. After `DONE`, reread all durable state before selecting again. After `STOPPED`, stop the whole run; never skip the task and continue.

## Per-task workflow

1. Task Selection Phase: Apply the factory controller. On clean `main`, select exactly one task and work directly on it.
2. Research Phase: Research the task, acceptance criteria, related code, architecture, analogous behavior, tests, and validation commands. Do not edit.
3. Plan Phase: Plan the smallest change: files, behavior, tests, implications, and risks. Do not edit or request approval.
4. Implement Phase: Implement the plan and automated checks. Do not perform Git operations or edit the run log.
5. Validate Phase: Inspect the diff and have the coordinator commit the implementation. Then run `make validate` with a non-editing validator. Record the exact command, exit code, and diagnostics. The validator may create ignored build output, but must not change sources, commits, the index, or the run log. A failed validation must not proceed to review or completion; record the failure and stop the task unless a correction explicitly allowed by this workflow remains.
6. Review Phase: After passing validation, use a fresh reviewer that did not implement or fix the change. The reviewer is read-only and may not edit files, fix findings, commit, update the run log. It must inspect the task acceptance criteria and the complete diff for correctness, regressions, architecture, automated checks, error handling, security, maintainability, unnecessary complexity, and scope.
7. Finding Contract: For every finding, report `BLOCKING`, `IMPORTANT`, or `SUGGESTION`; identify the affected requirement, file, line, or behavior; provide concrete evidence; and explain the consequence. `BLOCKING` means unsafe, incorrect, or impossible to complete. `IMPORTANT` means a required defect or omission that must be corrected before completion. `SUGGESTION` is optional and does not prevent completion. Report explicitly when no findings exist.
8. Decision Gate: The coordinator, not the reviewer, owns the decision and records its evidence. Choose `GO` only when there are no unresolved `BLOCKING` or `IMPORTANT` findings; suggestions are recorded but do not force repair. Choose `FIX` when at least one required finding is actionable and fewer than `MAX_FIX_ROUNDS` have been used. Choose `STOP` when a required finding remains and no fix round is available, when a product or architecture decision is unclear or undocumented, when safe correction is not possible, or when another stop condition applies. Never allow unresolved `BLOCKING` or `IMPORTANT` findings to reach completion.
9. Fix Loop: On `FIX`, a separate fixer changes only what is needed to resolve the required findings and adds or updates automated checks where appropriate. The coordinator commits the fix using Conventional Commits and increments the fix-round count. Then the same non-editing validation contract runs `make validate` again and a fresh read-only reviewer performs a complete new review. Return to the Decision Gate. Do not reuse the previous review conclusion. Suggestions may be left unresolved but must remain recorded.
10. Document Phase: After `GO`, add one entry using the required shape in `.dark-factory/run-log.md`, above its marker, and commit only that evidence on `main`. It must include `Status: DONE`, `Review: CLEAN`, and `Validation: PASS`. Record the implementation or fix tip before this log-only commit. Include all review classifications and suggestions in the concise findings summary. Confirm clean `main`, then return control to the factory controller.

## Stop and finish

Stop the factory immediately—do not continue to later tasks or complete failed work—when the baseline is unexpectedly red; a requirement needs a meaningful undocumented product or architecture decision; an unsafe or destructive operation is required; the workflow invariants or durable state are ambiguous; a required review finding remains after `MAX_FIX_ROUNDS`. When safe, add a `Status: STOPPED` entry on `main` and preserve all evidence. A stopped task stops the whole run.

Finish when the queue is empty, `MAX_TASKS` tasks have been attempted in the current run, the user-requested explicitly bounded task is complete, or the factory stops. Report the finish reason; available, attempted, completed, stopped, failed, validation-failure, review-finding, and fix-round counts; any stop condition; the last completed task; `main` cleanliness; the exact last validation command and result; and `git log --graph --oneline --decorate --all`.
```

## Observaciones sobre la solución propuesta

### Por qué `GO` no significa todavía “tarea completada”

`GO` permite registrar la finalización. La tarea queda completada cuando:

1. `make validate` ha terminado satisfactoriamente;
2. una revisión fresca no tiene findings requeridos abiertos;
3. el run log registra `Status: DONE` y el commit de implementación o fix;
4. el commit de evidencia está en `main` y el árbol de trabajo queda limpio.

El log debe reflejar comprobaciones ya realizadas. El historial Git permite contrastar el commit de implementación y el commit de evidencia con lo registrado.

## Rúbrica sugerida

| Criterio | Puntos |
|---|---:|
| Reviewer fresco, independiente y estrictamente de solo lectura | 1.5 |
| Findings con severidad y evidencia concreta | 1.0 |
| Tabla o reglas inequívocas para `GO`, `FIX` y `STOP` | 1.5 |
| Fixer separado y repetición de validación y revisión fresca | 1.0 |
| Selección léxica basada en tareas y run log | 1.5 |
| Relectura de estado después de cada tarea completada | 1.0 |
| Límite `MAX_TASKS`, límite de fixes y parada global tras `STOPPED` | 1.0 |
| Evidencia Git/run log e informe final completo | 1.0 |
| Explicación correcta del probe verde con requisito incumplido | 0.5 |
| **Total** | **10** |

Errores críticos que impiden considerar completado el ejercicio:

- marcar DONE con un `BLOCKING` o `IMPORTANT` abierto;
- permitir que reviewer o validator modifiquen fuentes;
- continuar con otra tarea después de `STOPPED`;
- inferir que una tarea está terminada solo porque existe un commit;
- evitar o debilitar `make validate` para conseguir verde;
- trabajar en más de una tarea a la vez.

## Preguntas para el cierre

1. ¿Qué detectó el reviewer que los tests no podían detectar en el probe?
2. ¿Por qué quien encuentra un problema no debe decidir también su finalización?
3. ¿Qué estado durable debe releerse en cada vuelta del bucle?
4. ¿Cómo podría un intento interrumpido confundir a un controlador ingenuo?
5. ¿Qué decisiones siguen necesitando intervención humana?
6. ¿Qué hechos están garantizados por Git y los comandos, y cuáles son solo texto escrito en el log?

La idea final que debería quedar es: autonomía no significa ausencia de control humano; significa que las decisiones rutinarias están codificadas y que las decisiones ambiguas escalan de forma explícita y segura.
