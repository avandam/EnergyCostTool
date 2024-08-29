using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnergyCostTool.Dal;
using EnergyCostTool.Models;

namespace EnergyCostTool;

/// <summary>
/// Interaction logic for AddEnergyPrice.xaml
/// </summary>
public partial class EnergyPriceWindow : Window, INotifyPropertyChanged
{
    private readonly EnergyMonthCollection energyTariffs;
    public ObservableCollection<EnergyMonth> EnergyTariffs { get; protected set; }
    public EnergyPriceWindow()
    {
        InitializeComponent();
        this.energyTariffs = Database.GetEnergyTariffs();
        DisableChanges();
        ToggleCapFields(false);
        InitializeUi();
    }

    private void InitializeUi()
    {
        ClearTextBoxes();
        EnergyTariffs = new ObservableCollection<EnergyMonth>(energyTariffs.Get().OrderByDescending(price => price.Month.Year).ThenByDescending(price => price.Month.Month));
        RaisePropertyChanged("EnergyTariffs");
    }

    private void ClearTextBoxes()
    {
        TxtNorm.Text = string.Empty;
        TxtNormT.Text = string.Empty;
        TxtLow.Text = string.Empty;
        TxtLowT.Text = string.Empty;
        TxtGas.Text = string.Empty;
        TxtGasCap.Text = string.Empty;
        TxtElectricityCap.Text = string.Empty;

    }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    private void RaisePropertyChanged(string propName)
    {
        PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }

    private void PreviewTextInputDecimal(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9\\.,]");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void ChkCapActive_OnChecked(object sender, RoutedEventArgs e)
    {
        ToggleCapFields(true);
    }

    private void ChkCapActive_OnUnchecked(object sender, RoutedEventArgs e)
    {
        ToggleCapFields(false);
    }

    private void ToggleCapFields(bool isEnabled)
    {
        TxtElectricityCap.IsEnabled = isEnabled;
        TxtGasCap.IsEnabled = isEnabled;
    }

    private void TxtPrice_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        textBox.Text = textBox.Text.Replace('.', ',');
        textBox.CaretIndex = textBox.Text.Length;
    }
        
    private void TxtDate_OnTextChanged(object sender, TextChangedEventArgs e)
    {

        try
        {
            int year = Convert.ToInt32(TxtDate.Text.Substring(0, 4));
            int month = Convert.ToInt32(TxtDate.Text.Substring(5));
            if (year < 2000 || year > 2100)
            {
                DisableChanges();
            }

            if (month < 1 || month > 12)
            {
                DisableChanges();
            }

            if (energyTariffs.ContainsDataFor(new DateTime(year, month, 1)))
            {
                EnableUpdateDelete();
            }
            else
            {
                EnableAdd();
            }
        }
        catch (Exception)
        {
            DisableChanges();
        }
    }

    private void DisableChanges()
    {
        if (BtnAddPrice != null) BtnAddPrice.IsEnabled = false;
        if (BtnUpdatePrice != null) BtnUpdatePrice.IsEnabled = false;
        if (BtnDeletePrice != null) BtnDeletePrice.IsEnabled = false;
    }

    private void EnableAdd()
    {
        BtnAddPrice.IsEnabled = true;
        BtnUpdatePrice.IsEnabled = false;
        BtnDeletePrice.IsEnabled = false;
    }

    private void EnableUpdateDelete()
    {
        BtnAddPrice.IsEnabled = false;
        BtnUpdatePrice.IsEnabled = true;
        BtnDeletePrice.IsEnabled = true;
    }

    private void BtnAddPrice_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            int year = Convert.ToInt32(TxtDate.Text.Substring(0, 4));
            int month = Convert.ToInt32(TxtDate.Text.Substring(5));
            double norm = Convert.ToDouble(TxtNorm.Text);
            double normT = Convert.ToDouble(TxtNormT.Text);
            double low = Convert.ToDouble(TxtLow.Text);
            double lowT = Convert.ToDouble(TxtLowT.Text);
            double gas = Convert.ToDouble(TxtGas.Text);
            double electricityCap = ChkCapActive.IsChecked != null && ChkCapActive.IsChecked.Value ? Convert.ToDouble(TxtElectricityCap.Text) : 1000;
            double gasCap = ChkCapActive.IsChecked != null && ChkCapActive.IsChecked.Value ? Convert.ToDouble(TxtGasCap.Text) : 1000;
            Tariff tariff = new Tariff(norm, normT, low, lowT, gas, electricityCap, gasCap);
            energyTariffs.AddOrUpdateEnergyMonth(new DateTime(year, month, 1), tariff);
            Database.SaveTariffs(energyTariffs);
            InitializeUi();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Could not add this consumption.\nError message: {exception.Message}");
        }
    }


    private void BtnUpdatePrice_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            int year = Convert.ToInt32(TxtDate.Text.Substring(0, 4));
            int month = Convert.ToInt32(TxtDate.Text.Substring(5));
            double norm = Convert.ToDouble(TxtNorm.Text);
            double normT = Convert.ToDouble(TxtNormT.Text);
            double low = Convert.ToDouble(TxtLow.Text);
            double lowT = Convert.ToDouble(TxtLowT.Text);
            double gas = Convert.ToDouble(TxtGas.Text);
            double electricityCap = ChkCapActive.IsChecked != null && ChkCapActive.IsChecked.Value ? Convert.ToDouble(TxtElectricityCap.Text) : 1000;
            double gasCap = ChkCapActive.IsChecked != null && ChkCapActive.IsChecked.Value ? Convert.ToDouble(TxtGasCap.Text) : 1000;
            Tariff tariff = new Tariff(norm, normT, low, lowT, gas, electricityCap, gasCap);
            energyTariffs.AddOrUpdateEnergyMonth(new DateTime(year, month, 1), tariff);
            Database.SaveTariffs(energyTariffs);
            InitializeUi();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Could not update this consumption.\nError message: {exception.Message}");
        }
    }

    private void BtnDeletePrice_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            int year = Convert.ToInt32(TxtDate.Text.Substring(0, 4));
            int month = Convert.ToInt32(TxtDate.Text.Substring(5));
            energyTariffs.Get(new DateTime(year, month, 1)).DeleteTariff();
            Database.SaveConsumptions(energyTariffs);
            InitializeUi();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Could not delete this consumption.\nError message: {exception.Message}");
        }
    }

    private void LvPrices_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LvPrices.SelectedItem is EnergyMonth selectedTariff)
        {
            TxtDate.Text = selectedTariff.Month.Year + "-" + selectedTariff.Month.Month; ;
            TxtNorm.Text = selectedTariff.Tariff.ElectricityHigh.ToString(CultureInfo.InvariantCulture);
            TxtNormT.Text = selectedTariff.Tariff.ReturnElectricityHigh.ToString(CultureInfo.InvariantCulture);
            TxtLow.Text = selectedTariff.Tariff.ElectricityLow.ToString(CultureInfo.InvariantCulture);
            TxtLowT.Text = selectedTariff.Tariff.ReturnElectricityLow.ToString(CultureInfo.InvariantCulture);
            TxtGas.Text = selectedTariff.Tariff.Gas.ToString(CultureInfo.InvariantCulture);
            ChkCapActive.IsChecked = Math.Abs(selectedTariff.Tariff.ElectricityCap - 1000) > 1;
            ToggleCapFields(Math.Abs(selectedTariff.Tariff.ElectricityCap - 1000) > 1);
            TxtElectricityCap.Text = selectedTariff.Tariff.ElectricityCap.ToString(CultureInfo.InvariantCulture);
            TxtGasCap.Text = selectedTariff.Tariff.GasCap.ToString(CultureInfo.InvariantCulture);
        }
    }
}