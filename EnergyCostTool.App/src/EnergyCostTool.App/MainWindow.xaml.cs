using System.Globalization;
using System.Windows;
using EnergyCostTool.Core;

namespace EnergyCostTool.App;

public partial class MainWindow : Window
{
    private readonly EnergyUsageService service;
    private UsageWindow? usageWindow;

    public MainWindow(EnergyUsageService service)
    {
        this.service = service;
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
