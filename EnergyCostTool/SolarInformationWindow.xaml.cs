using System.Windows;
using EnergyCostTool.Logic;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool;

/// <summary>
/// Interaction logic for AddFixedCost.xaml
/// </summary>
public partial class SolarInformationWindow : Window
{
    private readonly EnergyViewModel energyViewModel;
    
    public SolarInformationWindow(EnergyViewModel energyViewModel)
    {
        InitializeComponent();
        this.energyViewModel = energyViewModel;
        ShowInformation();
    }

    private void ShowInformation()
    {
        SolarInformationViewModel solarInformation = SolarInformationCalculator.ComputeSolarInformation(energyViewModel);
        LblGenerated.Content = solarInformation.SolarGenerated;
        LblDirectlyUsed.Content = solarInformation.SolarDirectlyUsed;
        LblDirectlyUsedPrice.Content = "\u20AC " + Math.Round(solarInformation.SolarDirectlyUsedPrice, 2, MidpointRounding.AwayFromZero);
        LblReturnedNorm.Content = solarInformation.SolarReturnedNorm;
        LblReturnedNormPrice.Content = "\u20AC " + Math.Round(solarInformation.SolarReturnedNormPrice, 2, MidpointRounding.AwayFromZero);
        LblReturnedLow.Content = solarInformation.SolarReturnedLow;
        LblReturnedLowPrice.Content = "\u20AC " + Math.Round(solarInformation.SolarReturnedLowPrice, 2, MidpointRounding.AwayFromZero);
        LblSolarCostPrice.Content = "\u20AC " + Math.Round(solarInformation.SolarCostPrice, 2, MidpointRounding.AwayFromZero);

        LblTotalPrice.Content = "\u20AC " + Math.Round(solarInformation.TotalPrice, 2, MidpointRounding.AwayFromZero);
        LblCostOfSolarPanels.Content = "\u20AC " + Math.Round(solarInformation.PriceOfSolarPanels, 2, MidpointRounding.AwayFromZero);
        LblDifferencePrice.Content = "\u20AC " + Math.Round(solarInformation.Difference, 2, MidpointRounding.AwayFromZero);

        LblExpectedCutoffDate.Content = $"{solarInformation.NrOfMonthsToReturnInvestment / 12} jaar, {solarInformation.NrOfMonthsToReturnInvestment % 12} maanden";
    }

}