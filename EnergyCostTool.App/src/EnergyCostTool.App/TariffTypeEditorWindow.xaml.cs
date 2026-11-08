using System.Windows;
using EnergyCostTool.Core;

namespace EnergyCostTool.App;

public partial class TariffTypeEditorWindow : Window
{
    public string? NameValue => NameTextBox.Text;
    public FixedCostTimeframe? TimeframeValue => TimeframeComboBox.SelectedItem is FixedCostTimeframe timeframe ? timeframe : null;
    public FixedCostEnergyType? EnergyTypeValue => EnergyTypeComboBox.SelectedItem is FixedCostEnergyType energyType ? energyType : null;

    public TariffTypeEditorWindow(FixedCostTariffType? initial)
    {
        InitializeComponent();
        TimeframeComboBox.ItemsSource = Enum.GetValues<FixedCostTimeframe>();
        EnergyTypeComboBox.ItemsSource = Enum.GetValues<FixedCostEnergyType>();
        if (initial is null)
        {
            Title = "Add tariff type";
            IdTextBox.Text = "Generated when saved";
            TimeframeComboBox.SelectedItem = FixedCostTimeframe.Monthly;
            EnergyTypeComboBox.SelectedItem = FixedCostEnergyType.Energy;
        }
        else
        {
            Title = "Edit tariff type";
            IdTextBox.Text = initial.Id.ToString();
            NameTextBox.Text = initial.Name;
            TimeframeComboBox.SelectedItem = initial.Timeframe;
            EnergyTypeComboBox.SelectedItem = initial.EnergyType;
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            MessageBox.Show("Enter a name for the tariff type.", "Check tariff type", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (TimeframeValue is null || EnergyTypeValue is null)
        {
            MessageBox.Show("Select a timeframe and energy type.", "Check tariff type", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
    }
}
