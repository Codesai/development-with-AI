.PHONY: build test validate run deployment-config

build:
	docker compose build

deployment-config:
	docker compose config --quiet

test:
	dotnet test tests/InterestApi.Tests.csproj

validate: build deployment-config test

run:
	docker compose up --build -d
