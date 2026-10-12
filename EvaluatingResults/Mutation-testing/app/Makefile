.PHONY: build run test mutation-test

build:
	docker compose build

run:
	docker compose up --build -d

test:
	dotnet test --collect:"XPlat Code Coverage" --results-directory TestResults
	dotnet tool restore
	dotnet reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:coverage-report -reporttypes:"Html;TextSummary"
	@cat coverage-report/Summary.txt

mutation-test: test
	dotnet stryker --config-file stryker-config.json
