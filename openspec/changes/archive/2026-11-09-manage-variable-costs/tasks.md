# Tasks

## 1. Core settings and tariff rules

- [x] 1.1 Add and validate `VariableCostFilePath` in application settings, configure it as `variableCost.dat`, and extend settings unit tests to verify the configured value is loaded and validated.
- [x] 1.2 Add the variable-tariff entry model and Core rules/service for decimal prices, one entry per month, unrestricted compensation values, and newest-first results; add unit tests for signed values, duplicate-month rejection, and compensation preservation across historical and later months.
- [x] 1.3 Add the JSON-array repository for variable tariffs and unit tests for first-of-month normalization, decimal round-tripping, missing-file behavior, and persisted add/edit/delete results.

## 2. Tariff management UI

- [x] 2.1 Add an entry editor with month/year selection and seven euro-price inputs; keep the selected month immutable on edit and allow return compensation in every month, then verify the WPF project builds.
- [x] 2.2 Add the Tariffs window with month/year and the specified price columns, newest-first ordering, and Add/Edit/Delete actions; route the main-window Tariffs button to it and verify the WPF project builds.

## 3. Integration verification

- [x] 3.1 Run the full unit-test suite and build the application and test projects with latest-recommended analysis enabled; resolve failures and analyzer warnings.

## 4. One-time legacy data conversion

- [x] 4.1 Convert the supplied `energyPrices.dat` to workspace `variableCost.dat` using capped Norm, Dal, and Gas rates, zero return costs, and source return prices as compensation; verify all 67 months and leave the source unchanged.
