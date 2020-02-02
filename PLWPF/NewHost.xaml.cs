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
    /// Interaction logic for NewHost.xaml
    /// </summary>
    public partial class NewHost : Window
    {

        public static Host host;
        IBL bl;
        public NewHost()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            host = new Host();
            this.DataContext = host;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
        }

        private void configuratebankaccount_Click(object sender, RoutedEventArgs e)
        {
            host.FamilyName = textBox3.Text;
            int a = 0;
            bool h = int.TryParse(textBox2.Text, out a);
            host.HostKey = a;
            host.PrivateName = textBox4.Text;
            host.MailAddress = textBox6.Text;
            int b = 0;
            bool o = int.TryParse(textBox5.Text, out b);
            host.PhoneNumber = b;
            
            new BankAccountWindow(host).ShowDialog();
            Close();
        }

        #region gris clair

        private void TextBox_FamilynameGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Family name...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_FamilynameLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Family name...";
                tb.Foreground = Brushes.LightGray;
            }
        }

        private void TextBox_PrivatenameGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Private name...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_PrivatenameLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Private name...";
                tb.Foreground = Brushes.LightGray;
            }
        }
        private void TextBox_IDGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Your ID...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_IDLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Your ID...";
                tb.Foreground = Brushes.LightGray;
            }
        }
        private void TextBox_MailGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "example@gmail.com...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_MailLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "example@gmail.com...";
                tb.Foreground = Brushes.LightGray;
            }
        }
        private void TextBox_PhoneGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Phone number...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_PhoneLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Phone number...";
                tb.Foreground = Brushes.LightGray;
            }
        }


        #endregion
    }

}
