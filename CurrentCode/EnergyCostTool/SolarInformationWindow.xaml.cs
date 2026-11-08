using System.Windows;
using EnergyCostTool.Dal;
using EnergyCostTool.Logic;
using EnergyCostTool.Models;
using EnergyCostTool.ViewModels;

namespace EnergyCostTool;

/// <summary>
/// Interaction logic for AddFixedCost.xaml
/// </summary>
public partial class SolarInformationWindow : Window
{
     public SolarInformationWindow()
    {
        InitializeComponent();
        EnergyMonthCollection energyMonths = Database.GetSolarEnergyMonths();
        energyMonths.InjectStandardCosts(Database.GetStandardCosts());
        SolarInformationViewModel solarInformation = SolarInformationCalculator.ComputeSolarInformation(energyMonths);
        ShowInformation(solarInformation);
    }

    private void ShowInformation(SolarInformationViewModel solarInformation)
    {
        LblGenerated.Content = solarInformation.Generation;
        LblDirectlyUsed.Content = solarInformation.DirectlyUsed;
        LblDirectlyUsedPrice.Content = "\u20AC " + Math.Round(solarInformation.DirectlyUsedPrice, 2, MidpointRounding.AwayFromZero);
        LblReturnedNorm.Content = solarInformation.ReturnedNorm;
        LblReturnedNormPrice.Content = "\u20AC " + Math.Round(solarInformation.ReturnedNormPrice, 2, MidpointRounding.AwayFromZero);
        LblReturnedLow.Content = solarInformation.ReturnedLow;
        LblReturnedLowPrice.Content = "\u20AC " + Math.Round(solarInformation.ReturnedLowPrice, 2, MidpointRounding.AwayFromZero);
        LblSolarCostPrice.Content = "\u20AC " + Math.Round(solarInformation.FixedCostPrice, 2, MidpointRounding.AwayFromZero);

        LblTotalPrice.Content = "\u20AC " + Math.Round(solarInformation.TotalPrice, 2, MidpointRounding.AwayFromZero);
        LblCostOfSolarPanels.Content = "\u20AC " + Math.Round(solarInformation.PriceOfSolarPanels, 2, MidpointRounding.AwayFromZero);
        LblDifferencePrice.Content = "\u20AC " + Math.Round(solarInformation.Difference, 2, MidpointRounding.AwayFromZero);

        LblExpectedCutoffDate.Content = $"{solarInformation.NrOfMonthsToReturnInvestment / 12} jaar, {solarInformation.NrOfMonthsToReturnInvestment % 12} maanden";
    }

}