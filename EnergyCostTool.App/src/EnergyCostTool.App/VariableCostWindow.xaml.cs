using System.IO;
using System.Windows;
using System.Windows.Controls;
using EnergyCostTool.Core;

namespace EnergyCostTool.App;

public partial class VariableCostWindow : Window
{
    private readonly VariableCostService service;

    public VariableCostWindow(VariableCostService service)
    {
        ArgumentNullException.ThrowIfNull(service);
        this.service = service;
        InitializeComponent();
        RefreshEntries();
    }

    public void RefreshEntries()
    {
        VariableCostGrid.ItemsSource = service.GetEntries();
        EmptyLabel.Visibility = VariableCostGrid.Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        UpdateSelectionButtons();
    }

    private void VariableCostGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateSelectionButtons();

    private void UpdateSelectionButtons()
    {
        bool hasSelection = VariableCostGrid?.SelectedItem is VariableCostEntry;
        if (EditButton is not null) EditButton.IsEnabled = hasSelection;
        if (DeleteButton is not null) DeleteButton.IsEnabled = hasSelection;
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        VariableCostEditorWindow editor = new(null) { Owner = this };
        if (editor.ShowDialog() == true && editor.EntryValue is { } entry)
        {
            Save(() => service.Add(entry));
        }
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (VariableCostGrid.SelectedItem is not VariableCostEntry selected)
        {
            return;
        }

        VariableCostEditorWindow editor = new(selected) { Owner = this };
        if (editor.ShowDialog() == true && editor.EntryValue is { } updated)
        {
            Save(() => service.Update(selected.StartDate, updated));
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (VariableCostGrid.SelectedItem is VariableCostEntry selected)
        {
            Save(() => service.Delete(selected.StartDate));
        }
    }

    private void Save(Action operation)
    {
        try
        {
            operation();
            RefreshEntries();
        }
        catch (VariableCostValidationException exception)
        {
            MessageBox.Show(exception.Message, "Check variable tariff", MessageBoxButton.OK, MessageBoxImage.Warning);
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

    private static void ShowSaveError(string message) =>
        MessageBox.Show($"Variable-cost data could not be saved.\n\n{message}", "Save failed", MessageBoxButton.OK, MessageBoxImage.Error);
}
