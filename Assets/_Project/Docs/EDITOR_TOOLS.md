# Editor Tools

## Purpose

Editor workflows for validation, cleanup, and rapid scene access.

## Responsibilities

| Tool | Menu |
|------|------|
| ProjectSetup | Studio/Setup/Initialize Project |
| ProjectValidator | Studio/Validate/Run Full Validation |
| MissingReferenceScanner | Studio/Validate/Missing References |
| MissingScriptRemover | Studio/Tools/Remove Missing Scripts |
| SceneSwitcher | Studio/Scenes/* |
| ScriptGenerator | Assets/Create/Studio/C# Script |

## Dependencies

`Project.Editor` assembly, Editor-only platform.

## Usage Examples

Run full validation before release candidates. Use **Initialize Project** on fresh clone.

## Common Workflows

1. **Initialize Project** after package import.
2. **Run Full Validation** — fix reported issues.
3. **Scene Switcher** for quick navigation.

## Extension Guide

Add checks to `ProjectValidator.RunValidation()` with new `ValidationResult` entries.

## Best Practices

- Run validator in CI via Unity batchmode if needed.
- Never remove missing scripts without version control backup.
