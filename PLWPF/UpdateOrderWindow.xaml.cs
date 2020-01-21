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
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for UpdateOrderWindow.xaml
    /// </summary>
    public partial class UpdateOrderWindow : Window
    {
        IBL bl;
        public static Order myorderr;
        
        HostingUnit myhu;
        public UpdateOrderWindow(Order myorder)
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            this.DataContext = myorder;
            myorderr = new Order();
            myorderr = myorder;
            text.Text = myorderr.ToString();
            //findGrid.SelectedItem = myorderr;
            myhu = new HostingUnit();
            myhu = bl.GetHostingUnit(myorderr.HostingUnitKey);

        }

        //private string TostringMatrice(bool[,] mat)
        //{
        //    string s = "";
        //    for (int i = 0; i < mat.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < mat.GetLength(1); j++)
        //        {
        //            s += mat[i, j].ToString() + " ";
        //        }
        //        s += "\n";
        //    }
        //    return s;
        //}


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HostingUnit myhosting = bl.GetHostingUnit(myorderr.HostingUnitKey);
            GuestRequest GR = bl.GetGuestRequest(myorderr.GuestRequestKey);
            //bool flag = bl.CheckMatrice(GR, myhosting);
            if (myorderr.Status == BE.Enumeration.OrderStatus.ClosedForCustomerUnresponsiveness)
            {
                bl.UpdateOrder(myorderr);
                MessageBox.Show("The dates are not available anymore for this hosting unit", "Information", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                bl.UpdateOrder(myorderr);
                ////MessageBox.Show(TostringMatrice(myhosting.Diary));
                MessageBox.Show("The status has been updated.\nOrder Details :\n" + myorderr.ToString() + "\nThe commission cost is " + bl.CommissionCost(GR) + "NIS", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            myorderr.Status = BE.Enumeration.OrderStatus.ClosedForCustomerResponse;
            
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            myorderr.Status = BE.Enumeration.OrderStatus.ClosedForCustomerUnresponsiveness;
            }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new MenuOrderWindow(myhu.Owner).Show();
        }
    }
}
