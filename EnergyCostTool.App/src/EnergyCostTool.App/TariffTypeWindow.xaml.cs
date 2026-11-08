using System.IO;
using System.Windows;
using EnergyCostTool.Core;

namespace EnergyCostTool.App;

public partial class TariffTypeWindow : Window
{
    private readonly FixedCostService service;
    private readonly Action refreshFixedCosts;

    public TariffTypeWindow(FixedCostService service, Action refreshFixedCosts)
    {
        ArgumentNullException.ThrowIfNull(service);
        ArgumentNullException.ThrowIfNull(refreshFixedCosts);
        this.service = service;
        this.refreshFixedCosts = refreshFixedCosts;
        InitializeComponent();
        RefreshTypes();
    }

    private void RefreshTypes() => TypesGrid.ItemsSource = service.GetTariffTypes();

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        TariffTypeEditorWindow editor = new(null) { Owner = this };
        if (editor.ShowDialog() != true || 
            editor.NameValue is not { } name || 
            editor.TimeframeValue is not { } timeframe || 
            editor.EnergyTypeValue is not { } energyType)
        {
            return;
        }

        try
        {
            service.AddTariffType(name, timeframe, energyType);
            RefreshTypes();
            refreshFixedCosts();
        }
        catch (FixedCostValidationException exception)
        {
            ShowValidationError(exception.Message);
        }
        catch (IOException exception)
        {
            ShowSaveError(exception.Message);
        }
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (TypesGrid.SelectedItem is not FixedCostTariffType existing)
        {
            ShowSelectMessage();
            return;
        }

        TariffTypeEditorWindow editor = new(existing) { Owner = this };
        if (editor.ShowDialog() != true || editor.NameValue is not { } name || editor.TimeframeValue is not { } timeframe || editor.EnergyTypeValue is not { } energyType)
        {
            return;
        }

        try
        {
            service.UpdateTariffType(existing.Id, name, timeframe, energyType);
            RefreshTypes();
            refreshFixedCosts();
        }
        catch (FixedCostValidationException exception)
        {
            ShowValidationError(exception.Message);
        }
        catch (IOException exception)
        {
            ShowSaveError(exception.Message);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (TypesGrid.SelectedItem is not FixedCostTariffType existing)
        {
            ShowSelectMessage();
            return;
        }

        try
        {
            service.DeleteTariffType(existing.Id);
            RefreshTypes();
            refreshFixedCosts();
        }
        catch (FixedCostValidationException exception)
        {
            ShowValidationError(exception.Message);
        }
        catch (IOException exception)
        {
            ShowSaveError(exception.Message);
        }
    }

    private static void ShowSelectMessage() => MessageBox.Show("Select a tariff type first.", "Fixed-cost tariff types", MessageBoxButton.OK, MessageBoxImage.Information);

    private static void ShowValidationError(string message) => MessageBox.Show(message, "Check tariff type", MessageBoxButton.OK, MessageBoxImage.Warning);

    private static void ShowSaveError(string message) => MessageBox.Show($"The tariff types could not be saved.\n\n{message}", "Save failed", MessageBoxButton.OK, MessageBoxImage.Error);
}
