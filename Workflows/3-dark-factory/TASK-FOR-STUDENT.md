# Five-minute challenge — Make the coordinator's next decision

After the README run has completed or stopped, act as coordinator for these hypothetical situations. Use the stop and integration rules in `AGENTS.md`.

1. Validation is green, but an IMPORTANT review finding remains after the second fix round. The implementer suggests merging and fixing it in the next issue.
2. Validation and review passed on the feature branch. Rebase then changed the feature's base commit, and the implementer proposes reusing the earlier validation result.
3. The next issue says “reject duplicate registrations” without defining what counts as a duplicate. The implementer offers to choose a policy automatically.

For each situation, choose **continue**, **revalidate**, or **stop**. Name the exact next action, whether merging is allowed, and the evidence or decision required before progress is possible.

Then draft a three-line stop-log entry for one situation: reason, evidence, and preserved branch/next required decision. Use clearly marked hypothetical references rather than inventing successful command results.

Deliver your decisions and log entry only. Do not restart the factory, modify its immutable queue, or add an eleventh issue. The challenge is to apply the controls when a shortcut looks tempting.
