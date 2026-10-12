.PHONY: build run test run-tests mutation-test verify open-report stop-report clean

REPORT_DIR ?= coverage-report
REPORT_PORT ?= 8000
REPORT_PID = .report-server.pid

build:
	docker compose build

run:
	docker compose up --build -d

test: run-tests
	@$(MAKE) --no-print-directory open-report

run-tests:
	dotnet test --collect:"XPlat Code Coverage" --results-directory TestResults
	dotnet tool restore
	dotnet reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:coverage-report -reporttypes:"Html;TextSummary"
	@cat coverage-report/Summary.txt

# Codespaces has no local browser: serve the report over HTTP so the forwarded port can be opened.
open-report:
ifdef CODESPACES
	@$(MAKE) --no-print-directory stop-report
	@nohup python3 -m http.server $(REPORT_PORT) --directory $(REPORT_DIR) >/dev/null 2>&1 & echo $$! > $(REPORT_PID)
	@sleep 1
	@"$$BROWSER" http://localhost:$(REPORT_PORT) >/dev/null 2>&1 || true
	@echo "Report served at http://localhost:$(REPORT_PORT) (see the Ports tab). Stop it with: make stop-report"
else
	@(open $(REPORT_DIR)/index.html || xdg-open $(REPORT_DIR)/index.html) >/dev/null 2>&1 || true
endif

stop-report:
	@if [ -f $(REPORT_PID) ]; then kill `cat $(REPORT_PID)` >/dev/null 2>&1 || true; rm -f $(REPORT_PID); fi

mutation-test: test
	dotnet stryker --config-file stryker-config.json

# Tests + mutation testing, then open one landing page linking both reports.
verify: run-tests
	dotnet stryker --config-file stryker-config.json
	rm -rf reports
	mkdir -p reports/mutation
	cp report-index.html reports/index.html
	cp -R coverage-report reports/coverage
	cp "`ls -td StrykerOutput/*/ | head -1`reports/mutation-report.html" reports/mutation/
	@$(MAKE) --no-print-directory open-report REPORT_DIR=reports

clean: stop-report
	-dotnet clean --nologo -v q
	rm -rf TestResults coverage-report StrykerOutput reports
	find . -type d \( -name bin -o -name obj \) -prune -exec rm -rf {} +
