.PHONY: build run rollback
build:
	docker compose build
run:
	docker compose up --build -d
rollback:
	git reset --hard
	git clean -fd
