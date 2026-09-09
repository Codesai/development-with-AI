.PHONY: build validate run deployment-config
build:
	docker compose build
deployment-config:
	docker compose config --quiet
validate: build deployment-config
run:
	docker compose up --build -d
