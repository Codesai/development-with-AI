# Workflows - Exercise 7 — GitHub becomes part of the feedback loop

## Learning goal

Make GitHub Actions another feedback channel the agent can inspect and act on within a bounded loop.

## Setup

1. Create an empty GitHub repository.
2. Copy the *contents* of `../app` into the repository root. In particular, `.github/workflows` must be at the repository root so GitHub can discover the workflows.
3. Initialize the repository, commit the starting point, and push the default branch.
4. In **Settings → Actions → General**, allow workflows to read and write pull-request data. Enable Copilot code review for the repository or organization.
5. Create an issue from the `normalize-registration.md` template in GitHub Issues.
6. Confirm that Actions is enabled and that `gh auth status` succeeds locally.

The issue requests storage-safety validation, giving the agent a concrete local change to implement.

## Starting state

The application contains the existing registration model and file repository. The local and CI checks define the formatter contract: fields must be trimmed and serialized as tab-separated values. The checks are intentionally red until the issue is implemented.


## Run

Launch `copilot` from the repository root and paste:

```text
Execute GitHub Issue #<issue-number> using `AGENTS.md`. Implement and validate locally, commit semantically, open a PR, and watch GitHub Actions. Inspect and repair CI failures, then run `make review-loop` to wait for Copilot review comments and repair actionable findings. Use at most three CI/review runs. Never weaken feedback. If the third review still has findings, stop and warn the user. Stop before merge and report the PR URL, corrections, and exact results.
```

, replacing `<issue-number>` with the GitHub Issue number.

Let it open the PR, watch checks, inspect the failing log, and repair the cause. 

Do not relay the error manually.

## Automated Copilot review loop

The `request-copilot-review` workflow runs only after `full-feedback` succeeds. It requests a Copilot review for the exact validated PR commit and refuses to request more than three Copilot reviews on one PR. Copilot code review must be enabled for the repository or organization, and GitHub Actions must have permission to write to pull requests.

After opening and checking out the PR branch, run:

```sh
make review-loop
# Or, when the current branch cannot be resolved automatically:
./scripts/copilot-review-loop.sh <pr-number>
```

The local loop watches required checks, waits for Copilot's review comments, gives actionable comments to the local Copilot CLI, runs `make validate`, commits, and pushes the correction. A push starts validation and remote review again. After three reviews with unresolved findings, it exits with status 2 and warns that manual review is required. It never merges the PR.

## Success and reflection

The agent should reach green CI within three runs without weakening a check, then stop before merge. 

How did remote feedback change the implementation? What should happen if the third run is still red?
