.PHONY: run stop logs build contract-test validate factory-scaffold factory-preflight factory-audit

run:
	docker compose up --build -d

stop:
	docker compose down

logs:
	docker compose logs -f interest-app

build:
	dotnet build Checks/Checks.csproj --nologo

contract-test:
	dotnet run --project Checks/Checks.csproj --no-build

validate: build contract-test
	git diff --check

factory-scaffold:
	bash scripts/check-factory-scaffold.sh

factory-preflight:
	bash scripts/preflight-factory.sh

factory-audit:
	bash scripts/audit-factory.sh
