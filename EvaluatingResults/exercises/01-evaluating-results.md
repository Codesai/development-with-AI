# Exercise 1 — Evaluating Results

# Objetivo

Practicar y revisar el resultado que ofrecen distintas herramientas de verificación que nos permiten analizar el código generado por la IA y ayudarnos en la fase de revisión.

Entender cómo, gracias al uso de distintas herramientas de verificación, podemos mejorar nuestra forma de revisar el código generado por la IA y aumentar la calidad de nuestros proyectos.

# Paso 0

La aplicación es la misma que en ejercicios anteriores con un pequeño añadido, dispone de una nueva funcionalidad que solo permite
registrar 30 alumnos por curso. Esta nueva funcionalidad tiene el objetivo de añadir un poco de lógica al proyecto para poder practicar con las herramientas de evaluación sobre un ejemplo algo más realista. Los verificadores incluidos son:

    - Test funcionales (unit, integration y e2e)
    - Cobertura de test y análisis CRAP
    - Mutation Testing
    - Architectural Testing
    - Static Code Analysis

Antes de empezar, ejecuta todos los evaluadores:

- make verify (en el directorio EvaluatingResults/app)

# Paso 1 - Revisar el informe de cobertura

Un informe de cobertura nos indica qué líneas y ramas lógicas de nuestro código están cubiertas por test.

    - Revisar el informe
    - La clase RegistrationController no tiene 100% de cobertura, eso significa que hay casos que no están cubiertos por los
    test (los test que cubren esta clase están en el fichero de test RegistrationEndToEndTests)
    - Razonar qué test se podrían añadir para cubrir los casos no cubiertos (¡sin utilizar la IA!).
    - Una vez razonado, podemos abrir Copilot y pedir a la IA que nos ayude a detectar qué test se podrían añadir
    para conseguir cubrir todo el código. Comparar nuestra respuesta con la que ofrece la IA.

# Paso 2 - Revisar el informe de mutation testing

La cobertura nos dice qué líneas y ramas están cubiertas por test, pero no nos asegura que esos test estén bien implementados y
realicen las comprobaciones adecuadas; simplemente nos dice que, al ejecutar el test, el código pasó por esas líneas. Mutation testing es una herramienta que permite evaluar la calidad de nuestros test para detectar si nos protegen o no ante posibles bugs que se puedan introducir en el código.

Revisar el informe de mutation testing para la clase "RegistrationPolicy" para ver los mutantes (modificaciones realizadas sobre el código) que hemos conseguido matar (nuestros test nos protegen ante esos cambios) y los que han sobrevivido. Estos últimos son los más interesantes, ya que nos dan información sobre los potenciales bugs que se podrían introducir sin que ningún test los detecte.

    - Intentar razonar qué test habría que añadir o modificar para matar al mutante que ha sobrevivido.
    - Realizar el cambio manualmente o con ayuda de la IA y lanzar la verificación nuevamente para comprobar si hemos conseguido matar al mutante.

# Paso 3 - Revisar el informe de test de arquitectura

Los test de arquitectura nos protegen ante código que pudiendo ser funcionalmente correcto viola alguna regla de arquitectura/diseño establecida en nuestro proyecto. 

    - Revisar el fichero LayerDependencyTests para entender qué dependencias entre las distintas capas de la aplicación no están
    permitidas según este test.
    - Verificar el resultado de los test de arquitectura
    - Intentar razonar qué violación se está cometiendo y pensar en qué refactorización habría que hacer para evitar esta violación (¡sin usar la IA!).
    - Abrir Copilot y preguntarle a la IA por posibles soluciones para este caso y compararlas con las nuestras.
    - Implementar la solución que más nos convenza y ejecutar de nuevo el proceso de verificación para comprobar que ahora los test
    de arquitectura no detectan violaciones

# Paso 4 - Static Code Analysis

El análisis estático de código nos permite detectar malas prácticas en nuestro código y oportunidades de mejora.

    - Revisar el resultado de este informe y tratar de entender y razonar sobre los avisos que nos proporciona el análisis.
    - Implementar al menos dos mejoras en el código basadas en el resultado de este análisis.
