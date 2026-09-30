.PHONY: build run
build:
	docker compose build
run:
	docker compose up --build -d
rollback:
	git reset --hard
	git clean -fd