using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using EnergyCostTool.Core;

namespace EnergyCostTool.App;

public partial class UsageWindow : Window
{
    private readonly EnergyUsageService service;
    private readonly Action refreshDashboard;

    public UsageWindow(EnergyUsageService service, Action refreshDashboard)
    {
        this.service = service;
        this.refreshDashboard = refreshDashboard;
        InitializeComponent();
        RefreshRows();
    }

    private void RefreshRows()
    {
        UsageGrid.ItemsSource = service.GetAll().Select(record => new UsageRow(record)).ToArray();
    }

    private void UsageGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        UsageEditor editor = new(null) { Owner = this };
        if (editor.ShowDialog() != true || editor.Record is not { } record) 
        {
            return;
        }
        Save(() => service.Add(record));
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (UsageGrid.SelectedItem is not UsageRow row) 
        { 
            ShowSelectMessage();
            return; 
        }
        UsageEditor editor = new(row.ToRecord()) { Owner = this };
        if (editor.ShowDialog() != true || editor.Record is not { } record)
        {
            return;
        }
        Save(() => service.Update(row.Month, record));
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (UsageGrid.SelectedItem is not UsageRow row) 
        { 
            ShowSelectMessage(); 
            return; 
        }
        MessageBoxResult result = MessageBox.Show($"Delete energy usage for {EnergyUsageRules.FormatMonth(row.Month)}?", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
        if (result == MessageBoxResult.Yes) 
        {
            Save(() => service.Delete(row.Month));
        }
    }

    private static void ShowSelectMessage() => MessageBox.Show("Select a usage month first.", "Energy usage", MessageBoxButton.OK, MessageBoxImage.Information);

    private void Save(Action operation)
    {
        try
        {
            operation();
            RefreshRows();
            refreshDashboard();
        }
        catch (EnergyUsageValidationException exception)
        {
            MessageBox.Show(exception.Message, "Check usage values", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        catch (IOException exception)
        {
            MessageBox.Show($"The usage file could not be saved.\n\n{exception.Message}", "Save failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

internal sealed class UsageEditor : Window
{
    private readonly DatePicker monthPicker = new() { SelectedDate = DateTime.Today };
    private readonly Dictionary<string, TextBox> fields = new(StringComparer.Ordinal);
    public EnergyUsageRecord? Record { get; private set; }

    public UsageEditor(EnergyUsageRecord? initial)
    {
        Title = initial is null ? "Add energy usage" : "Edit energy usage";
        Width = 440; SizeToContent = SizeToContent.Height; ResizeMode = ResizeMode.NoResize; WindowStartupLocation = WindowStartupLocation.CenterOwner;
        Grid form = new() { Margin = new Thickness(24) };
        form.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
        form.ColumnDefinitions.Add(new ColumnDefinition());
        form.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        AddField(form, "Month", monthPicker, 0);
        string[] labels = ["Generation (kWh)", "Normal Return (kWh)", "Low Return (kWh)", "Normal (kWh)", "Low (kWh)", "Gas (m³)"];
        int[] values = initial is null ? [0, 0, 0, 0, 0, 0] : [initial.SolarGeneration, initial.ReturnElectricityHigh, initial.ReturnElectricityLow, initial.ElectricityHigh, initial.ElectricityLow, initial.Gas];
        for (int i = 0; i < labels.Length; i++)
        {
            TextBox box = new() { Text = values[i].ToString(CultureInfo.InvariantCulture), MaxLength = 10 };
            fields.Add(labels[i], box);
            AddField(form, labels[i], box, i + 1);
        }
        form.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        form.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        TextBlock derived = new() { Text = "Directly Used is calculated from generation minus returns.", Foreground = System.Windows.Media.Brushes.DimGray, Margin = new Thickness(0, 10, 0, 14) };
        Grid.SetRow(derived, 7); Grid.SetColumnSpan(derived, 2); form.Children.Add(derived);
        StackPanel buttons = new() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
        Button cancel = new() { Content = "Cancel", IsCancel = true, MinWidth = 80 };
        Button save = new() { Content = "Save", IsDefault = true, MinWidth = 80 };
        save.Click += Save_Click;
        buttons.Children.Add(cancel); buttons.Children.Add(save);
        Grid.SetRow(buttons, 8); Grid.SetColumnSpan(buttons, 2); form.Children.Add(buttons);
        Content = form;
        if (initial is not null)
        {
            monthPicker.SelectedDate = initial.Month;
        }
    }

    private static void AddField(Grid grid, string label, Control control, int row)
    {
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        TextBlock caption = new() { Text = label, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 12, 5) };
        control.Margin = new Thickness(0, 4, 0, 4);
        Grid.SetRow(caption, row); Grid.SetColumn(caption, 0); grid.Children.Add(caption);
        Grid.SetRow(control, row); Grid.SetColumn(control, 1); grid.Children.Add(control);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (monthPicker.SelectedDate is not { } month) { ShowError("Choose a month."); return; }
        int[] values = new int[fields.Count];
        int index = 0;
        foreach (TextBox field in fields.Values)
        {
            if (!int.TryParse(field.Text, NumberStyles.None, CultureInfo.InvariantCulture, out int value)) 
            { 
                ShowError("Enter whole numbers using digits only."); 
                return; 
            }
            if (value < 0)
            { 
                ShowError("Values cannot be negative."); 
                return; 
            }
            values[index++] = value;
        }
        Record = new EnergyUsageRecord { Month = month, SolarGeneration = values[0], ReturnElectricityHigh = values[1], ReturnElectricityLow = values[2], ElectricityHigh = values[3], ElectricityLow = values[4], Gas = values[5] };
        try 
        { 
            _ = EnergyUsageRules.NormalizeAndValidate(Record); 
        }
        catch (EnergyUsageValidationException exception) 
        { 
            ShowError(exception.Message); 
            return; 
        }
        DialogResult = true;
    }

    private static void ShowError(string message) => MessageBox.Show(message, "Check usage values", MessageBoxButton.OK, MessageBoxImage.Warning);
}
