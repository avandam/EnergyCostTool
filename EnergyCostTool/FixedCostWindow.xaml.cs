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
/// Interaction logic for AddFixedCost.xaml
/// </summary>
public partial class FixedCostWindow : Window, INotifyPropertyChanged
{
    private readonly FixedCostCollection fixedCostCollection;
    public ObservableCollection<FixedCost> FixedCosts { get; protected set; }
    public FixedCostWindow(FixedCostCollection fixedCostCollection)
    {
        InitializeComponent();
        this.fixedCostCollection = fixedCostCollection;
        DisableChanges();
        InitializeUi();
    }

    private void InitializeUi()
    {
        ClearTextBoxes();
        FixedCosts = new ObservableCollection<FixedCost>(fixedCostCollection.Get().OrderByDescending(price => price.StartDate));
        RaisePropertyChanged("FixedCosts");
    }

    private void ClearTextBoxes()
    {
        TxtPrice.Text = string.Empty;
    }

    public event PropertyChangedEventHandler PropertyChanged = delegate { };

    private void RaisePropertyChanged(string propName)
    {
        PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }

    public FixedCostType FixedCostType
    {
        get => fixedCostType;
        set
        {
            if (fixedCostType != value)
            {
                fixedCostType = value;
                RaisePropertyChanged("FixedCostType");
            }
        }
    }
    private FixedCostType fixedCostType;

    private void PreviewTextInputDecimal(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9-\\.,]");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void TxtPrice_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        textBox.Text = textBox.Text.Replace('.', ',');
        textBox.CaretIndex = textBox.Text.Length;
    }
        
    private void TxtDate_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateUiButtons();
    }

    private void CmbType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateUiButtons();
    }


    private void UpdateUiButtons()
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

            if (fixedCostCollection.ContainsDataFor(startDate, (FixedCostType)CmbType.SelectedValue))
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
            DateTime startDate = DateTime.ParseExact(TxtDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            FixedCostType fixedCostTypeFromInput = (FixedCostType)CmbType.SelectedValue;
            double price = Convert.ToDouble(TxtPrice.Text);
            FixedCost fixedCost = new FixedCost(startDate, fixedCostTypeFromInput, price);
            fixedCostCollection.Add(fixedCost);
            fixedCostCollection.Save();
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
            FixedCostType fixedCostTypeFromInput = (FixedCostType)CmbType.SelectedValue;
            double price = Convert.ToDouble(TxtPrice.Text);
            FixedCost fixedCost = new FixedCost(startDate, fixedCostTypeFromInput, price);
            fixedCostCollection.Update(fixedCost);
            fixedCostCollection.Save();
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
            FixedCostType fixedCostTypeFromInput = (FixedCostType)CmbType.SelectedValue;
            fixedCostCollection.Delete(startDate, fixedCostTypeFromInput);
            fixedCostCollection.Save();
            InitializeUi();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Could not delete this consumption.\nError message: {exception.Message}");
        }
    }

    private void LvFixedCosts_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LvFixedCosts.SelectedItem is FixedCost selectedFixedCost)
        {
            TxtDate.Text = selectedFixedCost.StartDate.ToString("yyyy-MM-dd");
            CmbType.SelectedIndex = (int)selectedFixedCost.CostType;
            TxtPrice.Text = selectedFixedCost.Price.ToString(CultureInfo.CurrentCulture);
            UpdateUiButtons();
        }
    }

    private void CmbSelectYear_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        throw new NotImplementedException();
    }
}