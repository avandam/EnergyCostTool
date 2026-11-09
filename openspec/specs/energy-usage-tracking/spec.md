# energy-usage-tracking Specification

## Purpose

Lets the household record and review monthly electricity generation, grid usage and returns, and gas usage while retaining compatibility with its existing consumption data.

## Requirements

### Requirement: Show current-year energy usage
The main window MUST show a grid titled "Energy usage this year" with records for the current calendar year and columns for Month, Generation, Normal Return, Low Return, Directly Used, Normal, Low, and Gas.

#### Scenario: Display current-year records
- **WHEN** the main window opens with saved usage records
- **THEN** it shows records whose month is in the current calendar year, with one row per month

#### Scenario: Compute directly used energy
- **WHEN** a current-year record is displayed
- **THEN** Directly Used equals Generation minus Normal Return minus Low Return

#### Scenario: No current-year records exist
- **WHEN** the main window opens and no usage records exist for the current calendar year
- **THEN** the usage grid is empty and the user can still open the Usage window

### Requirement: Provide grouped navigation on the main window
The main window MUST show an "Add/edit data" group with Usage, Tariffs, and Fixed costs buttons, and a "Financial" group with Per calendar year, Per bookyear, and Solar panels buttons.

#### Scenario: Open usage management
- **WHEN** the user selects Usage
- **THEN** the application opens the usage management interface

#### Scenario: Open tariff management
- **WHEN** the user selects Tariffs
- **THEN** the application opens the Tariffs window to manage variable tariff entries

#### Scenario: Open fixed-cost management
- **WHEN** the user selects Fixed costs
- **THEN** the application opens the Fixed costs window to manage fixed-cost entries

#### Scenario: Select a later feature
- **WHEN** the user selects Per calendar year, Per bookyear, or Solar panels
- **THEN** the application presents that feature as not yet implemented without changing usage data

### Requirement: Review all usage history
The Usage interface MUST show every saved monthly usage record in a grid ordered from newest month to oldest, with Month and year displayed (for example, "Sep 2026") and the same usage fields as the main window.

#### Scenario: View all saved records
- **WHEN** the user opens Usage
- **THEN** the grid shows saved records across all years in descending month order

#### Scenario: Display historical directly used energy
- **WHEN** a historical record is displayed
- **THEN** Directly Used equals Generation minus Normal Return minus Low Return

### Requirement: Add and edit monthly usage
The Usage interface MUST allow the user to add a monthly record and edit a selected record's month and whole-number usage fields. Directly Used MUST be calculated and MUST NOT be directly editable. Each calendar month MUST have at most one usage record.

#### Scenario: Add a new month
- **WHEN** the user enters a month with whole-number Generation, Normal Return, Low Return, Normal, Low, and Gas values and saves it
- **THEN** the application stores the record and refreshes the history grid in newest-first order

#### Scenario: Edit a selected month
- **WHEN** the user selects a record, changes its month or usage values, and saves the edit
- **THEN** the application stores the updated record and refreshes the history grid in newest-first order

#### Scenario: Reject a duplicate month
- **WHEN** the user attempts to add or edit a record to a month that already has a different usage record
- **THEN** the application rejects the save and identifies the duplicate month

#### Scenario: Reject non-integer or invalid values
- **WHEN** the user enters a non-integer, missing, or otherwise invalid usage value
- **THEN** the application rejects the save and identifies that the usage data needs correction

#### Scenario: Reject returns exceeding generation
- **WHEN** Normal Return plus Low Return is greater than Generation
- **THEN** the application rejects the save and does not store a record with negative Directly Used energy

### Requirement: Delete monthly usage with confirmation
The Usage interface MUST allow deletion of a selected record only after the user confirms the deletion.

#### Scenario: Confirm deletion
- **WHEN** the user selects a record, requests deletion, and confirms
- **THEN** the application removes the record from persistent storage and the history grid

#### Scenario: Cancel deletion
- **WHEN** the user requests deletion and declines the confirmation
- **THEN** the record remains unchanged in persistent storage and the history grid

### Requirement: Preserve the consumption JSON format
The application MUST load and save usage records as a JSON array using the existing property names and meanings: `Month`, `SolarGeneration`, `ElectricityHigh`, `ReturnElectricityHigh`, `ElectricityLow`, `ReturnElectricityLow`, and `Gas`.

#### Scenario: Load existing consumption data
- **WHEN** the application opens with a valid existing consumption JSON file
- **THEN** it displays the file's records without requiring a format conversion

#### Scenario: Create a missing configured consumption file
- **WHEN** the application starts and the configured consumption file does not exist in an available directory
- **THEN** it creates the file containing an empty JSON array and opens with no usage records

#### Scenario: Persist an added, edited, or deleted record
- **WHEN** the user successfully adds, edits, or deletes a record
- **THEN** the application saves the resulting records in the existing JSON array format with the existing property names

### Requirement: Preserve and flag invalid historical usage
The application MUST preserve existing monthly records whose computed Directly Used value is negative, and MUST visibly flag those records in usage grids. This legacy-data handling MUST NOT permit an added or edited record with negative Directly Used.

#### Scenario: Load a legacy record with negative directly used energy
- **WHEN** a valid JSON file contains a record where Normal Return plus Low Return exceeds Generation
- **THEN** the application displays and flags that record without changing or removing it from the file

#### Scenario: Edit an invalid historical record
- **WHEN** the user edits a flagged record and submits values that still make Directly Used negative
- **THEN** the application rejects the edit and keeps the previously saved record unchanged

### Requirement: Maintain code-analysis and unit-test quality
The application and its tests MUST use the latest recommended .NET code-analysis rules, and relevant non-UI usage logic and persistence behavior MUST have automated unit-test coverage. A fully automated UI test suite is not required.

#### Scenario: Build with recommended code analysis
- **WHEN** the application and test projects are built with the supported .NET SDK
- **THEN** they use the SDK's latest recommended code-analysis rules and the new code has no unresolved analyzer warnings

#### Scenario: Verify usage logic and persistence with unit tests
- **WHEN** the unit-test suite is run
- **THEN** it covers directly-used calculation, usage validation, month uniqueness, and compatibility with the existing JSON fields
