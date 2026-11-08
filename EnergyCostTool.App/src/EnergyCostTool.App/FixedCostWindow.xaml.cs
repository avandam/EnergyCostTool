using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.IO;
using EnergyCostTool.Core;

namespace EnergyCostTool.App;

public partial class FixedCostWindow : Window
{
    private readonly FixedCostService service;
    private TariffTypeWindow? tariffTypeWindow;

    public FixedCostWindow(FixedCostService service)
    {
        ArgumentNullException.ThrowIfNull(service);
        this.service = service;
        InitializeComponent();
        RefreshEntries();
    }

    public void RefreshEntries()
    {
        FixedCostSortOrder sortOrder = SortOrderComboBox.SelectedIndex == 1
            ? FixedCostSortOrder.TariffType
            : FixedCostSortOrder.StartDate;
        FixedCostGrid.ItemsSource = service.GetFixedCosts(sortOrder);
        EmptyLabel.Visibility = FixedCostGrid.Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        UpdateSelectionButtons();
    }

    private void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (IsInitialized)
        {
            RefreshEntries();
        }
    }

    private void ManageTariffTypes_Click(object sender, RoutedEventArgs e)
    {
        if (tariffTypeWindow is { IsLoaded: true })
        {
            tariffTypeWindow.Activate();
            return;
        }

        tariffTypeWindow = new TariffTypeWindow(service, RefreshEntries) { Owner = this };
        tariffTypeWindow.Closed += (_, _) => tariffTypeWindow = null;
        tariffTypeWindow.Show();
    }

    private void FixedCostGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateSelectionButtons();

    private void UpdateSelectionButtons()
    {
        bool hasSelection = FixedCostGrid?.SelectedItem is FixedCostView;
        if (EditButton is not null) EditButton.IsEnabled = hasSelection;
        if (DeleteButton is not null) DeleteButton.IsEnabled = hasSelection;
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        IReadOnlyList<FixedCostTariffType> tariffTypes = service.GetTariffTypes();
        if (tariffTypes.Count == 0)
        {
            MessageBox.Show("Add a tariff type before entering a fixed cost. The tariff type manager will open now.", "No tariff types", MessageBoxButton.OK, MessageBoxImage.Information);
            ManageTariffTypes_Click(sender, e);
            return;
        }

        FixedCostEditorWindow editor = new(tariffTypes, null) { Owner = this };
        if (editor.ShowDialog() != true || editor.StartDateValue is not { } startDate || editor.TariffTypeIdValue is not { } tariffTypeId || editor.PriceValue is not { } price)
        {
            return;
        }

        Save(() => service.AddFixedCost(startDate, tariffTypeId, price));
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (FixedCostGrid.SelectedItem is not FixedCostView selected)
        {
            return;
        }

        FixedCostEditorWindow editor = new(service.GetTariffTypes(), selected) { Owner = this };
        if (editor.ShowDialog() != true || editor.TariffTypeIdValue is not { } tariffTypeId || editor.PriceValue is not { } price)
        {
            return;
        }

        Save(() => service.UpdateFixedCost(selected.StartDate, selected.TariffTypeId, tariffTypeId, price));
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (FixedCostGrid.SelectedItem is not FixedCostView selected)
        {
            return;
        }

        Save(() => service.DeleteFixedCost(selected.StartDate, selected.TariffTypeId));
    }

    private void Save(Action operation)
    {
        try
        {
            operation();
            RefreshEntries();
        }
        catch (FixedCostValidationException exception)
        {
            MessageBox.Show(exception.Message, "Check fixed-cost entry", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        catch (IOException exception)
        {
            ShowSaveError(exception.Message);
        }
        catch (UnauthorizedAccessException exception)
        {
            ShowSaveError(exception.Message);
        }
    }

    private static void ShowSaveError(string message) => MessageBox.Show($"The fixed-cost data could not be saved.\n\n{message}", "Save failed", MessageBoxButton.OK, MessageBoxImage.Error);
}
