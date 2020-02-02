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
    /// Interaction logic for HostConfWindow.xaml
    /// </summary>
    public partial class HostConfWindow : Window
    {
        public Host myhost;
        IBL bl;

        public HostConfWindow()
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            myhost = new Host();

        }

        private void ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Host myhost = new Host();
                myhost = bl.GetHost(int.Parse(passw.Password));
                if ((myhost.FamilyName == username.Text) && myhost.HostKey == int.Parse(passw.Password))
                {
                    new MyAccountWindow(myhost).ShowDialog();
                    Close();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Incorrect Username Or Password", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

            }


           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
