# fixed-cost-management Specification

## Purpose

Lets the household manage reusable fixed-cost tariff types and dated prices, with each price referencing its tariff type without duplicating the type's name or attributes.

## Requirements

### Requirement: View fixed costs
The application MUST provide a Fixed costs window that shows each saved fixed-cost entry with its start date, tariff type name, price in euros, and timeframe and energy type derived from the tariff type referenced by its GUID. The grid MUST default to newest start date first and allow ordering by tariff type name ascending followed by newest start date first. It MUST provide no additional ordering or filtering options.

#### Scenario: Open fixed costs
- **WHEN** the user selects Fixed costs from the main window
- **THEN** the application opens the fixed-cost window and displays all saved entries

#### Scenario: Default fixed-cost ordering
- **WHEN** the fixed-cost window opens
- **THEN** entries are ordered by start date from newest to oldest

#### Scenario: Order fixed costs by tariff type
- **WHEN** the user orders the grid by fixed-cost tariff type
- **THEN** entries are ordered by tariff type name and then start date from newest to oldest

#### Scenario: Display type-derived fields
- **WHEN** a fixed-cost entry is displayed
- **THEN** its type name, timeframe, and energy type are resolved from its referenced tariff type

### Requirement: Manage fixed-cost tariff types
The application MUST allow the user to add, edit, and delete fixed-cost tariff types. Each type MUST have a GUID identifier, a name, a timeframe of Daily or Monthly, and an energy type of Energy, Gas, Solar, or Generic. Names MUST be unique after trimming surrounding whitespace and comparing without regard to case. A type's GUID MUST remain unchanged when edited, and a type referenced by any fixed-cost entry MUST NOT be deletable.

#### Scenario: Add a tariff type
- **WHEN** the user adds a tariff type with a unique name and selects a timeframe and energy type
- **THEN** the application generates a GUID and stores the type with those values

#### Scenario: Select Generic as an energy type
- **WHEN** the user adds or edits a tariff type
- **THEN** Generic is available alongside Energy, Gas, and Solar in the energy-type choices

#### Scenario: Reject a duplicate tariff type name
- **WHEN** the user adds or edits a tariff type with a name that duplicates another name after trimming and case-insensitive comparison
- **THEN** the application rejects the change and identifies the name conflict

#### Scenario: Edit a tariff type
- **WHEN** the user edits a tariff type's name, timeframe, or energy type
- **THEN** the application saves the changed values while retaining the same GUID, and fixed-cost entries referencing it display the updated values

#### Scenario: Prevent deletion of a referenced tariff type
- **WHEN** the user attempts to delete a tariff type referenced by one or more fixed-cost entries
- **THEN** the application prevents deletion and explains that the type is in use

#### Scenario: Delete an unused tariff type
- **WHEN** the user deletes a tariff type that no fixed-cost entry references
- **THEN** the application removes that tariff type

### Requirement: Manage fixed-cost entries
The application MUST allow the user to add, edit, and delete fixed-cost entries. An entry MUST have a start date, a reference to an existing tariff type GUID, and a signed euro price with any decimal scale supported by .NET `decimal`, without a fixed two-decimal-place limit. The start date MUST be selectable with a date picker and MUST NOT be editable after the entry is created. There MUST be at most one entry for each start-date and tariff-type pair.

#### Scenario: Add a fixed-cost entry
- **WHEN** the user adds an entry with a start date, existing tariff type, and valid signed decimal price
- **THEN** the application stores the entry and shows it in the grid with fields resolved from its tariff type

#### Scenario: Require a tariff type before adding a fixed cost
- **WHEN** no tariff types exist
- **THEN** the application prevents fixed-cost entry creation and provides access to tariff-type management

#### Scenario: Edit a fixed-cost entry
- **WHEN** the user edits a selected entry's tariff type or price
- **THEN** the application saves the changes while preserving the original start date

#### Scenario: Reject a duplicate start-date and tariff-type pair
- **WHEN** the user adds or edits an entry to use a start date and tariff type already used by another entry
- **THEN** the application rejects the change and identifies the conflicting entry

#### Scenario: Accept a negative price with more than two decimal places
- **WHEN** the user enters a negative valid decimal price with more than two digits after the decimal separator
- **THEN** the application accepts and preserves the signed value without restricting it to two decimal places

#### Scenario: Delete a fixed-cost entry
- **WHEN** the user deletes a selected fixed-cost entry
- **THEN** the application removes the entry and refreshes the grid

### Requirement: Persist fixed costs and tariff types separately
Read both fixed-cost file paths from `appsettings.json`. Store entries in `fixedCosts.dat` and types in `fixedCostTariffTypes.dat` as JSON arrays. Entries MUST store only tariff type GUIDs. Missing files represent empty collections. Start with new empty data; do not migrate or modify legacy reference-app data.

#### Scenario: Load fixed-cost data
- **WHEN** one or both fixed-cost JSON files exist and contain valid arrays
- **THEN** the application loads their records and resolves fixed-cost type details by GUID

#### Scenario: Start with missing fixed-cost files
- **WHEN** either fixed-cost file does not exist
- **THEN** the application treats the corresponding collection as empty and remains usable

#### Scenario: Use configured fixed-cost file paths
- **WHEN** the application loads `FixedCostsFilePath` and `FixedCostTariffTypesFilePath` from `appsettings.json`
- **THEN** it loads and saves each collection at its configured path without deriving either path from the consumption file

#### Scenario: Persist fixed-cost changes
- **WHEN** the user adds, edits, or deletes a fixed cost or tariff type
- **THEN** the application persists the corresponding collection to its designated JSON file

#### Scenario: Do not import legacy data
- **WHEN** legacy fixed-cost data exists in the reference application's files
- **THEN** the new application does not import, overwrite, or modify that data

### Requirement: Test fixed-cost rules and persistence
The fixed-cost domain rules and JSON persistence MUST have automated unit-test coverage under the project's latest recommended code-analysis settings. A fully automated UI test suite is not required.

#### Scenario: Verify fixed-cost behavior with unit tests
- **WHEN** the unit-test suite is run
- **THEN** it covers ordering, GUID references, duplicate-name and duplicate-entry validation, referenced-type deletion prevention, signed decimal prices, and missing-file behavior
