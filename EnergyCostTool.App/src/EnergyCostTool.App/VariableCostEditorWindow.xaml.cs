using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using EnergyCostTool.Core;

namespace EnergyCostTool.App;

public partial class VariableCostEditorWindow : Window
{
    public VariableCostEntry? EntryValue { get; private set; }

    public VariableCostEditorWindow(VariableCostEntry? initial)
    {
        InitializeComponent();
        MonthComboBox.ItemsSource = DateTimeFormatInfo.CurrentInfo!.MonthNames.Take(12).ToArray();
        DateTime month = initial?.StartDate ?? DateTime.Today;
        MonthComboBox.SelectedIndex = month.Month - 1;
        YearTextBox.Text = month.Year.ToString(CultureInfo.InvariantCulture);

        if (initial is null)
        {
            Title = "Add variable tariff";
        }
        else
        {
            Title = "Edit variable tariff";
            MonthComboBox.IsEnabled = false;
            YearTextBox.IsEnabled = false;
            ElectricityHighTextBox.Text = Format(initial.ElectricityHigh);
            ElectricityLowTextBox.Text = Format(initial.ElectricityLow);
            ReturnElectricityHighCostTextBox.Text = Format(initial.ReturnElectricityHighCost);
            ReturnElectricityHighCompensationTextBox.Text = Format(initial.ReturnElectricityHighCompensation);
            ReturnElectricityLowCostTextBox.Text = Format(initial.ReturnElectricityLowCost);
            ReturnElectricityLowCompensationTextBox.Text = Format(initial.ReturnElectricityLowCompensation);
            GasTextBox.Text = Format(initial.Gas);
        }
    }

    private static string Format(decimal value) => value.ToString(CultureInfo.CurrentCulture);

    private bool TryGetSelectedMonth(out DateTime month)
    {
        month = default;
        if (MonthComboBox.SelectedIndex is < 0 or > 11 ||
            !int.TryParse(YearTextBox.Text, NumberStyles.None, CultureInfo.InvariantCulture, out int year) ||
            year is < 1 or > 9999)
        {
            return false;
        }

        month = new DateTime(year, MonthComboBox.SelectedIndex + 1, 1);
        return true;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedMonth(out DateTime month))
        {
            ShowValidationError("Select a month and enter a valid year.");
            return;
        }

        if (!TryReadPrice(ElectricityHighTextBox, "Norm", out decimal electricityHigh) ||
            !TryReadPrice(ElectricityLowTextBox, "Dal", out decimal electricityLow) ||
            !TryReadPrice(ReturnElectricityHighCostTextBox, "Norm T kosten", out decimal returnElectricityHighCost) ||
            !TryReadPrice(ReturnElectricityHighCompensationTextBox, "Norm T vergoeding", out decimal returnElectricityHighCompensation) ||
            !TryReadPrice(ReturnElectricityLowCostTextBox, "Dal T kosten", out decimal returnElectricityLowCost) ||
            !TryReadPrice(ReturnElectricityLowCompensationTextBox, "Dal T vergoeding", out decimal returnElectricityLowCompensation) ||
            !TryReadPrice(GasTextBox, "Gas", out decimal gas))
        {
            return;
        }

        EntryValue = new VariableCostEntry(
            month,
            electricityHigh,
            electricityLow,
            returnElectricityHighCost,
            returnElectricityHighCompensation,
            returnElectricityLowCost,
            returnElectricityLowCompensation,
            gas);
        DialogResult = true;
    }

    private static bool TryReadPrice(TextBox textBox, string label, out decimal value)
    {
        if (decimal.TryParse(textBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out value))
        {
            return true;
        }

        ShowValidationError($"Enter a valid euro amount for {label} using your regional number format.");
        return false;
    }

    private static void ShowValidationError(string message) =>
        MessageBox.Show(message, "Check variable tariff", MessageBoxButton.OK, MessageBoxImage.Warning);
}
