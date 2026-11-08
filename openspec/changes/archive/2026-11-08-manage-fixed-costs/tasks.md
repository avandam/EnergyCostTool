# Tasks

## 1. Implement fixed-cost domain rules

- [x] 1.1 Add tariff-type and fixed-cost models with GUID references, Daily/Monthly and Energy/Gas/Solar values, and signed `decimal` prices; verify both projects build with the latest recommended analysis and add calculation-free model tests.
- [x] 1.2 Implement Core validation for non-empty trimmed tariff-type names, case-insensitive name uniqueness, valid enum choices, normalized start dates, and unique `(StartDate, TariffTypeId)` pairs; verify these rules with unit tests.
- [x] 1.3 Add Generic as a fixed-cost energy type and verify it appears in the managed choices and survives JSON serialization and deserialization.

## 2. Add JSON persistence

- [x] 2.1 Implement independent JSON repositories for `fixedCosts.dat` and `fixedCostTariffTypes.dat` at their configured paths, loading absent files as empty and writing atomically on save; verify missing-file, serialization, and round-trip tests without touching legacy `CurrentCode` files.
- [x] 2.2 Add Core services that coordinate type and cost changes, preserve IDs and start dates during edits, prevent deletion of referenced types, resolve type display data by GUID, and persist consistent collections; verify CRUD, signed decimal precision, and referential integrity with unit tests.
- [x] 2.3 Read `FixedCostsFilePath` and `FixedCostTariffTypesFilePath` from `appsettings.json`, validate both settings, and pass them directly to the repositories; verify custom configured paths are used and both projects build.

## 3. Build fixed-cost management windows

- [x] 3.1 Replace the Fixed costs placeholder with a window showing start date, type name, price, derived timeframe, and energy type; verify default newest-first and type-name-then-newest sorting behavior in unit tests and build the WPF project.
- [x] 3.2 Add tariff-type management with list, add, edit, delete, immutable GUIDs, unique-name feedback, Daily/Monthly timeframe choices, Energy/Gas/Solar/Generic energy-type choices, and prevention of deleting referenced types; verify the WPF project builds and Core validation tests pass.
- [x] 3.3 Add fixed-cost add/edit/delete forms with date picker, tariff-type dropdown, signed decimal input without a two-decimal limit, and immutable start date on edit; verify the WPF project builds and Core validation tests pass.

## 4. Integrate and verify

- [x] 4.1 Wire Fixed costs navigation to the new window, refresh both lists after successful changes, and ensure no legacy data is imported; verify both app and test projects build and the full unit suite passes.
