# Design

## Context

The WPF application has a Core project with JSON-array repositories and domain services for fixed costs. The main-window Tariffs button is currently wired to the placeholder handler. The current appsettings model already validates configured data-file paths.

## Goals / Non-Goals

**Goals:**
- Follow the existing Core service/repository and WPF window patterns.
- Keep variable tariffs in their own configured file and make price/date rules testable outside the UI.
- Preserve calendar-month semantics and decimal values through JSON persistence.

**Non-Goals:**
- Import or transform tariff rows from the reference application's `energyPrices.dat`.
- Calculate household bills from the tariff entries in this change.
- Add automated UI tests.

## Decisions

1. **Add a Core variable-tariff domain and service.** Use a tariff entry model with a start month and seven `decimal` prices. Normalize the month to its first calendar day in storage, and put one-entry-per-month validation and newest-first ordering in Core. This keeps business rules consistent across the editor, grid, and tests. Reusing the fixed-cost model is unsuitable because fixed costs reference tariff-type GUIDs and have different fields.

2. **Use a dedicated JSON-array repository and settings property.** Add `VariableCostFilePath` to `EnergyCostToolSettings`, set its default deployment value to `variableCost.dat`, and use the existing JSON-array repository pattern. Missing files load as empty collections; the first successful save creates the configured file. Storing tariffs in the consumption file or legacy `energyPrices.dat` was rejected because it would couple unrelated data and risk modifying reference data.

3. **Allow compensation prices for every month.** Do not normalize, reject, disable, or clear compensation prices based on the entry month. Keep the editor and Core service consistent so historical return-compensation values can be preserved.

4. **Keep signed prices as `decimal` values.** All seven prices accept negative values and the complete .NET `decimal` scale. Use decimal parsing and serialization instead of `double` to avoid binary rounding in monetary values. Display formatting may follow the current culture, but must not round the stored value.

5. **Use month/year selection instead of a day picker.** The editor selects a month and year, and stores its canonical date as the first day of that month. Reject a second entry for the same year and month; editing preserves the selected month. This matches the tariff's monthly validity period and makes ordering unambiguous.

6. **Add a modeless Tariffs window following the Fixed costs window.** The window owns a read-only overview grid and Add/Edit/Delete actions; an editor dialog handles month/year selection and prices. MainWindow tracks the open window like it tracks Fixed costs, and the Tariffs button receives its own handler.

## Risks / Trade-offs

- [Existing users may expect the old reference tariff data to appear] → Keep the new file empty and separate; do not read, overwrite, or migrate `energyPrices.dat`.
- [Legacy return rates could be assigned to the wrong fields during conversion] → Compare each converted month's compensation values directly with the source `ReturnElectricityHigh` and `ReturnElectricityLow` values; verify return-cost fields remain zero.
- [Locale-specific decimal input can be misread] → Parse and format using the current culture consistently, while persisting JSON numeric decimals without display rounding.

## Migration Plan

The application does not automatically import legacy data. A separate one-time conversion creates the workspace `variableCost.dat` from the supplied `energyPrices.dat`: Norm and Dal use the minimum of their source price and `ElectricityCap`, Gas uses the minimum of `Gas` and `GasCap`, the two return-cost fields are zero, and return compensation copies `ReturnElectricityHigh` and `ReturnElectricityLow`. The source file remains untouched. The application continues to load only the path configured by `VariableCostFilePath`.
