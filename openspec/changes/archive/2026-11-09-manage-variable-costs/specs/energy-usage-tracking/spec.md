# Spec Delta

## MODIFIED Requirements

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
