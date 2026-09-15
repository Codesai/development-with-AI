.PHONY: build run test mutation-test

build:
	docker compose build

run:
	docker compose up --build -d

test:
	dotnet test

mutation-test: test
	dotnet stryker --config-file stryker-config.json
