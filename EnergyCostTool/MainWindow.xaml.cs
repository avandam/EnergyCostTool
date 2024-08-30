using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using EnergyCostTool.Dal;
using EnergyCostTool.Models;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public ObservableCollection<EnergyMonth> EnergyConsumptionForCurrentYear { get; protected set; }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    private void RaisePropertyChanged(string propName)
    {
        PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }

    public MainWindow()
    {
        InitializeComponent();
        InitializeUi();
    }

    private void InitializeUi()
    {
        EnergyConsumptionForCurrentYear =
            new ObservableCollection<EnergyMonth>(Database.GetEnergyConsumptionForYear(DateTime.Now.Year).Get());
        RaisePropertyChanged("EnergyConsumptionForCurrentYear");
    }

    private void BtnConsumption_Click(object sender, RoutedEventArgs e)
    {
        EnergyConsumptionWindow energyConsumptionWindow = new EnergyConsumptionWindow();
        energyConsumptionWindow.Closed += (_, _) => InitializeUi();
        energyConsumptionWindow.Show();
    }

    private void BtnEnergyPrices_OnClick(object sender, RoutedEventArgs e)
    {
        EnergyPriceWindow energyPriceWindow = new EnergyPriceWindow();
        energyPriceWindow.Closed += (_, _) => InitializeUi();
        energyPriceWindow.Show();
    }
        
    private void BtnFixedCosts_OnClick(object sender, RoutedEventArgs e)
    {
        FixedCostWindow fixedCostWindow = new FixedCostWindow();
        fixedCostWindow.Closed += (_, _) => InitializeUi();
        fixedCostWindow.Show();
    }

    private void BtnYearlyCost_OnClick(object sender, RoutedEventArgs e)
    {
        YearlyCostWindow yearlyCostWindow = new YearlyCostWindow();
        yearlyCostWindow.Closed += (_, _) => InitializeUi();
        yearlyCostWindow.Show();
    }

    private void BtnEnergyYearlyCost_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtnSolarPanelFinancialResults_OnClick(object sender, RoutedEventArgs e)
    {
        SolarInformationWindow solarInformationWindow = new SolarInformationWindow();
        solarInformationWindow.Closed += (_, _) => InitializeUi();
        solarInformationWindow.Show();
    }
}