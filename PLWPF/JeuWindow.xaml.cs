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
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for JeuWindow.xaml
    /// </summary>
    public partial class JeuWindow : Window
    {
        IBL bl; 
        public JeuWindow()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();

        }

        #region FOCUS
        private void TextBox_NameGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Enter a number between 1 and 30...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_NameLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Enter a number between 1 and 30...";
                tb.Foreground = Brushes.LightGray;
            }
        }
        #endregion

        private void button_valid_Click(object sender, RoutedEventArgs e)
        {
            int mynum = 0;
            bool h = int.TryParse(textbchance.Text, out mynum);
            bool rep = bl.testYourChance(mynum);
            if (rep == true)
            {
                MessageBox.Show("Omg, you just win your stay for free. Congrats!!", "Result", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                MessageBox.Show("Oh you lose! Test your chance in your next trip.", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
