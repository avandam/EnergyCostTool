# Proposal

## Why

The main window already exposes a Tariffs action, but it is still a placeholder. The application needs a dedicated way to review and maintain the variable energy prices used by the household, while keeping this data separate from the legacy reference application.

## What Changes

- Replace the Tariffs placeholder with a window that lists monthly tariff entries from newest month to oldest.
- Allow users to add, edit, and delete entries by selecting a month and year; an entry's month is fixed after creation.
- Support signed euro prices with any precision representable by .NET `decimal`.
- Keep both return-compensation prices editable for every month.
- Persist entries to a separately configured `variableCost.dat` JSON file. A missing file represents an empty collection, and the application does not automatically import legacy tariff data.
- Provide a separate one-time conversion of the supplied `energyPrices.dat`: cap Norm, Dal, and Gas using their corresponding source cap values, copy the old return prices into their compensation fields, and set return-cost fields to zero.
- Allow at most one entry per calendar month and reject duplicate months. Represent each selected month by its first day in storage.
- Add automated unit-test coverage for tariff rules and persistence. Fully automated UI tests are not required.

## Capabilities

### New Capabilities

- `variable-cost-management`: Review, edit, validate, and persist variable energy tariff entries.

### Modified Capabilities

- `energy-usage-tracking`: Selecting Tariffs or Fixed costs opens its management window instead of showing the unimplemented-feature message.

## Impact

The change affects the WPF main-window navigation and new tariff management windows, the Core tariff model/rules/service/repository and application settings, `appsettings.json`, and the unit-test project. It adds a `VariableCostFilePath` setting and a new JSON data file; the reference application's existing tariff data is left untouched.
