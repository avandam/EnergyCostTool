# Design

## Context

See `proposal.md` for motivation and `specs/fixed-cost-management/spec.md` for observable behavior. The application is a new .NET 10 WPF app with a Core project for domain rules and JSON persistence. The reference app stores fixed costs as enum-backed records in `fixedCosts.dat`; this feature starts with its own empty data and uses GUID-linked tariff types instead.

## Goals / Non-Goals

**Goals:**

- Keep fixed-cost validation and persistence outside WPF code so domain behavior can be unit tested.
- Store each tariff type once and resolve its display name, timeframe, and energy type from each fixed-cost entry's GUID.
- Keep the two data collections independent so either missing file loads as an empty collection.
- Leave `CurrentCode` and its legacy files unchanged.

**Non-Goals:**

- Import or migrate enum-based fixed-cost files from `CurrentCode`.
- Add fixed-cost calculations to calendar-year, book-year, or solar financial views.
- Add filtering or sorting modes beyond the specified default date order and tariff-type order.

## Decisions

### Add a fixed-cost domain to the Core project

Represent a tariff type with `Guid Id`, `Name`, a Daily/Monthly timeframe, and an Energy/Gas/Solar/Generic energy type. Represent a fixed cost with `StartDate`, `TariffTypeId`, and `decimal Price`. A read model joins those values for display; the persisted entry does not duplicate type metadata.

Alternative considered: reuse the reference app's enum and `double` model. Rejected because types must be user-managed, entries must refer to them by GUID, and signed high-precision currency is more appropriately represented by `decimal`.

### Configure all JSON data paths explicitly

Store `ConsumptionFilePath`, `FixedCostsFilePath`, and `FixedCostTariffTypesFilePath` as separate entries in `appsettings.json`; the two fixed-cost settings default to `fixedCosts.dat` and `fixedCostTariffTypes.dat`. Pass the explicit fixed-cost paths into their repositories instead of deriving them from the consumption path. Each repository loads a missing file as an empty collection and creates its file on the first successful write. Use the existing atomic temporary-file-then-replace pattern. Do not scan or import the reference application's output folders.

Alternative considered: derive fixed-cost paths from `ConsumptionFilePath` or search legacy output folders. Rejected because each data file needs an explicit, user-editable location and automatic discovery could accidentally migrate or modify legacy data.

### Enforce stable references and domain uniqueness in Core

Generate tariff-type GUIDs when adding types and preserve IDs on edits. Normalize names by trimming and compare case-insensitively for uniqueness. Refuse deletion when any fixed-cost record refers to the ID. Treat `(calendar date, tariff type ID)` as the fixed-cost key and reject duplicate adds or edits. The start date stays fixed on edit; changing a type or its attributes changes the joined display for its entries.

Alternative considered: duplicate entries with the same effective date and type or copy type attributes into every entry. Rejected because duplicates make the active tariff ambiguous and copied attributes could drift from the managed tariff type.

### Use dedicated WPF windows with derived display columns

Replace the main-window Fixed costs placeholder with a window containing a read-only grid, add/edit/delete actions, and a button opening tariff-type management. Display the default rows newest-first. Sorting by type uses the unique type name ascending and then start date descending. The type manager has its own list and add/edit form with dropdowns for timeframe and energy type. The fixed-cost form uses a date picker, a type-name dropdown, and locale-aware signed decimal input; the start date is read-only while editing.

Alternative considered: combine tariff-type and fixed-cost editing in one form or expose multiple filters. Rejected because managing reusable types is a distinct workflow, and only the two requested orderings are needed.

### Test behavior at the Core boundary

Add unit coverage for type normalization and uniqueness, ID stability, referenced-type delete prevention, fixed-cost uniqueness and immutable dates, sorting, signed decimal persistence, and missing files. Keep UI tests manual or omitted; no fully automated UI suite is in scope.

## Risks / Trade-offs

- **A tariff type can be changed after historical costs reference it** → Preserve the GUID and derive display values from the latest saved type record, as specified; this intentionally updates those entries' displayed metadata.
- **One data file can be absent while the other exists** → Load each collection independently and initialize a missing file only when that collection is first saved.
- **The configured data directory may be unavailable** → Report a clear persistence error and do not silently choose a different directory.

## Migration Plan

No migration is performed. The app ignores enum-backed files under `CurrentCode`; new files are created in the configured data directory as the user saves new tariff types or costs. Rollback is closing the new app and continuing to use `CurrentCode`; its files remain untouched.
