# Tasks

## 1. Create the independent WPF application

- [x] 1.1 Scaffold a new C# WPF application targeting `net10.0-windows` in a solution area beside `CurrentCode` and add a shared `Directory.Build.props` setting `AnalysisLevel` to `latest-recommended` for the app and test projects; verify the new solution builds with the .NET 10 SDK without analyzer warnings or modifying `CurrentCode`.
- [x] 1.2 Establish the new application's startup window and shared visual conventions; verify the application launches independently of the reference app.

## 2. Implement usage data and persistence

- [x] 2.1 Add monthly usage data types and JSON serialization using the existing field names and meanings; verify the attached JSON file loads without conversion and a serialize/deserialize round trip retains every existing field.
- [x] 2.2 Centralize the configurable consumption-file path with the attached file as its initial target, create the configured file as an empty JSON array when it is missing, and save through a temporary file before replacing the original; verify a missing file is initialized at the configured location and an unavailable directory reports an error without creating a separate data file.
- [x] 2.3 Implement month normalization, one-record-per-month rules, whole-number and non-negative validation, and the non-negative Directly Used constraint; verify duplicate months and returns greater than generation are rejected.
- [x] 2.4 Add NUnit unit tests for directly-used calculation, month and value validation, duplicate-month rejection, preservation of a legacy negative Directly Used record, and existing JSON field compatibility; verify they pass with `dotnet test`.

## 3. Build the usage interfaces

- [x] 3.1 Implement the main window with the current-year grid, required columns and labels, directly-used calculation, and grouped navigation; visibly flag any existing row with negative Directly Used, and verify the grid shows only current-year records and an empty state when none exist.
- [x] 3.2 Implement the Usage window with all history ordered newest first and month-year display; visibly flag any existing row with negative Directly Used, and verify all records and derived directly-used values match the persisted data.
- [x] 3.3 Add the usage form and selection-based add/edit actions, keeping Directly Used read-only; verify valid entries save and invalid or duplicate entries show a useful correction message.
- [x] 3.4 Add selected-record deletion with confirmation and cancel handling; verify confirming removes the record and declining leaves it unchanged.
- [x] 3.5 Add the remaining navigation buttons as not-yet-implemented placeholders; verify selecting them does not alter usage data.

## 4. Integrate and verify the first feature

- [x] 4.1 Refresh the usage history and current-year dashboard after successful add, edit, and delete operations; verify saved changes appear consistently in both views after reopening them.
- [x] 4.2 Verify the application preserves the existing JSON array structure and property names after add, edit, and delete, and confirm the reference application files remain unchanged.
