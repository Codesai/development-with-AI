# 05 - Evals (non-deterministic)

## Goal

Grade the same trials as exercise 04, but with a question a script cannot answer: is the comment-free code actually *readable*, not just comment-free? See that the grader itself - an LLM asked to judge - is now also non-deterministic, and has to be measured the same way the agent under test does.

## The feature

Same feature prompt as exercise 04. Keep the no-comments instruction in `app/AGENTS.md` from that exercise. Use this prompt:

> Add a registration confirmation code to this project. Implement the feature end to end: generate the code when a registration is saved, store it in `interests.txt`, return it in the API response, and show it in the frontend confirmation message. The code format is `AAA-YYYYMMDD-NNN-C` (course prefix, UTC date, daily per-course sequence, check character). Make the smallest change that satisfies this - do not refactor or touch unrelated code. Do not build, run, or otherwise validate the app (no build, no server start, no curl, no manual testing) - just make the code change and stop.

## Instructions

1. From `HarnessingAgents/harness`, run:

   ```bash
   make evals:non-deterministic
   ```

   This is the same as `./eval-readability.sh 3` from `harness/evals` (use `make evals:non-deterministic N=5` to vary the count). Note the `k/N judged readable` summary and the list of kept trial directories.

2. Cross-reference with exercise 04. Run this against a couple of the same trials `eval-readability.sh` just judged:

   ```bash
   ../guardrails/check-no-comments.js <trial-dir>
   ```

   Do the two graders ever disagree - a trial that is comment-free but judged `NOT-READABLE` (terse rather than clear), or one the heuristic missed that the judge still calls out?

3. Pick one trial directory and, from `HarnessingAgents/harness`, run:

   ```bash
   make evals:repeat-judge DIR=<trial-dir>
   ```

   This is the same as `./eval-readability.sh --repeat-judge <trial-dir> 3` from `harness/evals` (use `M=` to change the count). The code is identical on every call - only the judge runs again. Does it give the same verdict three times, or does it waver?

4. Read the reasoning the judge gives on a wavering trial. Is it actually engaging with the diff, or does it read like a plausible-sounding sentence that would fit almost any code?

5. Try sharpening the rubric in `judge-readability.sh` (e.g. ask it to point at a specific line it found unclear, or require it to name what a hypothetical comment would have said) and re-run `make evals:repeat-judge DIR=<trial-dir>`. Does a more specific rubric reduce the wobble?

6. Decide what you would actually do with a wavering judge in a real pipeline - more judge samples and a majority vote, a stronger/more capable judge model, a sharper rubric, or accept that this particular quality is not worth automating and belongs in human review instead.

## How it works

### Why a script is not enough here

`check-no-comments.js` can tell you whether a function contains a `//` or `/*`. It cannot tell you whether removing those comments left behind something a stranger could still follow, or just terse code that happens to satisfy a regex. That is a semantic judgment, not a mechanical one - the kind of thing you reach for an LLM-as-judge for, per the same "Running Copilot CLI programmatically" docs as exercise 04, just calling `copilot -p ... -s` a second time with a grading prompt instead of a feature prompt.

The trade-off: a script gives the same verdict on the same input every time. A judge might not. You are trading a narrow, reliable check for a broader, less reliable one - and that unreliability needs its own measurement, not just an assumption that "the AI will know it when it sees it".

### The eval scripts

`HarnessingAgents/harness/evals/`:

- `run-trial.sh` - the same trial runner as exercise 04; both exercises share it so the trials are comparable. Agent output streams live to the terminal as each trial runs, so a slow trial does not sit silent.
- `judge-readability.sh <trial-dir>` - takes one trial, stages and diffs it against its baseline commit, and sends that diff inline in a rubric prompt to a *second*, independent `copilot -p ... -s` call, run from an empty scratch directory (it has no reason to use any tool, so none is given anything to touch). Prints one line: `READABLE` or `NOT-READABLE`, a dash, then the judge's one-sentence reasoning.
- `eval-readability.sh [N]` - runs N trials and judges each one, printing `k/N judged readable`. Unlike exercise 04's eval, trial directories are kept (not deleted) so you can re-judge them.
- `eval-readability.sh --repeat-judge <trial-dir> [M]` - holds one trial's code fixed and re-runs only the judge M times (default 3), to isolate judge-side non-determinism from agent-side non-determinism: the code cannot have changed between runs, so any disagreement is the judge talking to itself. Shorthand: `make evals:repeat-judge DIR=<trial-dir>` (add `M=` to change the count) from `HarnessingAgents/harness`.

Limits, by design: one judge call, one model, one rubric wording - all things known to sway an LLM-as-judge's verdict. `READABLE`/`NOT-READABLE` is a coarse binary for what is really a spectrum. And the judge is graded on trust: nothing here checks that its stated reasoning actually matches the diff it was given.

## Recommendations

Trial directories are kept under `harness/evals/results/` by this exercise, gitignored so they never end up in a commit - clean them up yourself once you are done. From `harness/evals`:

```bash
rm -rf results/trial-*
```

If every verdict comes back `NOT-READABLE - no changes were made`, the trial's diff was empty - check `agent.log` in that trial's directory before suspecting the judge.
