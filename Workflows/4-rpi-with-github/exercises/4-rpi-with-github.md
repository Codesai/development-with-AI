# Workflows - Exercise 7 — GitHub becomes part of the feedback loop

## Learning goal

Make GitHub Actions another feedback channel the agent can inspect and act on within a bounded loop.

## Setup

- Copy the `../app` directory to the root of a new repository, 
- initialize and push it to a new Github repository, {{CONTINUE WITH A LIST OF ACTIONS}} then create the `normalize-registration.md` issue in Github Issues. Confirm Actions is enabled and `gh auth status` succeeds. The issue requests storage-safety validation, giving the agent a concrete local change to implement.

## Starting state

`make validate` is green for the starter formatter: surrounding whitespace is trimmed and tab-separated storage is preserved. 

## Goal

After the issue is implemented locally, CI also enforces an organization-wide canonical-email policy absent from local checks. `make ci-validate` exposes that policy and is intentionally red, but students should encounter it through the PR check.

## Run

Launch `copilot` and paste:

```text
Execute GitHub Issue #<issue-number> using `AGENTS.md`. Implement and validate locally, commit semantically, open a PR, and watch GitHub Actions. Inspect and repair CI failures, then run `make review-loop` to wait for Copilot review comments and repair actionable findings. Use at most three CI/review runs. Never weaken feedback. If the third review still has findings, stop and warn the user. Stop before merge and report the PR URL, corrections, and exact results.
```

, replacing `<issue-number>` with the id in Github Issues. 

Let it open the PR, watch checks, inspect the failing log, and repair the cause. Do not relay the error manually.

## Automated Copilot review loop

The `request-copilot-review` workflow runs only after `full-feedback` succeeds. It requests a Copilot review for the exact validated PR commit and refuses to request more than three Copilot reviews on one PR. Copilot code review must be enabled for the repository or organization, and GitHub Actions must be permitted to write to pull requests.

After opening and checking out the PR branch, run:

```sh
make review-loop
# Or, when the current branch cannot be resolved automatically:
./scripts/copilot-review-loop.sh <pr-number>
```

The local loop watches required checks, waits for Copilot's review comments, gives actionable comments to the local Copilot CLI, runs `make validate`, commits, and pushes the correction. A push starts validation and remote review again. After three reviews with unresolved findings it exits with status 2 and warns that manual review is required. It never merges the PR.

## Success and reflection

The agent should reach green CI within three runs without weakening a check, then stop before merge. 

How did remote feedback change the implementation? What should happen if the third run is still red?
