using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnergyCostTool.Data;

namespace EnergyCostTool;

/// <summary>
/// Interaction logic for AddEnergyPrice.xaml
/// </summary>
public partial class EnergyPriceWindow : Window, INotifyPropertyChanged
{
    private readonly EnergyPriceCollection energyPriceCollection;
    public ObservableCollection<EnergyPrice> EnergyPrices { get; protected set; }
    public EnergyPriceWindow(EnergyPriceCollection energyPriceCollection)
    {
        InitializeComponent();
        this.energyPriceCollection = energyPriceCollection;
        DisableChanges();
        ToggleCapFields(false);
        InitializeUi();
    }

    private void InitializeUi()
    {
        EnergyPrices = new ObservableCollection<EnergyPrice>(energyPriceCollection.Get().OrderByDescending(price => price.StartDate));
        RaisePropertyChanged("EnergyPrices");
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
        textBox.Text.Replace('.', ',');
    }
        
    private void TxtDate_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            DateTime startDate = DateTime.ParseExact(TxtDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (startDate.Year is < 2000 or > 2100)
            {
                DisableChanges();
            }

            if (startDate.Month is < 1 or > 12)
            {
                DisableChanges();
            }

            if (energyPriceCollection.ContainsDataFor(startDate))
            {
                EnableUpdateDelete();
            }
            else
            {
                EnableAdd();
            }
        }
        catch (Exception exception)
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
            DateTime startDate = DateTime.ParseExact(TxtDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            double norm = Convert.ToDouble(TxtNorm.Text);
            double normT = Convert.ToDouble(TxtNormT.Text);
            double low = Convert.ToDouble(TxtLow.Text);
            double lowT = Convert.ToDouble(TxtLowT.Text);
            double gas = Convert.ToDouble(TxtGas.Text);
            double electricityCap = ChkCapActive.IsChecked.Value ? Convert.ToDouble(TxtElectricityCap.Text) : 1000;
            double gasCap = ChkCapActive.IsChecked.Value ? Convert.ToDouble(TxtGasCap.Text) : 1000;
            EnergyPrice energyPrice = new EnergyPrice(startDate, norm, normT, low, lowT, gas, electricityCap, gasCap);
            energyPriceCollection.Add(energyPrice);
            energyPriceCollection.Save();
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
            DateTime startDate = DateTime.ParseExact(TxtDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            double norm = Convert.ToDouble(TxtNorm.Text);
            double normT = Convert.ToDouble(TxtNormT.Text);
            double low = Convert.ToDouble(TxtLow.Text);
            double lowT = Convert.ToDouble(TxtLowT.Text);
            double gas = Convert.ToDouble(TxtGas.Text);
            double electricityCap = ChkCapActive.IsChecked.Value ? Convert.ToDouble(TxtElectricityCap.Text) : 1000;
            double gasCap = ChkCapActive.IsChecked.Value ? Convert.ToDouble(TxtGasCap.Text) : 1000;
            EnergyPrice energyPrice = new EnergyPrice(startDate, norm, normT, low, lowT, gas, electricityCap, gasCap);
            energyPriceCollection.Update(energyPrice);
            energyPriceCollection.Save();
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
            DateTime startDate = DateTime.ParseExact(TxtDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            energyPriceCollection.Delete(startDate);
            energyPriceCollection.Save();
            InitializeUi();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Could not delete this consumption.\nError message: {exception.Message}");
        }
    }

    private void LvPrices_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LvPrices.SelectedItem is EnergyPrice selectedConsumption)
        {
            TxtDate.Text = selectedConsumption.StartDate.ToString("yyyy-MM-dd");
            TxtNorm.Text = selectedConsumption.ElectricityHigh.ToString();
            TxtNormT.Text = selectedConsumption.ReturnElectricityHigh.ToString();
            TxtLow.Text = selectedConsumption.ElectricityLow.ToString();
            TxtLowT.Text = selectedConsumption.ReturnElectricityLow.ToString();
            TxtGas.Text = selectedConsumption.Gas.ToString();
            ChkCapActive.IsChecked = selectedConsumption.ElectricityCap != 1000;
            ToggleCapFields(selectedConsumption.ElectricityCap != 1000);
            TxtElectricityCap.Text = selectedConsumption.ElectricityCap.ToString();
            TxtGasCap.Text = selectedConsumption.GasCap.ToString();
        }
    }
}