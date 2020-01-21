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
    /// Interaction logic for MenuOrderWindow.xaml
    /// </summary>
    public partial class MenuOrderWindow : Window
    {
        IBL bl;
        private Host myhost;

        public MenuOrderWindow(Host host)
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            myhost = new Host();
            myhost = host;
            List<HostingUnit> listHostingUnit = bl.HostingUnitPerHost(myhost);
            //List<Order> mylistOrder = bl.OrderSelonHostingUnit(listHostingUnit);
            listorder.ItemsSource = bl.OrderSelonHostingUnit(listHostingUnit);
            //mylistOrder;
            listorder.DisplayMemberPath = "OrderKey";
        }

        

        private void Selectorderbtn_Click(object sender, RoutedEventArgs e)
        {
            Order myorder = listorder.SelectedItem as Order;
            new UpdateOrderWindow(myorder).ShowDialog();
            
            Close();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new GuestRequestListWindow(myhost).ShowDialog();
            Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
