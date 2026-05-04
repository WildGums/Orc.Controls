# Orc.Controls

Orc.Controls is a WPF UI controls library by WildGums. It provides a comprehensive set of reusable controls for Line of Business applications, including date/time pickers, log viewers, color pickers, validation controls, and many more.

Orc.Controls consists of the following projects:

- `Orc.Controls` — Core WPF controls library.
- `Orc.Controls.Settings` — Settings infrastructure for controls.
- `Orc.Controls.Tests` — Unit and UI tests for the library.

---

## Critical Rules (Read First)

These rules are **non-negotiable**. Violating them causes broken builds, crashes, or downstream breakage.

### 1. Never Edit Generated Files

Files matching `*.generated.cs`, `*.generated.xaml` are auto-generated.

- **NEVER** manually edit these files

### 2. ABI / API Stability

This project maintains stable ABI / API. Breaking changes break downstream apps.

| Allowed | Never |
|---------|-------|
| Add new overloads | Modify existing signatures |
| Add new methods | Remove public APIs |
| Add new classes | Change return types |

The `PublicApiFacts` tests in `Orc.Controls.Tests` automatically detect breaking public API changes. These tests must pass.

### 3. Tests Are Mandatory

**Building alone is NOT sufficient.** Run tests before claiming completion (see [Commands](#commands)).

### 4. Branch Protection (COMPLIANCE REQUIRED)

**Direct commits to protected branches are a policy violation.**

| Repository | Protected Branches |
|------------|-------------------|
| Orc.Controls | `master` |
| Orc.Controls | `develop` |

**Required workflow:**

1. **Create a feature branch FIRST** — Use naming convention: `feature/issue-NNNN-description`
2. **Make all commits on the feature branch** — Never commit directly to protected branches
3. **Submit a Pull Request** — Changes must be reviewed by a human before merging

```bash
# CORRECT — Always create a feature branch first
git checkout -b feature/issue-1234-fix-description

# NEVER DO THIS — Policy violation
git checkout develop && git commit  # FORBIDDEN

# NEVER DO THIS — Policy violation
git checkout master && git commit  # FORBIDDEN
```

---

## Commands

Single source of truth for all commands:

| Task | Command |
|------|---------|
| **Build** | `dotnet cake --target=build` |
| **Test** | `dotnet cake --target=test` |
| **Build and test** | `dotnet cake --target=buildandtest` |

---

## Architecture & Directories

### Solution Overview

```
src/
  Orc.Controls/           # Core WPF controls (DateTimePicker, LogViewer, ColorPicker, etc.)
  Orc.Controls.Settings/  # Settings infrastructure
  Orc.Controls.Tests/     # NUnit unit and UI tests
  Orc.Controls.Example/   # Example application demonstrating controls
deployment/               # Build and deployment scripts
design/                   # Design assets
```

### Controls Overview

The `Orc.Controls/Controls/` directory contains each control in its own subdirectory:

- `DateTimePicker`, `DateRangePicker`, `TimePicker`, `TimeSpanPicker`
- `ColorPicker`, `ColorLegend`
- `LogViewer`
- `FilterBox`, `WatermarkTextBox`
- `OpenFilePicker`, `SaveFilePicker`, `DirectoryPicker`
- `ValidationContextControl`
- `NumericTextBox`, `NumericUpDown`
- `DropDownButton`, `LinkLabel`, `StepBar`, `RangeSlider`
- And many more

### Directory Guide

| Directory | Editable? | Notes |
|-----------|-----------|-------|
| `*.generated.cs` | No | Leave as-is |
| `*.generated.xaml` | No | Leave as-is |
| `deployment/` | No | Deployment / build scripts |
| `src/Orc.Controls/Controls/` | Yes | Individual WPF control implementations |
| `src/Orc.Controls/Themes/` | Yes | XAML resource dictionaries and styles |
| `src/Orc.Controls.Tests/` | Yes | Tests for all controls and utilities |

---

## Writing Code

### Anti-Patterns (Never Do This)

| Anti-Pattern | Why |
|-------------|-----|
| Modifying method signatures | ABI breaking |
| Manual edits to `*.generated.cs`, `*.generated.xaml` | Overwritten on regenerate |
| Using default parameters in public APIs | ABI breaking |
| **Skipping failing tests** | **Unacceptable — tests must pass** |

---

## Testing & Debugging

### Running Tests

```bash
dotnet cake --target=test
```

### Tests MUST Pass

> **NON-NEGOTIABLE:** Tests must PASS before claiming completion.
>
> - Do NOT skip failing tests
> - Do NOT claim completion if tests fail
> - Do NOT use `SkipException` to work around failures

### Writing Tests

1. Use NUnit to write tests
2. Name test classes with the suffix `Facts` or `Tests`
3. Combine Pascal / Snake case for test methods (e.g. `Feature_Does_Work`)

```csharp
[TestFixture]
public class MyControlFacts
{
    [Test]
    public void Feature_Does_Work()
    {
        var result = 47 - 5;

        Assert.That(result, Is.EqualTo(42));
    }
}
```

**Philosophy:** Tests FAIL when wrong, never skip (except missing hardware).

### Public API Tests

The `PublicApiFacts` class verifies that no breaking changes are introduced to the public API:

- `Orc_Controls_HasNoBreakingChanges_Async` — Checks `Orc.Controls` assembly
- `Orc_Controls_Settings_HasNoBreakingChanges_Async` — Checks `Orc.Controls.Settings` assembly

When intentionally adding new public API, update the corresponding `.verified.txt` snapshot files in `Orc.Controls.Tests/`.

### Debugging Methodology

1. **Establish baseline** — What's the known-good state?
2. **One change at a time** — Verify each change before proceeding
3. **Track changes in a table** — Log what you changed and the result
4. **Platform differences are signals** — If X works and Y fails, the difference IS the answer
5. **Revert if worse** — Don't pile fixes on top of failures

---

## Further Reading

| Topic | Document |
|-------|---------|
| Contributing guidelines | [CONTRIBUTING.md](CONTRIBUTING.md) |
| Project documentation | [opensource.wildgums.com](https://opensource.wildgums.com) |
