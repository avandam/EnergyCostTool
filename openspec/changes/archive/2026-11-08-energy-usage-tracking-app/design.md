# Design

## Context

See `proposal.md` for the motivation and `specs/energy-usage-tracking/spec.md` for user-visible behavior. `CurrentCode` is a C# WPF reference application with a monthly `Consumption` model and a JSON data access layer. Its consumption file is a JSON array with stable property names. The new application must be created alongside the reference application and must not depend on changing it.

## Goals / Non-Goals

**Goals:**

- Create an independent WPF application targeting .NET 10 LTS that can be developed and launched without replacing `CurrentCode`.
- Keep the consumption JSON schema compatible with the existing file.
- Keep usage entry simple, with one record per calendar month and directly used energy calculated from generation and returns.

**Non-Goals:**

- Implement tariff, fixed-cost, calendar-year financial, bookyear, or solar-panel financial features.
- Change the existing consumption JSON property names or add calculated fields to the persisted records.
- Modify or remove the reference application.

## Decisions

### Keep the new application independent

Add a distinct WPF project/solution area next to `CurrentCode`, taking its UI patterns and domain behavior as reference. Reuse concepts and compatible serialization, but avoid project references or edits that make the old application a runtime dependency. This lets the new app evolve without coupling its startup or release to the reference.

Alternative considered: extend `CurrentCode` directly. Rejected because the requested deliverable is a new application and the reference should remain available for comparison.

### Use WPF on .NET 10 LTS

Build the new application with WPF and target `net10.0-windows`. WPF directly matches the reference application's UI framework and is well suited to this data-entry grid and form workflow. Target .NET 10 LTS rather than inheriting the reference's .NET 8 target, so the new app starts on the newest stable long-term-support runtime available for this work.

Alternative considered: WinUI 3 on .NET 10. It is Microsoft's recommended framework for new native Windows desktop apps and offers a more current Windows design system, but this feature does not require those capabilities. WPF better preserves the reference patterns and keeps the initial implementation focused. The UI framework can be reconsidered if the product later needs a broader Fluent design refresh or Windows App SDK features.

### Preserve the existing JSON record schema

Represent each saved month with the existing fields (`Month`, `SolarGeneration`, `ElectricityHigh`, `ReturnElectricityHigh`, `ElectricityLow`, `ReturnElectricityLow`, and `Gas`) and serialize the collection as the same JSON array shape. Keep `DirectlyUsed` derived in the application model/view and do not write it to the file. Use whole-number inputs to match the existing model and data.

Alternative considered: introduce a new schema with display-oriented names or persisted derived values. Rejected because it would require a conversion and risks breaking compatibility with the current data file.

### Treat the calendar month as the record identity

Normalize entered months to the first day of the month, consistent with the reference model. Permit one record per year/month. Adding a duplicate month is rejected; editing may change the selected record's month only when the destination month is unused. Validate all fields as non-negative whole numbers and require `Normal Return + Low Return <= Generation` before saving.

Alternative considered: allow arbitrary day values or duplicate months. Rejected because readings are monthly and the dashboard and history are organized by month.

### Preserve invalid legacy records while rejecting invalid writes

Do not run the new usage-entry validation against records during file loading. The supplied data already contains one record whose returns exceed generation. Load it unchanged and visibly flag its negative Directly Used value in both usage grids. Apply the generation/returns invariant to every add and edit; if a user edits the flagged record, save only after its values satisfy the invariant. Deletion remains available through the normal confirmation flow.

Alternative considered: refuse to load the entire data file or silently rewrite the legacy row. Rejected because either choice would hide or alter the user's existing history.

### Use a main dashboard and a dedicated usage window

The main window owns the current-calendar-year grid and grouped navigation. Usage opens a separate window containing the full history grid and add/edit/delete controls, following the reference application's window pattern. After a successful change, refresh both the usage list and the main dashboard when its window is next active or receives the update.

Alternative considered: edit usage inline in the dashboard. Rejected because the dashboard is scoped to one year while maintenance needs full history and distinct add/edit/delete actions.

### Keep the data file location configurable

Use the attached `energyConsumptions.dat` file as the initial data source and persistence target, with its location held in one application setting rather than embedded in multiple DAL/UI components. This retains the user's existing data location while allowing the new app to run if the synced drive path changes. If the configured file is missing but its directory is available, create it as an empty JSON array. Preserve the file's JSON structure on every write.

Alternative considered: copy the data into the executable's working directory. Rejected because it could create a second divergent data set and would not update the attached file.

### Apply recommended analysis and test non-UI behavior

Set `<AnalysisLevel>latest-recommended</AnalysisLevel>` in a shared `Directory.Build.props` so the application and test projects use the recommended analyzer set from the installed .NET SDK. Resolve analyzer warnings in new code; document a narrow suppression only when a warning is intentional and cannot reasonably be fixed. Keep calculation, month/value validation, uniqueness rules, and JSON mapping in non-UI code that can be exercised with unit tests. Use NUnit, which is the framework used by the existing reference test sources, and avoid carrying over the reference test projects' redundant MSTest package references. Cover those deterministic behaviors with unit tests; a fully automated UI test suite is out of scope.

Alternative considered: omit automated tests. Rejected because calculations, validation, and serialization are deterministic and benefit from repeatable unit coverage. Fully automated UI tests are not needed for this first feature.

## Risks / Trade-offs

- **A save can damage the only copy of the data if interrupted** → Write the complete updated JSON to a temporary file beside the configured file, then replace the original after serialization succeeds; retain a clear error if replacement fails.
- **The configured synced-drive directory may be unavailable** → Report the path and error clearly; create a missing file only in the configured directory and do not silently create a database elsewhere.
- **A second application instance could overwrite a newer edit** → Treat concurrent writers as out of scope for this local desktop feature and avoid silently merging or discarding detected file changes where practical.
- **The latest recommended analyzer set may surface findings that add implementation work** → Address new-code findings as part of each task and document any justified suppression narrowly.

## Migration Plan

No conversion is required. Point the new application at the existing JSON file and verify it can read all entries before the first write. Keep `CurrentCode` and the source data file intact during rollout; rollback consists of closing the new app and continuing to use the reference app.
