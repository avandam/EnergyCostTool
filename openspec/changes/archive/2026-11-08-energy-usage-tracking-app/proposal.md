# Proposal

## Why

The existing reference application provides a starting point for tracking household energy usage, but the new application needs a clear usage-first interface and safe editing of monthly readings. This change establishes a separate WPF application that can preserve the household's existing consumption data while making current-year usage and full history easy to maintain.

## What Changes

- Create a new C# WPF application based on the structure and behavior of `CurrentCode`, keeping the reference application intact.
- Show current-year usage in the main window, including solar generation, grid returns, directly used energy, grid usage, and gas.
- Add a usage window with a newest-first history grid and controls to add, edit, and delete monthly readings, confirming before deletion.
- Preserve the existing consumption JSON array and property names when saving usage data.
- Accept whole-number readings and reject entries where returned electricity exceeds generation, which would make directly used energy negative.
- Show placeholder navigation for tariffs, fixed costs, and financial views that are outside this first feature.

## Capabilities

### New Capabilities

- `energy-usage-tracking`: Record, view, and maintain monthly household energy consumption.

### Modified Capabilities

None.

## Impact

- Adds a new WPF application and its usage interface alongside the existing `CurrentCode` reference.
- Adds or adapts C# models and JSON persistence for the existing consumption data format.
- No external service or new dependency is required by this feature.
