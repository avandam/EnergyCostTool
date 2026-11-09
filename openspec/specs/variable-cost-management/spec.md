# variable-cost-management Specification

## Purpose

Lets users review and maintain the household's variable energy tariff prices, including the rates used for electricity imports, electricity returns, and gas.

## Requirements

### Requirement: View variable tariff entries
The application MUST provide a Tariffs window showing each saved entry's calendar month and year, normal rate, low rate, normal-return cost, normal-return compensation, low-return cost, low-return compensation, and gas rate. The grid MUST be ordered by month from newest to oldest and MUST provide no other ordering or filtering.

#### Scenario: Open tariffs
- **WHEN** the user selects Tariffs from the main window
- **THEN** the application opens the Tariffs window and displays all saved entries

#### Scenario: Order entries newest first
- **WHEN** the Tariffs window opens
- **THEN** entries appear in descending calendar-month order

#### Scenario: Display all tariff fields
- **WHEN** a tariff entry is displayed
- **THEN** the grid shows its month and year, Norm, Dal, Norm T kosten, Norm T vergoeding, Dal T kosten, Dal T vergoeding, and Gas prices

### Requirement: Manage variable tariff entries
The Tariffs window MUST allow users to add, edit, and delete entries. The user MUST select a month and year, without selecting a day, and MUST NOT be able to change an entry's month after creation. At most one entry may exist for each calendar month.

#### Scenario: Add an entry
- **WHEN** the user adds a tariff entry for a month and year with valid prices
- **THEN** the application stores the entry and refreshes the grid in newest-first order

#### Scenario: Edit an entry
- **WHEN** the user edits a selected entry's prices
- **THEN** the application saves the changed prices while preserving the original month

#### Scenario: Reject a duplicate month
- **WHEN** the user adds an entry for a month that already has a tariff entry
- **THEN** the application rejects the save and identifies the conflicting month

#### Scenario: Delete an entry
- **WHEN** the user deletes a selected tariff entry
- **THEN** the application removes it and refreshes the grid

### Requirement: Accept signed decimal tariff prices
Each tariff price MUST accept positive, zero, or negative euro amounts with any decimal scale supported by .NET `decimal`, without a fixed two-decimal-place limit.

#### Scenario: Save negative prices with extended precision
- **WHEN** the user saves valid negative tariff prices with more than two digits after the decimal separator
- **THEN** the application accepts and preserves their signed decimal values

### Requirement: Allow return compensation for every month
The normal-return and low-return compensation prices MUST be editable and persisted for entries in every calendar month, including months before January 2027.

#### Scenario: Save return compensation for a historical month
- **WHEN** the user enters return-compensation prices for a month before January 2027
- **THEN** both values remain editable and are saved as entered

#### Scenario: Save return compensation for a later month
- **WHEN** the user enters return-compensation prices for January 2027 or a later month
- **THEN** both values are editable and are saved as entered

### Requirement: Persist variable tariffs in a separate JSON file
The application MUST load and save tariff entries as a JSON array at the path configured by `VariableCostFilePath` in `appsettings.json`. Missing data files MUST represent an empty collection, and the application MUST NOT automatically import legacy tariff data.

#### Scenario: Start with a missing variable-cost file
- **WHEN** the configured variable-cost file does not exist
- **THEN** the Tariffs window opens with no entries and the application remains usable

#### Scenario: Persist tariff changes
- **WHEN** the user successfully adds, edits, or deletes a tariff entry
- **THEN** the application saves the resulting collection to the configured JSON file

#### Scenario: Do not automatically import legacy tariffs
- **WHEN** legacy tariff data exists but the configured variable-cost file does not
- **THEN** the application treats variable costs as empty and does not automatically import the legacy data

### Requirement: Test variable tariff rules and persistence
Variable-tariff validation, ordering, compensation-date rules, and JSON persistence MUST have automated unit-test coverage. A fully automated UI test suite is not required.

#### Scenario: Verify tariff behavior with unit tests
- **WHEN** the unit-test suite is run
- **THEN** it covers duplicate-month rejection, month normalization, signed decimal prices, compensation values before and after January 2027, newest-first ordering, and missing-file behavior
