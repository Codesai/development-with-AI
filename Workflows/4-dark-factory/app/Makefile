.PHONY: run stop logs build test validate factory-preflight factory-audit

run:
	docker compose up --build -d

stop:
	docker compose down

logs:
	docker compose logs -f interest-app

build:
	dotnet build back/InterestApi.csproj --nologo

test:
	dotnet test tests/InterestApi.Tests.csproj --nologo

validate: build test
	git diff --check

factory-preflight:
	bash scripts/preflight-factory.sh

factory-audit:
	bash scripts/audit-factory.sh
