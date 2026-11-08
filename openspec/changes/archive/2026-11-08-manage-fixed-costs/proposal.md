# Proposal

## Why

The new Energy Cost Tool currently has a placeholder for fixed costs, while ongoing household cost data needs a dedicated place to manage the rates that apply from specific dates. Separating reusable tariff types from dated prices avoids repeating type names and keeps the usage data model extensible.

## What Changes

- Replace the main-window Fixed costs placeholder with a fixed-cost management window.
- Show fixed-cost entries with start date, tariff type name, price, and timeframe and energy type derived from the referenced tariff type; support the requested default and type-grouped ordering.
- Add fixed-cost tariff type management with immutable GUIDs, unique names, Daily/Monthly timeframes, and Energy/Gas/Solar/Generic energy types.
- Add, edit, and delete fixed-cost entries, preventing edits to an existing entry's start date and rejecting duplicate start-date/type pairs.
- Persist tariff types and fixed costs separately in `fixedCostTariffTypes.dat` and `fixedCosts.dat`, with both paths read explicitly from `appsettings.json`; treat missing files as empty and begin with new empty data without migrating or changing legacy `CurrentCode` data.
- Preserve signed euro prices with any decimal scale supported by .NET `decimal`, without imposing a two-decimal-place limit.

## Capabilities

### New Capabilities
- `fixed-cost-management`: Manage reusable fixed-cost tariff types and dated fixed-cost entries in the desktop application.

### Modified Capabilities

None.

## Impact

- Extends the new WPF application and its non-UI Core domain and persistence services.
- Adds explicit appsettings paths for the two JSON files and unit coverage for type and entry rules, persistence, and sorting.
- Replaces the Fixed costs placeholder while leaving the old `CurrentCode` application and its data untouched.
