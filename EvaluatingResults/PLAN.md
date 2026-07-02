# Migrar ejercicios de ramas a carpetas en `main`

## Resumen

Convertir los ejercicios actuales basados en ramas `evaluation/*` a una estructura navegable por carpetas en `main`, usando pasos autocontenidos:

- `Testing/Step1`, `Testing/Step2`, `Testing/Step3`
- `Mutation/Step1`, `Mutation/Step2`
- `Architecture/Step1`, `Architecture/Step2`, `Architecture/Step3`

Cada `Step` contendrá el proyecto completo correspondiente a su rama original, de modo que el alumnado pueda abrir, compilar y ejecutar cada estado sin hacer `git switch`.

## Cambios clave

- Exportar el contenido de estas ramas a carpetas:
  - `evaluation/tests/01-no-tests` -> `Testing/Step1`
  - `evaluation/tests/02-my-ai-generated-tests` -> `Testing/Step2`
  - `evaluation/tests/03-complete-review-solution` -> `Testing/Step3`
  - `evaluation/mutation/01-finding-mutants` -> `Mutation/Step1`
  - `evaluation/mutation/02-finding-mutants-solution` -> `Mutation/Step2`
  - `evaluation/architecture/01-finding-violation` -> `Architecture/Step1`
  - `evaluation/architecture/02-fitness-test-red` -> `Architecture/Step2`
  - `evaluation/architecture/03-fitness-test-green` -> `Architecture/Step3`

- En cada carpeta `Step`, copiar solo los assets ejecutables del ejercicio:
  - soluciones/proyectos `.sln` o `.slnx`
  - `global.json` si existe
  - `src/`
  - `tests/`
  - `stryker-config.json` si existe
  - notas especificas de solucion cuando existan, por ejemplo la solucion de mutation

- No duplicar dentro de cada `Step` los documentos raiz actuales (`README.md`, `01-*`, `02-*`, `03-*`) salvo que sean notas especificas del paso.
- las notas del 01*, etc que sean para explicar brevemente en 

- Actualizar el `README.md` raiz para que sea el indice principal:
  - explicar que todos los ejercicios estan en `main`
  - brevemente que hay 3 ejercicios
  - hacer referencia  las 01* , y no entrar en detalle de lo que hay que hacer.
  - eliminar instrucciones basadas en ramas

- Actualizar las guias:
  - `01-review-ai-generated-change.md`: sustituir ramas por `Testing/Step1..Step3`
  - `02-mutation-testing-thinking.md`: sustituir ramas por `Mutation/Step1..Step2`
  - `03-architectural-fitness-mini-lab.md`: sustituir ramas por `Architecture/Step1..Step3`
  - cambiar comandos tipo `git switch ...` por `cd Testing/Step1`, `dotnet test`, `dotnet stryker`, etc.
  - en los 01*, etc, explicar muy brevemente cada step1 y hacer referencia al README DE DENTRO.

- Dentro de cada Step1 tiene que haber un nuevo README que explique lo que hay que hacer.

## Implementacion

- Usar exportacion desde Git para poblar carpetas desde cada rama, evitando mezclar el working tree actual.
- Crear la estructura en `main` y mantener nombres simples `Step1`, `Step2`, `Step3`.
- Validar que las soluciones siguen funcionando desde dentro de cada carpeta `Step`.
- Dejar las ramas antiguas sin borrar en esta primera migracion para compatibilidad temporal.

## Plan de pruebas

- Ejecutar `dotnet test` en:
  - `Testing/Step1`
  - `Testing/Step2`
  - `Testing/Step3`
  - `Mutation/Step1`
  - `Mutation/Step2`
  - `Architecture/Step2`
  - `Architecture/Step3`

- Ejecutar o al menos validar configuracion de Stryker en:
  - `Mutation/Step1`
  - `Mutation/Step2`

- Para `Architecture/Step1`, validar build si no tiene tests todavia.

- Revisar que ningun documento raiz siga diciendo que el alumnado debe cambiar de rama.

## Supuestos

- La estructura final vive en `main`.
- Las ramas antiguas se mantienen temporalmente y no se eliminan en esta tarea.
- Los pasos deben ser proyectos completos y autocontenidos, no diffs parciales.
- Los nombres visibles seran en ingles y con mayuscula inicial: `Testing`, `Mutation`, `Architecture`, `Step1`, `Step2`, `Step3`.
