# 04 - Evals (deterministic)

## Goal

Run a feature prompt against Copilot CLI many times, non-interactively, and grade every run with a script. See that compliance with an instruction is not something you verify once - the same prompt, the same `AGENTS.md`, the same model can pass on one run and fail on the next.

An eval runs outside any live session entirely: many independent attempts at the same prompt, each graded after the fact, with nothing watching or correcting while the agent works. It answers "how often does this hold", not "did this one run comply".

## The feature

Same feature prompt in every trial:

> Add a registration confirmation code to this project. Implement the feature end to end: generate the code when a registration is saved, store it in `interests.txt`, return it in the API response, and show it in the frontend confirmation message. The code format is `AAA-YYYYMMDD-NNN-C` (course prefix, UTC date, daily per-course sequence, check character). Make the smallest change that satisfies this - do not refactor or touch unrelated code. Do not build, run, or otherwise validate the app (no build, no server start, no curl, no manual testing) - just make the code change and stop.

## Instructions

1. Add a no-comments instruction to `app/AGENTS.md`, if it is not already there: the agent should rely on descriptive names and small functions, and should strip comments from any code it has to edit that already has them.

2. From `HarnessingAgents/harness/evals`, run `./eval-no-comments.sh 3`. Each trial launches its own `copilot` run against a private copy of `app/` - your real `app/` and its git state are untouched.

3. Read the summary. Is it `10/10`? Most instructions given to an agent are not. If any trial failed, open its kept directory: `grade.log` shows exactly which lines were flagged and in which file, `agent.log` shows the full run.

4. Re-run a few times, and vary N. A single run tells you almost nothing about how reliably an instruction is followed; five or ten runs start to.

5. Try weakening or strengthening the wording in `AGENTS.md` (e.g. drop the "when editing code that already has comments" clause) and re-run. Does the pass rate move the way you would expect?

6. Notice what a single run could never have told you. "It happened this one time" or "it failed this one time" is not a number. Running many trials turns "the agent should behave like X" into something you can track over time, e.g. after every prompt or model change.

7. Optional: measure whether a corrective mechanism changes the number. Install the post-edit script that nudges the agent to remove comments after it edits (`../install-no-comments-hook.sh`, user-level under `~/.copilot/hooks/`, so it applies automatically - no change to `run-trial.sh` needed), then re-run `./eval-no-comments.sh 3`. Compare the pass rate against step 2's baseline. Uninstall it again when done (`../install-no-comments-hook.sh uninstall`) since it stays active for every `copilot` session on this machine, not just this eval, until removed.

## How it works

### Non-interactive Copilot CLI

GitHub Copilot CLI has no dedicated eval feature - no `copilot eval` subcommand, no eval file format. What it does document is running the CLI non-interactively, which is all an eval needs:

- `copilot -p "<prompt>"` - execute a prompt and exit when done, instead of opening a session.
- `-s` - suppress the session banner/stats, so stdout is just the agent's final response. Useful for capturing output in a script; irrelevant to how this exercise grades (it grades the file changes, not what the agent says).
- `--allow-all-tools` - skip approval prompts, since there is no human in the loop to answer them.
- `--share=PATH` - export the session transcript, if you want to read what the agent actually did on a given trial.

Docs:

- Running Copilot CLI programmatically: https://docs.github.com/en/copilot/how-tos/copilot-cli/automate-copilot-cli/run-cli-programmatically
- Copilot CLI programmatic reference: https://docs.github.com/en/copilot/reference/copilot-cli-reference/cli-programmatic-reference

### The eval scripts

`HarnessingAgents/harness/evals/`:

- `run-trial.sh` - one trial. Copies `app/` into a fresh throwaway git repo (so a grader that reads `git diff` works unmodified), runs the feature prompt there with `copilot -p ... -s --allow-all-tools`, and prints the trial directory's path. It does not delete it - the caller grades it first.
- `eval-no-comments.sh [N]` - runs `run-trial.sh` N times (default 3) and grades each trial by pointing `check-no-comments.sh` directly at the resulting diff - no live session, just the finished result. Prints `k/N passed`, deletes passing trials, and keeps failing ones on disk for inspection. Each trial's agent output streams live to the terminal as it runs (and into `agent.log`), so a slow trial does not sit silent.

A quirk worth knowing, because it is the kind of thing that quietly breaks an eval: `check-no-comments.sh` behaves differently depending on whether its stdout is a terminal. Run interactively it exits 2 on a failure; piped, as `eval-no-comments.sh` runs it, it always exits 0 - on a pass it prints nothing, on a fail it prints a JSON `additionalContext` blob (the shape a different calling context expects). So the grader here checks stdout *content*, not the exit code. A script written for one calling context does not necessarily port to another for free.

Limits, by design: the grader is a brace-counting heuristic, with real blind spots (expression-bodied members, top-level statements, comments inside strings). And this only tells you whether comments are present or absent - not whether the comment-free code is actually any good. That is exercise 05.

## Recommendations

Trial directories live under `harness/evals/results/`, gitignored so they never end up in a commit. Passing trials are deleted automatically; clean up any leftover failing ones yourself (`rm -rf results/trial-*` from `harness/evals`) once you are done reading them.

If every trial fails immediately with no diff at all, check `agent.log` first - a Copilot CLI error (auth, rate limit, a bad flag) looks identical to a trial where the agent genuinely did nothing.
