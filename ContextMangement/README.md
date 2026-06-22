Pequeña API que recibe registros de interés y los guarda en interests.txt.

Requisitos:
- .NET 9 SDK/runtime para desarrollo o usar la imagen Docker incluida.

Ejecutar localmente (sin Docker):
1. Abrir terminal en la carpeta back
2. dotnet restore
3. dotnet run

La API se expondrá en http://localhost:5000 (dentro del contenedor). El front también se sirve desde el backend en /. (index.html en wwwroot)

Nota: si el puerto 5000 del host está ocupado, el compose predeterminado mapea el host 5001 al contenedor 5000 (ver docker-compose.yml).
Construir y ejecutar con Docker (recomendado):
Desde la raíz del proyecto (donde están las carpetas front/ y back/):

1. Construir la imagen:
   docker build -t interest-app .

2. Ejecutar el contenedor:
   docker run --rm -p 5000:5000 interest-app

Abrir http://localhost:5000 en el navegador; la UI estará disponible y el endpoint POST /api/register gestionará los envíos.

Usar docker-compose (mapea interests.txt para persistencia):

1. Crear el fichero interests.txt en la carpeta back si no existe:
   touch back/interests.txt

2. Levantar el servicio con docker-compose:
   docker compose up --build -d

3. Ver registros en el host en back/interests.txt (se actualiza conforme lleguen envíos)

Alternativamente, usar `docker run` con volumen:
   docker run --rm -p 5000:5000 -v $(pwd)/back/interests.txt:/app/interests.txt interest-app

Cosas pendientes:
   - eliminar todas las validaciones tanto en front como en back para el proyecto de partida
   - añadir un poquito de arquitectura en el back, separar controlador-dominio-repositorio al menos, algo muy sencillo
   - crear el proyecto en el github de codesai para el curso
   - Slides explicando los ficheros de contexto (explicar un poco los que son particulares de github copilot y los que son genericos para cualquier agente)
   - planteamiento del primer ejercicio:
      - Gestión del contexto con ficheros (en un sólo y estructurado por directorios)
      - Validación de email con reglas para el front y el back
      - Enunciado y pruebas
   - Slides para skills
   - planteamiento del segundo ejercicio:
      - Gestión del contexto mediante skills (intención vs posición)
      - El mismo ejercicio que el anterior pero solucionandolo con skills así nos centramos en el mecanismo de skills dado un problema ya resuelto
      - Enunciado y pruebas
