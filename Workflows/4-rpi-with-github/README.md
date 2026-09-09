# Workflows - Exercise 7 — GitHub becomes part of the feedback loop

A small API in .NET 10, that receives interest records and saves them to `interests.txt`.

## Requirements

- Use GitHub Codespaces

OR 

- Docker and Docker Compose

## Run the application

From this directory, start the service with:

```bash
make run
```

The application is available at <http://localhost:8080>.

View records on the host in back/interests.txt (it updates as submissions arrive)

## Learning goal

Make GitHub Actions another feedback channel the agent can inspect and act on within a bounded loop.


## Setup

Copy this folder to the root of a new repository, initialize and push it, then create the `normalize-registration.md` issue. Confirm Actions is enabled and `gh auth status` succeeds. The issue requests storage-safety validation, giving the agent a concrete local change to implement.

## Starting state

`make validate` is green for the starter formatter: surrounding whitespace is trimmed and tab-separated storage is preserved. After the issue is implemented locally, CI also enforces an organization-wide canonical-email policy absent from local checks. `make ci-validate` exposes that policy and is intentionally red, but learners should encounter it through the PR check.

## Run

Launch `copilot` with `prompts/ci-loop.md`, replacing `<issue-number>`. Let it open the PR, watch checks, inspect the failing log, and repair the cause. Do not relay the error manually.

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

The agent should reach green CI within three runs without weakening a check, then stop before merge. How did remote feedback change the implementation? What should happen if the third run is still red?
