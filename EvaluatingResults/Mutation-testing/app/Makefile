.PHONY: build run test mutation-test clean

build:
	docker compose build

run:
	docker compose up --build -d

test:
	dotnet test --collect:"XPlat Code Coverage" --results-directory TestResults
	dotnet tool restore
	dotnet reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:coverage-report -reporttypes:"Html;TextSummary"
	@cat coverage-report/Summary.txt
	@(open coverage-report/index.html || xdg-open coverage-report/index.html) >/dev/null 2>&1 || true

mutation-test: test
	dotnet stryker --config-file stryker-config.json

clean:
	-dotnet clean --nologo -v q
	rm -rf TestResults coverage-report StrykerOutput
	find . -type d \( -name bin -o -name obj \) -prune -exec rm -rf {} +
