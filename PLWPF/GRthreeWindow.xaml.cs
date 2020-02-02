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
    /// Interaction logic for GRthreeWindow.xaml
    /// </summary>
    public partial class GRthreeWindow : Window
    {
        GuestRequest gs2;
        IBL bl;

        public GRthreeWindow(GuestRequest gs)
        {
            InitializeComponent();
            
            bl = FactoryBL.GetBL();
            gs2 = new GuestRequest();
            gs2 = gs;
            this.DataContext = gs2;
        }

        #region FOCUS
        private void TextBox_FamilynameGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Insert the family name...")
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
                tb.Text = "Insert the family name...";
                tb.Foreground = Brushes.LightGray;
            }
        }
        private void TextBox_PrivatenameGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Insert your private name...")
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
                tb.Text = "Insert your private name...";
                tb.Foreground = Brushes.LightGray;
            }
        }
        private void TextBox_IDGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Insert the number...")
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
                tb.Text = "Insert the number...";
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
            if (tb.Text == "Insert the number...")
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
                tb.Text = "Insert the number...";
                tb.Foreground = Brushes.LightGray;
            }
        }

        #endregion

        

        private void addrequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long value = Configuration.NumStaticGuestRequest;
                gs2.GuestRequestKey = value;
                gs2.MailAddress = this.textBox2.Text;
                gs2.FamilyName = this.textBox.Text;
                gs2.PrivateName = this.textBox4.Text;
                int a = 0;
                bool h = int.TryParse(textBox3.Text, out a);
                gs2.ID = a;
                int b = 0;
                bool o = int.TryParse(textBox1.Text, out b);
                gs2.PhoneNumber = b;
                bl.AddGuestRequest(gs2);
                MessageBox.Show(gs2.ToString(), "Your Request", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                new MainWindow().Show();
                Close();

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error) ;

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)

        {
            Close();
            new GRsecondWindow(gs2).ShowDialog();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new JeuWindow(gs2).ShowDialog();
            
        }
    }
}
