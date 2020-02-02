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
    /// Interaction logic for GuestRequestListWindow.xaml
    /// </summary>
    public partial class GuestRequestListWindow : Window
    {
        IBL bl;
        public GuestRequest myrequest;
        public static Host myhost;
        public static List<GuestRequest> mylist;

        public GuestRequestListWindow(Host host)
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            //foreach (var item in bl.GetAllGuestRequest())
            //{
           
               // MessageBox.Show());
            //}
            mylist = bl.GetAllGuestRequest().ToList();
            findGrid.ItemsSource = mylist;
            myrequest = new GuestRequest();
            myhost = new Host();
            myhost = host;
        }

        #region FOCUS
        private void TextBox_IdGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Enter Guest Request Key...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_IdLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Enter Guest Request Key...";
                tb.Foreground = Brushes.LightGray;
            }
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                long num = 0;
                string s = txtboxKey.Text;
                bool temp = long.TryParse(s, out num);
                myrequest = bl.GetGuestRequest(num);
                if (myrequest.Status == Enumeration.GuestRequestStatus.Open)
                {
                    var listhostingunit = from item in bl.HostingUnitPerHost(myhost)
                                          where bl.comparaison(myrequest, item) == true
                                          select item;


                    if (listhostingunit.Count() != 0)
                    {
                        new NewOrdeWindow(myrequest.GuestRequestKey, listhostingunit).ShowDialog();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("You have not hosting unit that match with this request", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    
                }


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error) ;

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            new MenuOrderWindow(myhost).Show();
            Close();
        }
    }
}
