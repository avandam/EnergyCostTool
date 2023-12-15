using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnergyCostTool.Data;

namespace EnergyCostTool;

/// <summary>
/// Interaction logic for AddEnergyConsumption.xaml
/// </summary>
public partial class EnergyConsumptionWindow : Window, INotifyPropertyChanged
{
    private readonly EnergyConsumptionCollection energyConsumptionCollection;
    public ObservableCollection<EnergyConsumption> EnergyConsumptions { get; protected set; }
    public EnergyConsumptionWindow(EnergyConsumptionCollection energyConsumptionCollection)
    {
        InitializeComponent();
        this.energyConsumptionCollection = energyConsumptionCollection;
        DisableChanges();
        InitializeUi();
    }

    private void InitializeUi()
    {
        ClearTextBoxes();
        EnergyConsumptions = new ObservableCollection<EnergyConsumption>(energyConsumptionCollection.Get().OrderByDescending(consumption => consumption.Month.Year).ThenByDescending(consumption => consumption.Month.Month));
        RaisePropertyChanged("EnergyConsumptions");
    }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    private void RaisePropertyChanged(string propName)
    {
        PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }

    private void PreviewTextInputInteger(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
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

            if (energyConsumptionCollection.ContainsDataFor(new DateTime(year, month, 1)))
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
        if (BtnAddConsumption != null) BtnAddConsumption.IsEnabled = false;
        if (BtnUpdateConsumption != null) BtnUpdateConsumption.IsEnabled = false;
        if (BtnDeleteConsumption != null) BtnDeleteConsumption.IsEnabled = false;
    }

    private void EnableAdd()
    {
        BtnAddConsumption.IsEnabled = true;
        BtnUpdateConsumption.IsEnabled = false;
        BtnDeleteConsumption.IsEnabled = false;
    }

    private void EnableUpdateDelete()
    {
        BtnAddConsumption.IsEnabled = false;
        BtnUpdateConsumption.IsEnabled = true;
        BtnDeleteConsumption.IsEnabled = true;

    }

    private void BtnAddConsumption_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            int year = Convert.ToInt32(TxtDate.Text.Substring(0, 4));
            int month = Convert.ToInt32(TxtDate.Text.Substring(5));
            int solar = Convert.ToInt32(TxtSolar.Text);
            int norm = Convert.ToInt32(TxtNorm.Text);
            int normT = Convert.ToInt32(TxtNormT.Text);
            int low = Convert.ToInt32(TxtLow.Text);
            int lowT = Convert.ToInt32(TxtLowT.Text);
            int gas = Convert.ToInt32(TxtGas.Text);
            EnergyConsumption energyConsumption = new EnergyConsumption(new DateTime(year, month, 1), solar, norm, normT, low, lowT, gas);
            energyConsumptionCollection.Add(energyConsumption);
            energyConsumptionCollection.Save();
            InitializeUi();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Could not add this consumption.\nError message: {exception.Message}");
        }
    }


    private void BtnUpdateConsumption_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            int year = Convert.ToInt32(TxtDate.Text.Substring(0, 4));
            int month = Convert.ToInt32(TxtDate.Text.Substring(5));
            int solar = Convert.ToInt32(TxtSolar.Text);
            int norm = Convert.ToInt32(TxtNorm.Text);
            int normT = Convert.ToInt32(TxtNormT.Text);
            int low = Convert.ToInt32(TxtLow.Text);
            int lowT = Convert.ToInt32(TxtLowT.Text);
            int gas = Convert.ToInt32(TxtGas.Text);
            EnergyConsumption energyConsumption = new EnergyConsumption(new DateTime(year, month, 1), solar, norm, normT, low, lowT, gas);
            energyConsumptionCollection.Update(energyConsumption);
            energyConsumptionCollection.Save();
            InitializeUi();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Could not update this consumption.\nError message: {exception.Message}");
        }
    }

    private void BtnDeleteConsumption_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            int year = Convert.ToInt32(TxtDate.Text.Substring(0, 4));
            int month = Convert.ToInt32(TxtDate.Text.Substring(5));
            energyConsumptionCollection.Delete(new DateTime(year, month, 1));
            energyConsumptionCollection.Save();
            InitializeUi();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Could not delete this consumption.\nError message: {exception.Message}");
        }
    }

    private void LvConsumption_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LvConsumption.SelectedItem is EnergyConsumption selectedConsumption)
        {
            TxtDate.Text = selectedConsumption.Month.Year + "-" + selectedConsumption.Month.Month;
            TxtSolar.Text = selectedConsumption.SolarGeneration.ToString();
            TxtNorm.Text = selectedConsumption.ElectricityHigh.ToString();
            TxtNormT.Text = selectedConsumption.ReturnElectricityHigh.ToString();
            TxtLow.Text = selectedConsumption.ElectricityLow.ToString();
            TxtLowT.Text = selectedConsumption.ReturnElectricityLow.ToString();
            TxtGas.Text = selectedConsumption.Gas.ToString();
        }
    }

    private void ClearTextBoxes()
    {
        TxtSolar.Text = string.Empty;
        TxtNorm.Text = string.Empty;
        TxtNormT.Text = string.Empty;
        TxtLow.Text = string.Empty;
        TxtLowT.Text = string.Empty;
        TxtGas.Text = string.Empty;

    }
}