.PHONY: hooks\:install hooks\:uninstall evals\:deterministic evals\:non-deterministic evals\:repeat-judge gateway\:copilot

hooks\:install:
	./install-no-comments-hook.js install

hooks\:uninstall:
	./install-no-comments-hook.js uninstall

evals\:deterministic:
	cd evals && ./eval-no-comments.js $(N)

evals\:non-deterministic:
	cd evals && ./eval-readability.js $(N)

evals\:repeat-judge:
	cd evals && ./eval-readability.js --repeat-judge $(DIR) $(M)

gateway\:copilot:
	cd ../app && ../harness/gateway/copilot-gated.js
