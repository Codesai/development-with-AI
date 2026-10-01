.PHONY: build run test

build:
	docker compose build

run:
	docker compose up --build -d

test:
	dotnet test back.Tests/InterestApi.Tests.csproj
