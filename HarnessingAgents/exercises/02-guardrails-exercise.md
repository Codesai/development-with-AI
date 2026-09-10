# 02 - Guardrails

## Goal

Wire a check into a GitHub Copilot CLI `postToolUse` hook and watch it correct the agent while it implements a feature. See how a guardrail (feedback, after the change) differs from a guideline (feedforward, before the change).

## Keep the guardrail out of the agent's view

This exercise only works if the agent does not know what you are checking. If it can read the guardrail script or an exercise brief that says "no comments", it will write comment-free code from the first turn and you never see the loop.

So nothing about the guardrail lives in `HarnessingAgents/app/`, the folder the agent runs in:

- the exercise instructions are in `HarnessingAgents/exercises/`;
- the script is `HarnessingAgents/harness/guardrails/check-no-comments.sh`;
- the hook is installed at the user level (`~/.copilot/hooks/`), not in the project.

Launch `copilot` from `HarnessingAgents/app/` and keep it there. Treat "what must the student know that the agent must not?" as part of designing any guardrail.

## Lifecycle hooks

Hooks are scripts Copilot CLI runs at fixed points in a session, no matter what the model reasons about. The events include `sessionStart`, `userPromptSubmitted`, `preToolUse`, `postToolUse`, `agentStop`, and more. Hook config is loaded from `.github/hooks/*.json` in the directory where you launch `copilot`, and from `~/.copilot/hooks/` for the current user.

- `preToolUse` runs *before* a tool and can allow, deny, or modify the call.
- `postToolUse` runs *after* the tool succeeded. It cannot undo the edit. It can return an `additionalContext` string that the agent sees on its next turn and can act on.

A comment check therefore has to be a `postToolUse` hook: the file is already written, so the guardrail reacts rather than blocks. This is the self-correcting loop.

Docs:

- Using hooks: https://docs.github.com/en/copilot/how-tos/copilot-cli/customize-copilot/use-hooks
- Hooks reference (events, payloads, output contract): https://docs.github.com/en/copilot/reference/hooks-reference
- Copilot CLI general availability: https://github.blog/changelog/2026-02-25-github-copilot-cli-is-now-generally-available/

## The guardrail

`HarnessingAgents/harness/guardrails/check-no-comments.sh` ships ready to run. It works on whatever git working tree `copilot` is running in. After each edit it:

1. finds the lines that changed since `HEAD` (`git diff` over `.cs .js .ts`),
2. for each changed line, takes the function or method that encloses it, and flags any comment (`//`, `/* */`, `///`, a `*`-prefixed line) anywhere in that function,
3. if any are found, returns `{"additionalContext": "..."}` listing them and asking the agent to remove them; otherwise it exits silently.

The rule is "a function you touch must contain no comments" — a pre-existing comment in a method the agent modified counts; a comment in an untouched method in the same file does not.

Limits, by design: enclosing-function detection is brace-based, not a parser, so expression-bodied members and top-level-statement files can be missed, and a `//` inside a string literal is a false positive. It only nudges — nothing forces the agent to re-run it or to obey.

## The feature

Same task as exercise 01. Use this prompt for every run:

> Add a registration confirmation code to this project. Implement the feature end to end: generate the code when a registration is saved, store it in `interests.txt`, return it in the API response, and show it in the frontend confirmation message. The code format is `AAA-YYYYMMDD-NNN-C` (course prefix, UTC date, daily per-course sequence, check character).

## Instructions

1. Baseline. Make sure `app/AGENTS.md` is the committed starter version (revert any no-comments rule you added in exercise 01). No hook yet. Start `copilot` in `HarnessingAgents/app`, give it the prompt, and note the comments in the diff. Revert the feature changes.

2. Run the guardrail by hand. From `HarnessingAgents/app`, add a throwaway `// note` to one of the `back/*.cs` files, run `../harness/guardrails/check-no-comments.sh`, and see it report the line. Remove the comment and run it again to see it pass. Revert.

3. Install the hook for your user, so it never enters the project the agent reads. From `HarnessingAgents/app`:

   ```bash
   mkdir -p ~/.copilot/hooks
   cp ../harness/guardrails/check-no-comments.sh ~/.copilot/hooks/
   chmod +x ~/.copilot/hooks/check-no-comments.sh
   ```

   Create `~/.copilot/hooks/no-comments.json`:

   ```json
   {
     "version": 1,
     "hooks": {
       "postToolUse": [
         {
           "type": "command",
           "matcher": "edit|create|apply_patch",
           "bash": "~/.copilot/hooks/check-no-comments.sh",
           "timeoutSec": 30
         }
       ]
     }
   }
   ```

4. Start a fresh `copilot` session in `HarnessingAgents/app` (hooks load at session start) and give it the same prompt.

5. Watch the run. After each `edit`, `create`, or `apply_patch` the hook fires. When the agent writes a comment, the check feeds it back and the agent should remove it on a following turn.

6. Check the feature works. Run `make run`, submit the form, and confirm a code in the `AAA-YYYYMMDD-NNN-C` shape comes back in the API response, lands as a new column in `back/interests.txt`, and shows in the page message. Only judge the code style once the behaviour is right.

7. Compare with the baseline diff. Are the new comments gone? How many extra turns did it cost? Because the check covers the whole enclosing function, editing a starter method pulls its existing `///` docs and `//` comments into scope too — does the agent strip them, push back, or ignore the feedback and move on?

## Recommendations

If the hook never fires: check that you launched `copilot` from `HarnessingAgents/app`, that `~/.copilot/hooks/no-comments.json` is valid JSON, that the script is executable (`chmod +x`), and that the `matcher` covers the tool the agent actually used - Copilot writes files with `edit`, `create`, or `apply_patch`.

A `postToolUse` hook cannot block a change. Hard-stopping the edit instead of nudging afterwards needs a `preToolUse` hook.

Revert freely with version control between runs.
