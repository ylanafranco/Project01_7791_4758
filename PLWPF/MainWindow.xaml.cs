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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonGuestRequest_Click(object sender, RoutedEventArgs e)
        {
            new GuestRequestWindow().ShowDialog();
            Close();
        }

        private void ButtonHostingUnit_Click(object sender, RoutedEventArgs e)
        {
            new MenuHostingUnit().ShowDialog();
            Close();
        }

        private void ButtonUser_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().ShowDialog();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.twitter.com");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://web.whatsapp.com/");
        }

        private void sortir(object sender, RoutedEventArgs e)
        {
            new bye().ShowDialog();
            Close();
        }
    }
}
