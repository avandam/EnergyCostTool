using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnergyCostTool.Data;
using EnergyCostTool.Logic;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool;

/// <summary>
/// Interaction logic for AddFixedCost.xaml
/// </summary>
public partial class YearlyCostWindow : Window, INotifyPropertyChanged
{
    private readonly EnergyViewModel energyViewModel;
    private YearlyCostViewModel yearlyCostViewModel;
    
    public YearlyCostWindow(EnergyViewModel energyViewModel)
    {
        InitializeComponent();
        this.energyViewModel = energyViewModel;
        InitializeUi();
    }

    private void InitializeUi()
    {
        List<int> years = energyViewModel.EnergyConsumptionCollection.Get().Select(consumption => consumption.Month.Year).Distinct().ToList();
        CmbYear.ItemsSource = years;
    }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    private void RaisePropertyChanged(string propName)
    {
        PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }

    private void CmbYear_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            int selectedYear = (int)((ComboBox)sender).SelectedItem;
            yearlyCostViewModel = YearlyCostCalculator.GetYearlyCostForYear(selectedYear, energyViewModel);
            LblNormUsed.Content = yearlyCostViewModel.NormUsed.ToString();
            LblNormReturned.Content = yearlyCostViewModel.NormReturned.ToString();
            LblNormPrice.Content = "\u20AC " + Math.Round(yearlyCostViewModel.NormPrice, 2, MidpointRounding.AwayFromZero);

            LblLowUsed.Content = yearlyCostViewModel.LowUsed.ToString();
            LblLowReturned.Content = yearlyCostViewModel.LowReturned.ToString();
            LblLowPrice.Content = "\u20AC " + Math.Round(yearlyCostViewModel.LowPrice, 2, MidpointRounding.AwayFromZero);


            LblSolarGenerated.Content = yearlyCostViewModel.SolarGenerated.ToString();


            LblGasUsed.Content = yearlyCostViewModel.GasUsed.ToString();
            LblGasPrice.Content = "\u20AC " + Math.Round(yearlyCostViewModel.GasPrice, 2, MidpointRounding.AwayFromZero);

            LblTotalElectricity.Content = "\u20AC " + Math.Round(yearlyCostViewModel.TotalElectricityPrice, 2, MidpointRounding.AwayFromZero);
            LblTotalGas.Content = "\u20AC " + Math.Round(yearlyCostViewModel.TotalGasPrice, 2, MidpointRounding.AwayFromZero);
            LblTotal.Content = "\u20AC " + Math.Round(yearlyCostViewModel.TotalPrice, 2, MidpointRounding.AwayFromZero);
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Could not show costs.\nError message: {exception.Message}");
        }
    }
}