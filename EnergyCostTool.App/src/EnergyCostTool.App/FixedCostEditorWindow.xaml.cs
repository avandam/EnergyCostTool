using System.Globalization;
using System.Windows;
using EnergyCostTool.Core;

namespace EnergyCostTool.App;

public partial class FixedCostEditorWindow : Window
{
    public DateTime? StartDateValue => StartDatePicker.SelectedDate;
    public Guid? TariffTypeIdValue => TariffTypeComboBox.SelectedValue is Guid id ? id : null;
    public decimal? PriceValue { get; private set; }

    public FixedCostEditorWindow(IReadOnlyList<FixedCostTariffType> tariffTypes, FixedCostView? initial)
    {
        ArgumentNullException.ThrowIfNull(tariffTypes);
        InitializeComponent();
        TariffTypeComboBox.ItemsSource = tariffTypes;
        if (initial is null)
        {
            Title = "Add fixed cost";
            StartDatePicker.SelectedDate = DateTime.Today;
            TariffTypeComboBox.SelectedIndex = 0;
        }
        else
        {
            Title = "Edit fixed cost";
            StartDatePicker.SelectedDate = initial.StartDate;
            StartDatePicker.IsEnabled = false;
            TariffTypeComboBox.SelectedValue = initial.TariffTypeId;
            PriceTextBox.Text = initial.Price.ToString(CultureInfo.CurrentCulture);
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (StartDateValue is null)
        {
            ShowValidationError("Select a start date.");
            return;
        }

        if (TariffTypeIdValue is null)
        {
            ShowValidationError("Select a tariff type.");
            return;
        }

        if (!decimal.TryParse(PriceTextBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal price))
        {
            ShowValidationError("Enter a valid euro amount using your regional number format.");
            return;
        }

        PriceValue = price;
        DialogResult = true;
    }

    private static void ShowValidationError(string message) => MessageBox.Show(message, "Check fixed-cost entry", MessageBoxButton.OK, MessageBoxImage.Warning);
}
