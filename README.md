## Exercise Sets: Introduction to AI-Assisted Development Training

### [Prompting](Prompting/README.md)
A series of prompting techniques and exercises covering naive prompting through to crystallizing reusable prompts. Learn how to write better prompts with role-based instructions, context, constraints, and structured formats.

### [ContextManagement](ContextManagement/README.md)
Hands-on exercises for managing context when working with AI agents. Learn how to structure prompts to improve responses and how to reuse them and eventually crystallise them into commands.

### [EvaluatingResults](EvaluatingResults/README.md)
Exercises for practicing evaluation of AI-generated code, tests, and architectural changes. Develop skills in code review, mutation testing, and architectural fitness validation.

## Prerequisites

These exercises use a .NET 9 codebase and GitHub Copilot as the AI assistant. Set up the following before starting:

### 1. .NET 9 SDK
Install the .NET 9 SDK (some exercises pin SDK version `9.0.308` via `global.json`):
- Download: https://dotnet.microsoft.com/download/dotnet/9.0
- Verify with `dotnet --version`

### 2. Visual Studio Code
- Download: https://code.visualstudio.com/
- Recent versions of VS Code ship with **GitHub Copilot** and **GitHub Copilot Chat** built in, so on a fresh install there's nothing extra to add — just sign in (Accounts icon in the bottom-left, or `Ctrl+Shift+P` → "GitHub Copilot: Sign In") when prompted.
- If you have an **older VS Code install** where Copilot isn't present or is outdated:
  - Check your version via `Help` → `About` (or `Ctrl+Shift+P` → "About"), and update VS Code itself first: `Ctrl+Shift+P` → "Update" (or download the latest installer from the link above).
  - Then install/update the extensions from the Extensions view (`Ctrl+Shift+X` / `Cmd+Shift+X`) — search for "GitHub Copilot" and "GitHub Copilot Chat" and click Install (or Update if already present), or via CLI:
    ```
    code --install-extension GitHub.copilot --force
    code --install-extension GitHub.copilot-chat --force
    ```
- Use the Copilot Chat panel (or inline `Ctrl+I` / `Cmd+I`) as the integrated agent/assistant for the exercises.

### 3. GitHub Copilot CLI
Install the CLI (requires Node.js 22+):
```
npm install -g @github/copilot
```
- Start an interactive session with `copilot` in your terminal, then authenticate with `/login` (or run `copilot` and follow the sign-in prompt) — this opens a browser to link your GitHub account.
- Check the docs for the latest install instructions: https://docs.github.com/en/copilot/how-tos/set-up/install-copilot-cli

### 4. GitHub account with Copilot access
- If you don't have a GitHub account yet, create one for free: https://github.com/signup
- GitHub Copilot requires an active subscription, but individuals can start a free trial or use the free tier (limited monthly usage) — see https://github.com/features/copilot/plans
- Once enabled, sign in from both VS Code and the Copilot CLI as described above to use the same account across both tools.
