using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for BankAccountWindow.xaml
    /// </summary>
    public partial class BankAccountWindow : Window
    {
        public static Host myhost;
        public static BankAccount bankacc;
        IBL bl;

        public BankAccountWindow(Host host)
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            myhost = new Host();
            myhost = host;
            bankacc = new BankAccount();
            myhost.BankAccount = new BankAccount();

        }

        #region FOCUS
        private void TextBox_NameGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text != "")
            {
                tb.Text = "";
                tb.FontWeight = FontWeights.Bold;
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_BankNameLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Enter your Bank Name...";

                tb.Foreground = Brushes.LightGray;
            }
        }

        private void TextBox_BankAdressLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Enter your Bank Address...";
                tb.Foreground = Brushes.LightGray;
            }
        }

        private void TextBox_BankCityLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Enter your Bank City...";
                tb.Foreground = Brushes.LightGray;
            }
        }
        private void TextBox_BranchnumLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Enter your Branch Number...";
                tb.Foreground = Brushes.LightGray;
            }
        }
        private void TextBox_BankNumberLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Enter your Bank Number...";
                tb.Foreground = Brushes.LightGray;
            }
        }
        private void TextBox_AccountNumberLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Enter your Account Number...";
                tb.Foreground = Brushes.LightGray;
            }
        }
        #endregion

        private void validation_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                
                int value = 0;
                bool temp = int.TryParse(this.bankaccountnumber.Text, out value);
                myhost.BankAccountNumber = value;
                bankacc.BankName = this.bankname.Text;
                //myhost.BankAccount.BankName = this.bankname.Text;
                temp = int.TryParse(this.banknumber.Text, out value);
                //myhost.BankAccount.BankNumber = value;
                bankacc.BankNumber = value;
                bankacc.BranchAddress = this.branchaddress.Text;
                //myhost.BankAccount.BranchAddress = (this.branchaddress.Text);
                bankacc.BranchCity = this.branchaddress.Text;
                //myhost.BankAccount.BranchCity = this.branchcity.Text;
                temp = int.TryParse(this.branchnumber.Text, out value);
                bankacc.BranchNumber = value;
                //myhost.BankAccount.BranchNumber = value;
                myhost.BankAccount = bankacc;
                if (collectionclearance.IsChecked == true && bl.checkCollectionClearance(myhost))
                {
                    myhost.CollectionClearance = true;
                }
                else
                    myhost.CollectionClearance = false;
                bl.AddHost(myhost);
                MessageBox.Show("Your registration has been saved", "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
                new AddHostingUnitWindow().Show();
                Close();
            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
            }

        }
    }
}
