using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Currency_Converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        private bool cbFrom_USelected = false;
        private bool cbTo_USelected = false;
        private JObject currencyExchange;
        public MainWindow()
        {
            InitializeComponent();
            CBCurrencyMV cBCurrencyMV = new CBCurrencyMV();
            cbFrom.DataContext = cBCurrencyMV;
            cbTo.DataContext = cBCurrencyMV;
            currencyExchange = cBCurrencyMV.getCurrencyExchange();
            tbDate.Text = "Last Update: " + currencyExchange["date"].ToString();
            initCurrency();
        }
        private void initCurrency()
        {
            tbFrom.Text = "0";
            
            cbFrom.SelectedIndex = 0;
            cbTo.SelectedIndex = 1;
        }

        private void clearCurrency()
        {
            tbFrom.Text = "";
            tbTo.Text = "";
        }


        private void TbFrom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(tbFrom.Text, out _))
            {
                tbFrom.Text = "";
            }
            else if (tbFrom.IsFocused)
            {
                calcCurrency(tbFrom, tbTo, cbFrom, cbTo);
            }
        }

        private void TbTo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse(tbTo.Text, out _) && tbTo.IsFocused)
            {
                tbTo.Text = "";
            }
            else if (tbTo.IsFocused)
            {
                calcCurrency(tbTo, tbFrom, cbTo, cbFrom);
            }

        }

        private void BtnSwapCurrency_Click(object sender, RoutedEventArgs e)
        {
            calcCurrency(tbFrom, tbTo, cbFrom, cbTo);
            int fromCurrency = cbFrom.SelectedIndex;
            cbFrom.SelectedIndex = cbTo.SelectedIndex;
            cbTo.SelectedIndex = fromCurrency;

            calcCurrency(tbFrom, tbTo, cbFrom, cbTo);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clearCurrency();
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            calcCurrency(tbFrom, tbTo, cbFrom, cbTo);
        }

        private void calcCurrency(TextBox fromText, TextBox toText, ComboBox fromCombo, ComboBox toCombo)
        {
            if (!string.IsNullOrEmpty(fromText.Text))
            {
                double fromCurrency = ((KeyValuePair<string, KeyValuePair<double, string>>)fromCombo.SelectedItem).Value.Key;
                double toCurrency = ((KeyValuePair<string, KeyValuePair<double, string>>)toCombo.SelectedItem).Value.Key;
                double result = double.Parse(fromText.Text) * toCurrency / fromCurrency;

                toText.Text = result.ToString("F");
                tbFromRes.Text = tbFrom.Text + " " + ((KeyValuePair<string, KeyValuePair<double, string>>)cbFrom.SelectedValue).Value.Value + " equal";
                tbToRes.Text = tbTo.Text + " " + ((KeyValuePair<string, KeyValuePair<double, string>>)cbTo.SelectedValue).Value.Value;
            }
        }


        private void cbFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFrom_USelected)
            {
                calcCurrency(tbFrom, tbTo, cbFrom, cbTo);
                cbFrom_USelected = false;
            }
        }

        private void cbTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTo_USelected)
            {
                calcCurrency(tbFrom, tbTo, cbFrom, cbTo);
                cbFrom_USelected = false;
            }
        }

        private void cbFrom_DropDownOpened(object sender, EventArgs e)
        {
            cbFrom_USelected = true;
        }

        private void cbTo_DropDownOpened(object sender, EventArgs e)
        {
            cbTo_USelected = true;
        }
    }
}
