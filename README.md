## Exercise Sets: Introduction to AI-Assisted Development Training

### [Prompting](Prompting/README.md)
A series of prompting techniques and exercises covering naive prompting through to crystallizing reusable prompts. Learn how to write better prompts with role-based instructions, context, constraints, and structured formats.

### [ContextManagement](ContextManagement/README.md)
Hands-on exercises for managing context when working with AI agents. Learn how to structure prompts to improve responses and how to reuse them and eventually crystallise them into commands.

### [EvaluatingResults](EvaluatingResults/README.md)
Exercises for practicing evaluation of AI-generated code, tests, and architectural changes. Develop skills in code review, mutation testing, and architectural fitness validation.

## Tools required by exercise set

| Exercise set | Required tools |
| --- | --- |
| Prompting | GitHub Copilot Chat or GitHub Copilot CLI |
| Context Management | GitHub Copilot CLI and Docker with Docker Compose |
| Evaluating Results | .NET 10 SDK and GitHub Copilot; the mutation-testing exercise also requires Stryker.NET |

## Prerequisites

These exercises use a .NET 10 codebase and GitHub Copilot as the AI assistant.

### Choosing your setup

| Setup | Choose it when... | Avoid it when... |
|---|---|---|
| **Codespaces** *(default option)* | You want zero local installs — just a browser and a GitHub account. Best for locked-down corporate laptops, quick trials, or Chromebook/tablet use. | You're already comfortable with Docker/Podman locally or need to work fully offline. |
| **Local environment** | You have (or can get) admin rights to install tools, want the fastest day-to-day performance, and will reuse the setup beyond this training. | Your machine is locked down (no admin rights, no ability to install .NET/Node/Copilot), or you don't want the exercises' tooling on your main machine. |
| **Docker** | You want a container on your own machine (offline-capable, no usage quota), and Docker Desktop is allowed on your machine/network. | Your company blocks Docker Desktop (licensing) or its daemon — use Podman instead. |
| **Podman** | Same use case as Docker, but your company disallows Docker Desktop specifically (e.g. licensing terms) or you prefer a daemonless tool. | You just want the path of least resistance and Docker is already allowed — Docker has more first-party tooling/documentation. |

If you're unsure, start with **Codespaces** — it needs no local setup at all and works from a browser. Switch to a local environment or Docker/Podman if you outgrow the free quota or need offline access.

## Codespaces (recommended default)

[Codespaces](https://github.com/features/codespaces) runs a Dev Container in the cloud, so there's nothing to install locally at all — not even Docker/Podman. This is the recommended default: fastest to start, no local footprint. Codespaces plans have limited quotas; check the [current GitHub billing documentation](https://docs.github.com/en/billing/managing-billing-for-github-copilot/about-billing-for-github-copilot) for details.
- From the repo on GitHub: `Code` → `Codespaces` → `Create codespace on main` (this picks up the `.devcontainer/devcontainer.json` from the Docker section below if present, or falls back to a default image).
- Codespaces opens either in the **browser** (a full VS Code UI served from the cloud instance) or you can attach your **desktop VS Code** to it via the "Dev Containers"/"GitHub Codespaces" extension (`Ctrl+Shift+P` → "Codespaces: Connect to Codespace") — Codespaces is built on the same Dev Containers spec.
- **Copilot in VS Code**: works exactly as on your local machine — sign in with the same GitHub account (Codespaces inherits your GitHub identity automatically since the codespace *is* tied to your account), and the Copilot/Copilot Chat extensions are typically pre-installed or auto-enabled; if not, install them from the Extensions view same as usual.
- **Copilot CLI**: Should be already present in the image, if not install/run it exactly as in the Docker section's Option B below (`npm install -g @github/copilot`, then `copilot` + `/login`). Since you're already authenticated to GitHub in that environment, login is often just a confirmation click rather than a full device-code flow.
- **Stryker.NET**: install it in the Codespaces terminal before starting the mutation-testing exercises:
  ```bash
  dotnet tool install --global dotnet-stryker
  ```
  If it is already installed, use `dotnet tool update --global dotnet-stryker`. Verify it with `dotnet stryker --version`.
- **Copilot CLI version**: the default Codespaces image can ship an outdated Copilot CLI. If model selection fails or looks wrong, update it:
  1. Run `npm install -g @github/copilot` to pull the latest version.
  2. Open a **new terminal** (the currently running `copilot` session won't pick up the upgrade) and run `copilot`.
  3. Confirm that the new session reports the updated version. If it does not, close all terminals and Copilot sessions, then retry.
- Everything is ephemeral per-codespace but persists across stop/start (not across deletion), so you don't need to redo setup every session — only if you delete the codespace.
- **Not a fit if**: you'll exceed the free quota regularly, or you need to work offline.

## Local environment

See the [decision table](#choosing-your-setup) above for which setup fits your situation. If you'd rather not install tools locally, skip to Docker or Podman below.

Install these on your machine:

### 1. Git
Required to clone the repo and work with version control.
- **Windows**: install [Git for Windows](https://git-scm.com/download/win) (includes Git Bash), or via winget: `winget install --id Git.Git -e`
- **macOS**: preinstalled with Xcode Command Line Tools (`git --version` will prompt you to install them if missing), or via Homebrew: `brew install git`
- **Linux**: via your package manager, e.g. `sudo apt install git` (Debian/Ubuntu), `sudo dnf install git` (Fedora), `sudo pacman -S git` (Arch)
- Verify with `git --version`

### 2. .NET 10 SDK
Install the .NET 10 SDK (some exercises pin SDK version `10.0.200` via `global.json`):
- Download: https://dotnet.microsoft.com/download/dotnet/10.0
- Verify with `dotnet --version`

### 3. Visual Studio Code
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

### 4. GitHub Copilot CLI
Install the CLI (requires Node.js 22+):
```
npm install -g @github/copilot
```
- Start an interactive session with `copilot` in your terminal, then authenticate with `/login` (or run `copilot` and follow the sign-in prompt) — this opens a browser to link your GitHub account.
- Check the docs for the latest install instructions: https://docs.github.com/en/copilot/how-tos/set-up/install-copilot-cli

### 5. GitHub account with Copilot access
- If you don't have a GitHub account yet, create one for free: https://github.com/signup
- GitHub Copilot requires an active subscription, but individuals can start a free trial or use the free tier (limited monthly usage) — see https://github.com/features/copilot/plans
- Once enabled, sign in from both VS Code and the Copilot CLI as described above to use the same account across both tools.

### 6. Stryker.NET (mutation-testing exercises)
The mutation-testing exercises use the Stryker.NET command-line tool. In a local environment, install it globally after installing the .NET SDK. For Codespaces, run the same command in the Codespaces terminal.

```bash
dotnet tool install --global dotnet-stryker
```

If it is already installed, update it with `dotnet tool update --global dotnet-stryker`. Verify the installation with:

```bash
dotnet stryker --version
```

See [the mutation-testing exercise](EvaluatingResults/02-Mutation/02-mutation-testing-thinking.md) for how to run it.

## Docker-based setup

Choose this over Codespaces if you want a container on your own machine — offline-capable, no usage quota — and Docker Desktop is allowed in your environment. You need Docker Desktop (or Docker Engine) installed on the host: https://www.docker.com/products/docker-desktop/

**Option A — VS Code Dev Container (recommended, keeps Copilot Chat working)**
The GitHub Copilot extension runs on the host side of VS Code and talks to whatever workspace is open, so it works transparently inside a [Dev Container](https://code.visualstudio.com/docs/devcontainers/containers) — you don't lose the Copilot Chat UI.
1. Install the **Dev Containers** extension in VS Code (`ms-vscode-remote.remote-containers`).
2. Add a `.devcontainer/devcontainer.json` to the repo, e.g.:
   ```json
   {
     "name": "development-with-AI",
     "image": "mcr.microsoft.com/dotnet/sdk:10.0",
     "features": {
       "ghcr.io/devcontainers/features/node:1": { "version": "22" }
     },
     "postCreateCommand": "npm install -g @github/copilot",
     "customizations": {
       "vscode": {
         "extensions": ["GitHub.copilot", "GitHub.copilot-chat"]
       }
     }
   }
   ```
3. `Ctrl+Shift+P` → "Dev Containers: Reopen in Container". VS Code builds the container, installs the .NET SDK and Copilot CLI inside it, and Copilot Chat keeps working as normal since you sign in through the host UI.

**Option B — Plain Docker container (CLI only, no VS Code)**
Useful for the Copilot CLI exercises without installing Node/.NET on the host:
```
docker run -it --rm \
  -v "$(pwd)":/workspace -w /workspace \
  mcr.microsoft.com/dotnet/sdk:10.0 bash
```
Then inside the container:
```
curl -fsSL https://deb.nodesource.com/setup_22.x | bash -
apt-get install -y nodejs
npm install -g @github/copilot
copilot
```
- `/login` inside the CLI triggers a device-code flow: it prints a URL and a one-time code. Open that URL in a browser on your host machine (the container has no GUI) and enter the code to link your GitHub account — the session then stays authenticated inside the container.
- Since the container is ephemeral (`--rm`), re-authentication is needed each time you recreate it unless you persist the Copilot config directory (typically `~/.config/@github/copilot` or similar) as a volume, e.g. add `-v copilot-config:/root/.config/@github/copilot`.

**Not a fit if**: Docker Desktop is blocked in your environment — use Podman below instead.

## Podman-based setup

Choose this over Docker specifically when your company blocks Docker Desktop (e.g. licensing restrictions under Docker's subscription terms for larger organizations) or doesn't allow it. [Podman](https://podman.io/) is a drop-in, daemonless alternative — same CLI syntax, no background daemon, and no Docker Desktop license involved. If Docker is already allowed in your environment, just use Docker — it has more first-party tooling/documentation.
- Install Podman Desktop or the CLI: https://podman.io/docs/installation
- The Docker section's Option B works unchanged — just replace `docker` with `podman`:
  ```
  podman run -it --rm \
    -v "$(pwd)":/workspace -w /workspace \
    mcr.microsoft.com/dotnet/sdk:10.0 bash
  ```
- For the Docker section's Option A (VS Code Dev Containers), point the extension at Podman instead of Docker:
  - Install/run [Podman Desktop](https://podman.io/) or start the Podman machine (`podman machine init && podman machine start` on macOS/Windows).
  - In VS Code settings, set `"dev.containers.dockerPath": "podman"` (and `"dev.containers.dockerComposePath": "podman-compose"` if using compose files).
  - `Ctrl+Shift+P` → "Dev Containers: Reopen in Container" works exactly as before, now backed by Podman.
- If you want zero config changes at all, some shells let you alias `docker` to `podman` (`alias docker=podman`), which the Dev Containers extension will also pick up.
