.PHONY: build run test mutation-test open-report stop-report clean

REPORT_PORT ?= 8000

build:
	docker compose build

run:
	docker compose up --build -d

test:
	dotnet test --collect:"XPlat Code Coverage" --results-directory TestResults
	dotnet tool restore
	dotnet reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:coverage-report -reporttypes:"Html;TextSummary"
	@cat coverage-report/Summary.txt
	@$(MAKE) --no-print-directory open-report

# Codespaces has no local browser: serve the report over HTTP so the forwarded port can be opened.
open-report:
ifdef CODESPACES
	@$(MAKE) --no-print-directory stop-report
	@nohup python3 -m http.server $(REPORT_PORT) --directory coverage-report >/dev/null 2>&1 &
	@sleep 1
	@"$$BROWSER" http://localhost:$(REPORT_PORT) >/dev/null 2>&1 || true
	@echo "Report served at http://localhost:$(REPORT_PORT) (see the Ports tab). Stop it with: make stop-report"
else
	@(open coverage-report/index.html || xdg-open coverage-report/index.html) >/dev/null 2>&1 || true
endif

stop-report:
	@pkill -f "http.server $(REPORT_PORT) --directory coverage-report" >/dev/null 2>&1 || true

mutation-test: test
	dotnet stryker --config-file stryker-config.json

clean: stop-report
	-dotnet clean --nologo -v q
	rm -rf TestResults coverage-report StrykerOutput
	find . -type d \( -name bin -o -name obj \) -prune -exec rm -rf {} +
