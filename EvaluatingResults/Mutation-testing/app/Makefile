.PHONY: build run test run-tests analyze run-analysis mutation-test verify open-report stop-report clean

REPORT_DIR ?= coverage-report
REPORT_PORT ?= 8000
REPORT_PID = .report-server.pid

build:
	docker compose build

run:
	docker compose up --build -d

test: run-tests
	@$(MAKE) --no-print-directory open-report

analyze: run-analysis
	@$(MAKE) --no-print-directory open-report REPORT_DIR=analysis-report

# Build with the Roslyn analyzers writing SARIF logs (one per project), then render them as HTML.
run-analysis:
	rm -rf analysis
	mkdir -p analysis
	dotnet build back/InterestApi.csproj --no-incremental "-p:ErrorLog=$(CURDIR)/analysis/back.sarif%2cversion=2.1"
	dotnet build tests/RegistrationDemo.Api.Tests/RegistrationDemo.Api.Tests.csproj --no-incremental --no-dependencies "-p:ErrorLog=$(CURDIR)/analysis/tests.sarif%2cversion=2.1"
	python3 scripts/sarif_to_html.py analysis analysis-report

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
	dotnet tool restore
	dotnet stryker --config-file stryker-config.json

# Static analysis + tests + mutation testing, then open one landing page linking the three reports.
verify: run-analysis run-tests
	dotnet tool restore
	dotnet stryker --config-file stryker-config.json
	rm -rf reports
	mkdir -p reports/mutation
	cp report-index.html reports/index.html
	cp -R coverage-report reports/coverage
	cp -R analysis-report reports/analysis
	cp "`ls -td StrykerOutput/*/ | head -1`reports/mutation-report.html" reports/mutation/
	@$(MAKE) --no-print-directory open-report REPORT_DIR=reports

clean: stop-report
	-dotnet clean --nologo -v q
	rm -rf TestResults coverage-report StrykerOutput reports analysis analysis-report
	find . -type d \( -name bin -o -name obj \) -prune -exec rm -rf {} +
