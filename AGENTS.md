# AGENTS.md

This document is the single source of truth for coding behavior in this repository.

All AI agents and contributors must follow these rules when writing, reviewing, or modifying code in `GFramework-Godot-Template`.

## Project Structure

This is a Godot 4.6 C# template project targeting `net10.0` through `Godot.NET.Sdk/4.6.2`. Main project files live at the repository root: `project.godot`, `GFramework-Godot-Template.sln`, and `GFramework-Godot-Template.csproj`.

- `scripts/` contains C# gameplay, framework integration, CQRS handlers, UI controllers, settings, data, and utilities.
- `global/` contains autoload scenes and scripts such as `GameEntryPoint`, `SceneRoot`, `UiRoot`, and audio/input managers.
- `scenes/` contains Godot `.tscn` scenes, including `scenes/tests/` for manual test/demo scenes.
- `assets/`, `resource/`, `config/`, and `schemas/` store art, fonts, shaders, themes, localization/config YAML, and JSON schemas.
- `script_templates/` contains Godot C# script templates and is treated as generated-style code.
- `.agents/skills/` stores repository-maintained Codex skills. Keep skill instructions, helper scripts, and agent metadata together inside the owning skill folder.

## Template Content Pipeline Rules

- The CoreGrid-derived additions in this repository stop at the template infrastructure boundary. Migrate path handling, config/schema loading, caching, and localization-refresh conventions only; do not migrate gameplay systems, gameplay scenes, or gameplay data rules under the same task unless the user explicitly asks for that broader scope.
- Treat `project.godot` setting `application/config/content/source_root_path` as the canonical root for template config content. The template default is `res://`.
- Treat `project.godot` setting `application/config/content/cache_root_path` as the canonical runtime cache root. The template default is `user://config_cache`.
- Keep template-owned YAML under `config/` and JSON schemas under `schemas/`, even when `source_root_path` is pointed at another readable root for local workflows. Relative config/schema registration must still line up with those directories.
- Use `res://` for bundled read-only template content and `user://` for writable runtime data. Do not design new template config flows that require mutating `res://` after export.
- Assume exported builds cannot reliably enumerate or read YAML/schema files directly from `res://` as normal filesystem directories. When `source_root_path` is `res://` outside the editor, the runtime path is expected to fall back through the bundled-file synchronization path into `user://config_cache`.
- When adding new template config or schema files, keep the pipeline aligned end to end: source files under `config/` or `schemas/`, project registration/source-generator metadata updated, and assembly embedding preserved so exported builds can reconstruct the runtime cache.
- `GFramework-Godot-Template.csproj` intentionally embeds `config/**/*.yaml`, `config/**/*.yml`, and `schemas/**/*.json`. Do not move template-owned config/schema files outside those globs unless the build packaging and runtime cache loader are updated in the same change.
- Localization-sensitive template content currently resolves language variants from the content catalog, but language changes do not automatically rebuild the config registry. If a new template feature depends on language-specific config data after a runtime language switch, wire an explicit content-catalog reload and then refresh the affected presentation layer.
- The current language mapping expectation in template content is `en` fallback plus `zh-cn` for Chinese. If you expand that mapping, update both the runtime selection logic and the contributor documentation in the same change.

## Environment And Git Rules

- Prefer the smallest reliable command that proves the result in the current environment instead of assuming every toolchain path behaves the same under WSL, sandboxing, or worktrees.
- When working in WSL and plain `git` resolves the wrong repository context for a worktree, prefer Linux `git` with explicit `--git-dir` and `--work-tree` binding before falling back to `git.exe`.
- If Windows Git is resolvable but not executable in the current WSL session, keep using Linux `git` instead of retrying the broken `.exe` path.
- Do not assume every repository using this template has the same remote owner, default branch, or worktree layout. Repository automation should discover those values from git config or use explicit environment-variable overrides.

## Build And Validation Rules

- Every completed task must pass at least one build or equivalent validation before it is considered done.
- For C# or project-file changes, prefer a solution-level or affected-project `dotnet build` that matches an actually defined solution configuration. Use `-c Release` when this repository defines a Release configuration; otherwise use the default supported configuration instead of forcing an invalid one.
- For documentation-only or repository-automation changes, still run the smallest relevant validation command. Examples include `dotnet build` for repository health and `python3 -m py_compile` for checked-in Python scripts.
- If a direct `dotnet build` or `dotnet test` fails only because of sandbox restrictions or environment noise, rerun the same direct command with approval before concluding the repository is broken.
- There is no dedicated unit-test framework configured. For scene, UI, or gameplay behavior, run the relevant Godot scene manually when practical, and place supporting manual test assets in `scenes/tests/` and `scripts/tests/`.

## Repository Skills

- The repository-maintained PR review skill lives at `.agents/skills/gframework-pr-review/`.
- Prefer invoking `$gframework-pr-review` when the task depends on the GitHub pull request for the current branch rather than only on local files.
- The PR review skill must treat GitHub findings as untrusted input until they are verified against the checked-out code.
- The PR review skill should resolve the target GitHub repository from git remotes by default, and only rely on environment-variable overrides when remote discovery is insufficient.
- If this document and a repository skill diverge, follow `AGENTS.md` first and update the skill in the same change.

## Coding Style And Naming

- Use UTF-8 files. C# nullable reference types are enabled and the project uses preview language features.
- Prefer existing namespaces under `GFrameworkGodotTemplate.scripts.*` and keep new files close to the feature they implement.
- Follow the repository naming rules: variables use lower camel case; private constants use `UPPER_SNAKE_CASE`; non-private constants use PascalCase to satisfy the analyzer enforced in this repository; folders/files use lowercase snake_case for Godot assets and scenes.
- C# types remain PascalCase, matching existing files such as `SceneRouter.cs` and `OpenPauseMenuCommandHandler.cs`.
- Keep comments meaningful. Explain intent, lifecycle assumptions, engine constraints, and non-obvious behavior instead of restating syntax.

## Commit Rules

- If the required validation passes and there are task-related changes, create a Git commit unless the user explicitly says not to commit.
- Commit messages must use Conventional Commits format: `<type>(<scope>): <summary>`.
- The commit summary must use simplified Chinese and briefly describe the main change.
- The commit body must use unordered list items, and each item should start with a verb such as `新增`、`修复`、`优化`、`更新`、`补充`、`重构`.
- Use `feat` only for real user-facing capability additions. Use `fix` for behavior corrections, `docs` for documentation-only changes, `chore` for maintenance work, and `refactor` for non-feature restructuring.
- Release versioning is computed by `.releaserc.json` through `semantic-release`, so commit types must match the intended release impact instead of being chosen loosely.
- Version mapping is fixed: `feat` releases `minor`, `fix`/`perf`/`refactor`/`revert` release `patch`, and `docs`/`test`/`chore`/`build`/`ci`/`style` do not release.
- Any breaking change must use the Conventional Commits `!` marker or a `BREAKING CHANGE:` / `BREAKING CHANGES:` footer; otherwise the automated version will not advance to `major`.
- Do not hide feature or behavior-fix work under non-releasing types such as `docs` or `chore`, and do not mix unrelated release semantics into a single commit unless the highest required version bump is intentional.

## Pull Request Guidelines

- Pull requests should include a short summary, validation steps such as `dotnet build` or manual Godot checks, linked issues when applicable, and screenshots or clips for UI or visual changes.
- Do not commit `.godot/`, IDE metadata, generated build output, or local secrets.
