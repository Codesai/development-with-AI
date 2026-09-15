# 04 - Evals (deterministic)

## Goal

Run the exercise 01/02 feature prompt against Copilot CLI many times, non-interactively, and grade every run with a script. See that a guideline is not something you verify once - the same prompt, the same `AGENTS.md`, the same model can pass on one run and fail on the next.

An eval is neither a guideline nor a guardrail. A guideline only asks, before the agent decides. A guardrail reacts inside one live session, after one change, and can nudge the agent to fix itself. An eval runs outside any session entirely: many independent attempts at the same prompt, each graded after the fact, with nothing watching or correcting while the agent works. It answers a different question than the other two - not "did this run comply" but "how often does it".

## Non-interactive Copilot CLI

GitHub Copilot CLI has no dedicated eval feature - no `copilot eval` subcommand, no eval file format. What it does document is running the CLI non-interactively, which is all an eval needs:

- `copilot -p "<prompt>"` - execute a prompt and exit when done, instead of opening a session.
- `-s` - suppress the session banner/stats, so stdout is just the agent's final response. Useful for capturing output in a script; irrelevant to how this exercise grades (it grades the file changes, not what the agent says).
- `--allow-all-tools` - skip approval prompts, since there is no human in the loop to answer them.
- `--share=PATH` - export the session transcript, if you want to read what the agent actually did on a given trial.

Docs:

- Running Copilot CLI programmatically: https://docs.github.com/en/copilot/how-tos/copilot-cli/automate-copilot-cli/run-cli-programmatically
- Copilot CLI programmatic reference: https://docs.github.com/en/copilot/reference/copilot-cli-reference/cli-programmatic-reference

## The eval

`HarnessingAgents/harness/evals/`:

- `run-trial.sh` - one trial. Copies `app/` into a fresh throwaway git repo (so a grader that reads `git diff` works unmodified), runs the exercise 01/02 prompt there with `copilot -p ... -s --allow-all-tools`, and prints the trial directory's path. It does not delete it - the caller grades it first.
- `eval-no-comments.sh [N]` - runs `run-trial.sh` N times (default 5) and grades each trial by pointing exercise 02's guardrail script, `check-no-comments.sh`, directly at the resulting diff - no hook, no live session, just the finished result. Prints `k/N passed`, deletes passing trials, and keeps failing ones on disk for inspection.

A quirk worth knowing, because it is the kind of thing that quietly breaks an eval: `check-no-comments.sh` behaves differently depending on whether its stdout is a terminal. Run interactively (as a hook would see it) it exits 2 on a failure; piped, as `eval-no-comments.sh` runs it, it always exits 0 - on a pass it prints nothing, on a fail it prints a JSON `additionalContext` blob (the shape a `postToolUse` hook expects). So the grader here checks stdout *content*, not the exit code. A script written for one calling context does not necessarily port to another for free.

Limits, by design: the grader is the same brace-counting heuristic from exercise 02, with the same blind spots (expression-bodied members, top-level statements, comments inside strings). And this only tells you whether comments are present or absent - not whether the comment-free code is actually any good. That is exercise 05.

## The feature

Same task and prompt as exercises 01 and 02:

> Add a registration confirmation code to this project. Implement the feature end to end: generate the code when a registration is saved, store it in `interests.txt`, return it in the API response, and show it in the frontend confirmation message. The code format is `AAA-YYYYMMDD-NNN-C` (course prefix, UTC date, daily per-course sequence, check character).

## Instructions

1. Add the no-comments guideline to `app/AGENTS.md` (from exercise 01), if it is not already there: the agent should rely on descriptive names and small functions, and should strip comments from any code it has to edit that already has them.

2. From `HarnessingAgents/harness/evals`, run `./eval-no-comments.sh 5`. Each trial launches its own `copilot` run against a private copy of `app/` - your real `app/` and its git state are untouched.

3. Read the summary. Is it `5/5`? Most guidelines are not. If any trial failed, open its kept directory: `grade.log` shows exactly which lines were flagged and in which file, `agent.log` shows the full run.

4. Re-run a few times, and vary N. A single run tells you almost nothing about a guideline's reliability; five or ten runs start to.

5. Try weakening or strengthening the wording in `AGENTS.md` (e.g. drop the "when editing code that already has comments" clause) and re-run. Does the pass rate move the way you would expect?

6. Compare with 01/02. Neither of those exercises could tell you a number - only "it happened this one time" or "the hook caught it this one time". This is the piece that turns "the agent should behave like X" into something you can track over time, e.g. after every prompt or model change.

## Recommendations

Trial directories live under `${TMPDIR:-/tmp}/harnessingagents-eval-*`. Passing trials are deleted automatically; clean up any leftover failing ones yourself (`rm -rf /tmp/harnessingagents-eval-*`) once you are done reading them.

If every trial fails immediately with no diff at all, check `agent.log` first - a Copilot CLI error (auth, rate limit, a bad flag) looks identical to a trial where the agent genuinely did nothing.
