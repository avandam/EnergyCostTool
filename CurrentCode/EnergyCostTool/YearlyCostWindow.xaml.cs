using System.Windows;
using System.Windows.Controls;
using EnergyCostTool.Dal;
using EnergyCostTool.Logic;
using EnergyCostTool.Models;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool;

/// <summary>
/// Interaction logic for AddFixedCost.xaml
/// </summary>
public partial class YearlyCostWindow : Window
{
    private YearlyCostViewModel yearlyCostViewModel;
    
    public YearlyCostWindow()
    {
        InitializeComponent();
        InitializeUi();
    }

    private void InitializeUi()
    {
        List<int> years = Database.GetEnergyYears();
        CmbYear.ItemsSource = years;
    }

    private void CmbYear_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            int selectedYear = (int)((ComboBox)sender).SelectedItem;
            EnergyMonthCollection energyMonths = Database.GetEnergyConsumptionForYear(selectedYear);
            energyMonths.InjectStandardCosts(Database.GetStandardCosts());
            yearlyCostViewModel = YearlyCostCalculator.GetYearlyCostForYear(selectedYear, energyMonths);

            LblNormUsed.Content = yearlyCostViewModel.NormUsed.ToString();
            LblNormReturned.Content = yearlyCostViewModel.NormReturned.ToString();
            LblNormPrice.Content = "\u20AC " + Math.Round(yearlyCostViewModel.NormPrice, 2, MidpointRounding.AwayFromZero);

            LblLowUsed.Content = yearlyCostViewModel.LowUsed.ToString();
            LblLowReturned.Content = yearlyCostViewModel.LowReturned.ToString();
            LblLowPrice.Content = "\u20AC " + Math.Round(yearlyCostViewModel.LowPrice, 2, MidpointRounding.AwayFromZero);
            
            LblSolarGenerated.Content = yearlyCostViewModel.SolarGenerated.ToString();

            LblStandingChargeElectricity.Content = "\u20AC " + Math.Round(yearlyCostViewModel.StandingChargesElectricity, 2, MidpointRounding.AwayFromZero);
            LblTransportCostElectricity.Content = "\u20AC " + Math.Round(yearlyCostViewModel.TransportCostElectricity, 2, MidpointRounding.AwayFromZero);
            LblSolarCost.Content = "\u20AC " + Math.Round(yearlyCostViewModel.SolarCost, 2, MidpointRounding.AwayFromZero);
            LblDiscountOnEnergyTax.Content = "\u20AC " + Math.Round(yearlyCostViewModel.DiscountOnEnergyTax, 2, MidpointRounding.AwayFromZero);

            LblGasUsed.Content = yearlyCostViewModel.GasUsed.ToString();
            LblGasPrice.Content = "\u20AC " + Math.Round(yearlyCostViewModel.GasPrice, 2, MidpointRounding.AwayFromZero);

            LblStandingChargeGas.Content = "\u20AC " + Math.Round(yearlyCostViewModel.StandingChargesGas, 2, MidpointRounding.AwayFromZero);
            LblTransportCostGas.Content = "\u20AC " + Math.Round(yearlyCostViewModel.TransportCostGas, 2, MidpointRounding.AwayFromZero);

            LblTotalElectricity.Content = "\u20AC " + Math.Round(yearlyCostViewModel.TotalElectricityPrice, 2, MidpointRounding.AwayFromZero);
            LblTotalGas.Content = "\u20AC " + Math.Round(yearlyCostViewModel.TotalGasPrice, 2, MidpointRounding.AwayFromZero);
            LblTotal.Content = "\u20AC " + Math.Round(yearlyCostViewModel.TotalPrice, 2, MidpointRounding.AwayFromZero);
            LblPayedDeposits.Content = "\u20AC " + Math.Round(yearlyCostViewModel.PayedDeposits, 2, MidpointRounding.AwayFromZero);
            LblDifference.Content = "\u20AC " + Math.Round(yearlyCostViewModel.TotalPrice - yearlyCostViewModel.PayedDeposits, 2, MidpointRounding.AwayFromZero);

        }
        catch (Exception exception)
        {
            MessageBox.Show($"Could not show costs.\nError message: {exception.Message}");
        }
    }
}