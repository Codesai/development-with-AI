.PHONY: build validate run deployment-config validate ci-validate review-loop

docker-build:
	docker compose build

deployment-config: docker-build
	docker compose config --quiet

run:
	docker compose up --build -d

build:
	dotnet build LocalChecks/LocalChecks.csproj --nologo

validate: deployment-config build
	dotnet run --project LocalChecks/LocalChecks.csproj --no-build

ci-validate: build
	dotnet run --project CiChecks/CiChecks.csproj

review-loop:
	./scripts/copilot-review-loop.sh
