using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using EnergyCostTool.Data;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly EnergyViewModel viewModel;
    public ObservableCollection<EnergyConsumption> EnergyConsumptionForCurrentYear { get; protected set; }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    private void RaisePropertyChanged(string propName)
    {
        PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }

    public MainWindow()
    {
        InitializeComponent();
        viewModel = new EnergyViewModel();
        InitializeUi();
    }

    private void InitializeUi()
    {
        EnergyConsumptionForCurrentYear =
            new ObservableCollection<EnergyConsumption>(viewModel.EnergyConsumptionCollection.GetForYear(2023));
        RaisePropertyChanged("EnergyConsumptionForCurrentYear");
    }

    private void BtnConsumption_Click(object sender, RoutedEventArgs e)
    {
        EnergyConsumptionWindow energyConsumptionWindow = new EnergyConsumptionWindow(viewModel.EnergyConsumptionCollection);
        energyConsumptionWindow.Closed += (_, _) => InitializeUi();
        energyConsumptionWindow.Show();
    }

    private void BtnEnergyPrices_OnClick(object sender, RoutedEventArgs e)
    {
        EnergyPriceWindow energyPriceWindow = new EnergyPriceWindow(viewModel.EnergyPriceCollection);
        energyPriceWindow.Closed += (_, _) => InitializeUi();
        energyPriceWindow.Show();
    }
        
    private void BtnFixedCosts_OnClick(object sender, RoutedEventArgs e)
    {
        FixedCostWindow fixedCostWindow = new FixedCostWindow(viewModel.FixedCostCollection);
        fixedCostWindow.Closed += (_, _) => InitializeUi();
        fixedCostWindow.Show();
    }

    private void BtnYearlyCost_OnClick(object sender, RoutedEventArgs e)
    {
        YearlyCostWindow yearlyCostWindow = new YearlyCostWindow(viewModel);
        yearlyCostWindow.Closed += (_, _) => InitializeUi();
        yearlyCostWindow.Show();
    }

    private void BtnEnergyYearlyCost_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtnSolarPanelFinancialResults_OnClick(object sender, RoutedEventArgs e)
    {
        SolarInformationWindow solarInformationWindow = new SolarInformationWindow(viewModel);
        solarInformationWindow.Closed += (_, _) => InitializeUi();
        solarInformationWindow.Show();
    }
}