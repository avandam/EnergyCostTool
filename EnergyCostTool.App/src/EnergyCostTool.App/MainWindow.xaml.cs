using System.Globalization;
using System.IO;
using System.Windows;
using EnergyCostTool.Core;

namespace EnergyCostTool.App;

public partial class MainWindow : Window
{
    private readonly EnergyUsageService service;
    private readonly string fixedCostsFilePath;
    private readonly string fixedCostTariffTypesFilePath;
    private UsageWindow? usageWindow;
    private FixedCostWindow? fixedCostWindow;

    public MainWindow(EnergyUsageService service, string fixedCostsFilePath, string fixedCostTariffTypesFilePath)
    {
        this.service = service;
        this.fixedCostsFilePath = fixedCostsFilePath;
        this.fixedCostTariffTypesFilePath = fixedCostTariffTypesFilePath;
        InitializeComponent();
        YearLabel.Text = DateTime.Today.Year.ToString(CultureInfo.CurrentCulture);
        RefreshUsage();
    }

    public void RefreshUsage()
    {
        UsageGrid.ItemsSource = service.GetForYear(DateTime.Today.Year)
            .Select(record => new UsageRow(record)).ToArray();
        EmptyLabel.Visibility = UsageGrid.Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    private void OpenUsage_Click(object sender, RoutedEventArgs e)
    {
        if (usageWindow is { IsLoaded: true })
        {
            usageWindow.Activate();
            return;
        }

        usageWindow = new UsageWindow(service, RefreshUsage) { Owner = this };
        usageWindow.Closed += (_, _) => usageWindow = null;
        usageWindow.Show();
    }

    private void Placeholder_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.Button button)
        {
            MessageBox.Show($"{button.Content} will be available in a future version.", "Not implemented yet", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void OpenFixedCosts_Click(object sender, RoutedEventArgs e)
    {
        if (fixedCostWindow is { IsLoaded: true })
        {
            fixedCostWindow.Activate();
            return;
        }

        try
        {
            FixedCostService fixedCostService = new(
                new FixedCostRepository(fixedCostsFilePath),
                new FixedCostTariffTypeRepository(fixedCostTariffTypesFilePath));
            fixedCostWindow = new FixedCostWindow(fixedCostService) { Owner = this };
            fixedCostWindow.Closed += (_, _) => fixedCostWindow = null;
            fixedCostWindow.Show();
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException or System.Text.Json.JsonException)
        {
            MessageBox.Show($"Fixed-cost data could not be loaded.\n\n{exception.Message}", "Fixed costs", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

public sealed class UsageRow(EnergyUsageRecord record)
{
    public DateTime Month => record.Month;
    public int SolarGeneration => record.SolarGeneration;
    public int ReturnElectricityHigh => record.ReturnElectricityHigh;
    public int ReturnElectricityLow => record.ReturnElectricityLow;
    public long DirectlyUsed => record.DirectlyUsed;
    public int ElectricityHigh => record.ElectricityHigh;
    public int ElectricityLow => record.ElectricityLow;
    public int Gas => record.Gas;
    public bool HasNegativeDirectlyUsed => DirectlyUsed < 0;
    public EnergyUsageRecord ToRecord() => new()
    {
        Month = Month,
        SolarGeneration = SolarGeneration,
        ReturnElectricityHigh = ReturnElectricityHigh,
        ReturnElectricityLow = ReturnElectricityLow,
        ElectricityHigh = ElectricityHigh,
        ElectricityLow = ElectricityLow,
        Gas = Gas
    };
}
