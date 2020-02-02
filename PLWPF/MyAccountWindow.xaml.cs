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
    /// Interaction logic for MyAccountWindow.xaml
    /// </summary>
    public partial class MyAccountWindow : Window
    {
        public static Host myhost;
        IBL bl;
        public MyAccountWindow(Host host)
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            myhost = new Host();
            myhost = host;
            this.DataContext = myhost;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            new DeleteHostingUnitWindow(myhost).ShowDialog();
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            new MenuOrderWindow(myhost).ShowDialog();
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            new UpdateHostingUnitWindow(myhost).ShowDialog();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show ();
            Close();
        }
    }
}
