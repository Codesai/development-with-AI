#!/usr/bin/env bash
set -euo pipefail

readonly MAX_RUNS=3
readonly POLL_SECONDS="${REVIEW_POLL_SECONDS:-20}"
readonly MAX_POLLS="${REVIEW_MAX_POLLS:-45}"

fail() {
  printf 'ERROR: %s\n' "$*" >&2
  exit 1
}

command -v gh >/dev/null || fail "gh is required"
command -v copilot >/dev/null || fail "GitHub Copilot CLI is required"
command -v jq >/dev/null || fail "jq is required"
gh auth status >/dev/null || fail "run 'gh auth login' first"

pr_number="${1:-}"
if [[ -z "$pr_number" ]]; then
  pr_number=$(gh pr view --json number --jq .number) || fail "pass a PR number or run from its branch"
fi
[[ "$pr_number" =~ ^[0-9]+$ ]] || fail "PR number must be numeric"

repo=$(gh repo view --json nameWithOwner --jq .nameWithOwner)
[[ -z "$(git status --porcelain -- .)" ]] || \
  fail "the exercise working tree must be clean before starting the review loop"

for ((run = 1; run <= MAX_RUNS; run++)); do
  printf '\nReview cycle %d/%d for %s#%s\n' "$run" "$MAX_RUNS" "$repo" "$pr_number"

  gh pr checks "$pr_number" --watch --fail-fast || \
    fail "validation failed in cycle $run; inspect it with: gh run view --log-failed"

  head_sha=$(gh pr view "$pr_number" --json headRefOid --jq .headRefOid)
  review_json=''
  for ((poll = 1; poll <= MAX_POLLS; poll++)); do
    review_json=$(gh api --paginate "repos/$repo/pulls/$pr_number/reviews" | jq -sc \
      --arg sha "$head_sha" \
      '[.[][] | select(.commit_id == $sha and (.user.login | test("copilot"; "i")))] | sort_by(.submitted_at) | last // empty')
    [[ -n "$review_json" ]] && break
    printf 'Waiting for Copilot review of %.12s (%d/%d)...\n' "$head_sha" "$poll" "$MAX_POLLS"
    sleep "$POLL_SECONDS"
  done
  [[ -n "$review_json" ]] || fail "no Copilot review arrived for $head_sha before the timeout"

  review_id=$(jq -r .id <<<"$review_json")
  comments=$(gh api --paginate "repos/$repo/pulls/$pr_number/reviews/$review_id/comments" | jq -sc '[.[][]]')
  comment_count=$(jq 'length' <<<"$comments")
  if (( comment_count == 0 )); then
    printf 'Copilot review passed on cycle %d: no inline findings for %s.\n' "$run" "$head_sha"
    exit 0
  fi

  if (( run == MAX_RUNS )); then
    printf 'STOP: Copilot still reported %d finding(s) after %d cycles. Manual review is required.\n' \
      "$comment_count" "$MAX_RUNS" >&2
    exit 2
  fi

  feedback_file=$(mktemp)
  trap 'rm -f "$feedback_file"' EXIT
  jq '[.[] | {path, line, start_line, body, html_url}]' <<<"$comments" >"$feedback_file"

  prompt="Address the actionable GitHub Copilot review comments in $feedback_file for PR #$pr_number.
Read AGENTS.md and README.md first. Fix the engineering causes and add or update tests when appropriate.
Do not edit anything under .github/workflows, do not weaken or skip checks, and do not commit or push.
Run make validate before finishing. If a comment is not actionable or is incorrect, leave the code unchanged and explain why."
  copilot -p "$prompt" --allow-all-tools --no-ask-user
  rm -f "$feedback_file"
  trap - EXIT

  git diff --quiet -- .github/workflows || \
    fail "the agent changed a workflow file; refusing to continue"
  make validate
  git diff --quiet && git diff --cached --quiet && \
    fail "the agent made no change for $comment_count finding(s); manual review is required"
  git add -- . ':!.github/workflows'
  git diff --cached --quiet && fail "only workflow files changed; refusing to weaken CI"
  git commit -m "fix: address Copilot review feedback"
  git push
done

fail "review loop exhausted unexpectedly"
