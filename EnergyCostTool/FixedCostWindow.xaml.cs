using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnergyCostTool.Dal;
using EnergyCostTool.Models;
using EnergyCostTool.Models.Enumerations;

namespace EnergyCostTool;

/// <summary>
/// Interaction logic for AddFixedCost.xaml
/// </summary>
public partial class FixedCostWindow : Window, INotifyPropertyChanged
{
    private readonly StandardCostCollection standardCosts;
    public ObservableCollection<StandardCost> StandardCosts { get; protected set; }
    public FixedCostWindow()
    {
        InitializeComponent();
        this.standardCosts = Database.GetStandardCosts();
        DisableChanges();
        InitializeUi();
    }

    private void InitializeUi()
    {
        ClearTextBoxes();
        StandardCosts = new ObservableCollection<StandardCost>(standardCosts.Get().OrderByDescending(price => price.StartDate));
        RaisePropertyChanged("StandardCosts");
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
            int year = Convert.ToInt32(TxtDate.Text.Substring(0, 4));
            int month = Convert.ToInt32(TxtDate.Text.Substring(5));
            if (year is < 2000 or > 2100)
            {
                DisableChanges();
            }

            if (month is < 1 or > 12)
            {
                DisableChanges();
            }

            if (standardCosts.ContainsDataFor(new DateTime(year, month, 1), (FixedCostType)CmbType.SelectedValue))
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
            FixedCostType fixedCostTypeFromInput = (FixedCostType)CmbType.SelectedValue;
            double price = Convert.ToDouble(TxtPrice.Text);
            StandardCost standardCost = new StandardCost(new DateTime(year, month, 1), fixedCostTypeFromInput, price);
            standardCosts.AddOrUpdate(standardCost);
            Database.SaveStandardCosts(standardCosts);
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
            FixedCostType fixedCostTypeFromInput = (FixedCostType)CmbType.SelectedValue;
            double price = Convert.ToDouble(TxtPrice.Text);
            StandardCost standardCost = new StandardCost(new DateTime(year, month, 1), fixedCostTypeFromInput, price);
            standardCosts.AddOrUpdate(standardCost);
            Database.SaveStandardCosts(standardCosts);
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
            FixedCostType fixedCostTypeFromInput = (FixedCostType)CmbType.SelectedValue;
            standardCosts.Delete(standardCosts.Get(new DateTime(year, month, 1), fixedCostTypeFromInput));
            Database.SaveStandardCosts(standardCosts);
            InitializeUi();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Could not delete this consumption.\nError message: {exception.Message}");
        }
    }

    private void LvFixedCosts_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LvFixedCosts.SelectedItem is StandardCost selectedFixedCost)
        {
            TxtDate.Text = selectedFixedCost.StartDate.ToString("yyyy-MM");
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