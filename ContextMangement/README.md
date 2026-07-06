Small API that receives interest records and saves them in interests.txt.

Requirements:
- .NET 9 SDK/runtime for development or use the included Docker image.

Run locally (without Docker):
1. Open a terminal in the back folder
2. dotnet restore
3. dotnet run

The API will be available at http://localhost:8080 (inside the container). The front end is also served by the backend at /. (index.html in wwwroot)

Note: if port 8080 on the host is already in use, adjust the mapping in docker-compose.yml.
Build and run with Docker (recommended):
From the project root (where the front/ and back/ folders are):

1. Build the image:
   docker build -t interest-app .

2. Run the container:
   docker run --rm -p 8080:8080 interest-app

Open http://localhost:8080 in the browser; the UI will be available and the POST /api/register endpoint will handle submissions.

Use docker-compose (maps interests.txt for persistence):

1. Create the interests.txt file in the back folder if it doesn't exist:
   touch back/interests.txt

2. Start the service with docker-compose:
   docker compose up --build -d

3. View records on the host in back/interests.txt (it updates as submissions arrive)

Alternatively, use `docker run` with a volume:
   docker run --rm -p 8080:8080 -v $(pwd)/back/interests.txt:/app/interests.txt interest-app

## Exercises

- One-file exercise: practice for working with a single file and keeping the context focused. [Read the description](./01-one-file-exercise.md)
- Modular files exercise: exercise for splitting the work across multiple files and improving context organization. [Read the description](./02-modular-files-exercise.md)
- Skills exercise: exercise for using specialized skills and keeping instructions more focused. [Read the description](./03-skills-exercise.md)
