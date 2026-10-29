.PHONY: build run stop test run-tests architecture-test code-review analyze run-analysis mutation-test verify open-report stop-report clean

REPORT_DIR ?= coverage-report
REPORT_PORT ?= 8000
REPORT_PID = .report-server.pid
ARCH_FILTER = FullyQualifiedName~Tests.Architecture

build:
	docker compose build

run:
	docker compose up --build -d

stop:
	docker compose down

test: run-tests
	@$(MAKE) --no-print-directory open-report

analyze: run-analysis
	@$(MAKE) --no-print-directory open-report REPORT_DIR=analysis-report

# Build with the Roslyn analyzers writing SARIF logs (one per project), then render them as HTML.
run-analysis:
	@cat scripts/build-banner.txt
	rm -rf analysis
	mkdir -p analysis
	dotnet build back/InterestApi.csproj --no-incremental "-p:ErrorLog=$(CURDIR)/analysis/back.sarif%2cversion=2.1"
	dotnet build tests/RegistrationDemo.Api.Tests/RegistrationDemo.Api.Tests.csproj --no-incremental --no-dependencies "-p:ErrorLog=$(CURDIR)/analysis/tests.sarif%2cversion=2.1"
	python3 scripts/sarif_to_html.py analysis analysis-report

# Behaviour tests only (unit, integration, e2e); architecture rules run separately.
run-tests:
	@cat scripts/functional-banner.txt
	@status=0; dotnet test --filter "FullyQualifiedName!~Tests.Architecture" --collect:"XPlat Code Coverage" --logger "trx;LogFileName=functional.trx" --results-directory TestResults || status=$$?; \
	python3 scripts/trx_to_html.py TestResults functional-report "Functional tests"; \
	exit $$status
	dotnet tool restore
	dotnet reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:coverage-report -reporttypes:"Html;TextSummary"
	@cat coverage-report/Summary.txt

# Architecture rules. Run last in `verify`, after mutation testing.
architecture-test:
	@cat scripts/architecture-banner.txt
	rm -rf architecture-results
	@status=0; dotnet test --filter "$(ARCH_FILTER)" --logger "trx;LogFileName=architecture.trx" --results-directory architecture-results || status=$$?; \
	python3 scripts/trx_to_html.py architecture-results architecture-report "Architecture tests"; \
	exit $$status

# AI code review: Copilot CLI (read-only) runs its code-review agent and returns a list of findings.
# Never fails the build on findings; a Copilot failure is shown in the report instead.
code-review:
	@cat scripts/code-review-banner.txt
	rm -rf code-review-results
	mkdir -p code-review-results
	@copilot -s -p "`cat scripts/code-review-prompt.txt`" --allow-tool='read' --deny-tool='write' --deny-tool='shell' > code-review-results/raw.txt 2>&1 & pid=$$!; \
	start=`date +%s`; \
	while kill -0 $$pid 2>/dev/null; do \
		printf "\rCopilot is reviewing the code... %ss elapsed" "`expr \`date +%s\` - $$start`"; \
		sleep 2; \
	done; \
	wait $$pid; status=$$?; \
	printf "\rCopilot finished in %ss (exit code %s)%20s\n" "`expr \`date +%s\` - $$start`" "$$status" ""
	python3 scripts/code_review_to_html.py code-review-results/raw.txt code-review-report

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
	@cat scripts/mutation-banner.txt
	dotnet stryker --config-file stryker-config.json

# Static analysis + behaviour tests + mutation testing, then open one landing page linking the three reports.
# Architecture tests are the last phase.
verify: run-analysis run-tests
	dotnet tool restore
	@cat scripts/mutation-banner.txt
	dotnet stryker --config-file stryker-config.json
	@$(MAKE) --no-print-directory architecture-test || touch .architecture-failed
	@$(MAKE) --no-print-directory code-review
	rm -rf reports
	mkdir -p reports/mutation
	cp report-index.html reports/index.html
	cp -R functional-report reports/functional
	cp -R coverage-report reports/coverage
	cp -R analysis-report reports/analysis
	cp -R architecture-report reports/architecture
	cp -R code-review-report reports/code-review
	cp "`ls -td StrykerOutput/*/ | head -1`reports/mutation-report.html" reports/mutation/
	@$(MAKE) --no-print-directory open-report REPORT_DIR=reports
	@if [ -f .architecture-failed ]; then rm -f .architecture-failed; echo "Architecture tests failed (see reports/architecture/index.html)"; exit 1; fi

clean: stop-report
	-dotnet clean --nologo -v q
	rm -rf TestResults functional-report architecture-results architecture-report .architecture-failed code-review-results code-review-report coverage-report StrykerOutput reports analysis analysis-report
	find . -type d \( -name bin -o -name obj \) -prune -exec rm -rf {} +
