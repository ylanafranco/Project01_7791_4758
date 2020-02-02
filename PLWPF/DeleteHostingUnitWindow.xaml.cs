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
    /// Interaction logic for DeleteHostingUnitWindow.xaml
    /// </summary>
    public partial class DeleteHostingUnitWindow : Window
    {

        public static HostingUnit hostingUnit;
        public static Host myhost;
        IBL bl;


        public DeleteHostingUnitWindow(Host host)
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            hostingUnit = new HostingUnit();
            myhost = new Host();
            myhost = host;
            combodelete.ItemsSource = bl.HostingUnitPerHost(myhost);
            combodelete.DisplayMemberPath = "HostingUnitName";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               hostingUnit = combodelete.SelectedItem as HostingUnit;
               HostingUnit item = bl.HostingUnitPerHost(myhost).FirstOrDefault(temp => temp.HostingUnitName == hostingUnit.HostingUnitName);                
               bl.EraseHostingUnit(hostingUnit.HostingUnitKey);
                MessageBox.Show("The Hosting Unit had been removed correctly", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new MyAccountWindow(myhost).ShowDialog();
            Close();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
