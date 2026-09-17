.PHONY: hooks\:install hooks\:uninstall evals\:deterministic evals\:non-deterministic evals\:repeat-judge gateway\:copilot

hooks\:install:
	./install-no-comments-hook.sh install

hooks\:uninstall:
	./install-no-comments-hook.sh uninstall

evals\:deterministic:
	cd evals && ./eval-no-comments.sh $(N)

evals\:non-deterministic:
	cd evals && ./eval-readability.sh $(N)

evals\:repeat-judge:
	cd evals && ./eval-readability.sh --repeat-judge $(DIR) $(M)

gateway\:copilot:
	cd ../app && ../harness/gateway/copilot-gated.sh
