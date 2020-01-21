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
    /// Interaction logic for GRsecondWindow.xaml
    /// </summary>
    public partial class GRsecondWindow : Window
    {
        public static GuestRequest gs1;
        IBL bl;
        public GRsecondWindow(GuestRequest gs)
        {
            InitializeComponent();

            bl = FactoryBL.GetBL();
            gs1 = new GuestRequest();
            gs1 = gs;
            this.DataContext = gs1;


        }

        #region check
        private void continue_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {
                gs1.Pool = Enumeration.Response.Necessary;
                checkBox1.IsChecked = false;
                checkBox17.IsChecked = false;
            }

            if (checkBox1.IsChecked == true)
            {
                gs1.Pool = Enumeration.Response.Possible;
                checkBox.IsChecked = false;
                checkBox17.IsChecked = false;
            }

            if (checkBox17.IsChecked == true)
            {
                gs1.Pool = Enumeration.Response.NotInteressed;
                checkBox.IsChecked = false;
                checkBox1.IsChecked = false;
            }


            if (checkBox13.IsChecked == true)
            {
                gs1.Parking = Enumeration.Response.Necessary;
                checkBox12.IsChecked = false;
                checkBox11.IsChecked = false;
            }

            if (checkBox12.IsChecked == true)
            {
                gs1.Parking = Enumeration.Response.Possible;
                checkBox13.IsChecked = false;
                checkBox11.IsChecked = false;
            }

            if (checkBox11.IsChecked == true)
            {
                gs1.Parking = Enumeration.Response.NotInteressed;
                checkBox13.IsChecked = false;
                checkBox12.IsChecked = false;
            }


            if (checkBox16.IsChecked == true)
            {
                gs1.KidClub = Enumeration.Response.Necessary;
                checkBox15.IsChecked = false;
                checkBox14.IsChecked = false;
            }

            if (checkBox15.IsChecked == true)
            {
                gs1.KidClub = Enumeration.Response.Possible;
                checkBox16.IsChecked = false;
                checkBox14.IsChecked = false;
            }

            if (checkBox14.IsChecked == true)
            {
                gs1.KidClub = Enumeration.Response.NotInteressed;
                checkBox15.IsChecked = false;
                checkBox16.IsChecked = false;
            }


            if (checkBox10.IsChecked == true)
            {
                gs1.PetsAccepted = Enumeration.Response.Necessary;
                checkBox9.IsChecked = false;
                checkBox8.IsChecked = false;
            }

            if (checkBox9.IsChecked == true)
            {

                gs1.PetsAccepted = Enumeration.Response.Possible;
                checkBox8.IsChecked = false;
                checkBox10.IsChecked = false;
            }

            if (checkBox8.IsChecked == true)
            {  gs1.PetsAccepted = Enumeration.Response.NotInteressed;
            checkBox9.IsChecked = false;
            checkBox10.IsChecked = false; 
            }

            if (checkBox7.IsChecked == true)
            {
                gs1.FoodIncluded = Enumeration.Response.Necessary;
                checkBox6.IsChecked = false;
                checkBox5.IsChecked = false;
            }

            if (checkBox6.IsChecked == true)
            {
                gs1.FoodIncluded = Enumeration.Response.Possible;
                checkBox7.IsChecked = false;
                checkBox5.IsChecked = false;
            }

            if (checkBox5.IsChecked == true)
            {
                gs1.FoodIncluded = Enumeration.Response.NotInteressed;
                checkBox6.IsChecked = false;
                checkBox7.IsChecked = false;
            }



            if (checkBox4.IsChecked == true)
            {
                gs1.WifiIncluded = Enumeration.Response.Necessary;
                checkBox3.IsChecked = false;
                checkBox2.IsChecked = false;
            }

            if (checkBox3.IsChecked == true)
            {
                gs1.WifiIncluded = Enumeration.Response.Possible;
                checkBox4.IsChecked = false;
                checkBox2.IsChecked = false;
            }

            if (checkBox2.IsChecked == true)
            {
                gs1.WifiIncluded = Enumeration.Response.NotInteressed;
                checkBox4.IsChecked = false;
                checkBox3.IsChecked = false;
            }

            new GRthreeWindow(gs1).ShowDialog();
            Close();


        }

        #endregion

        //return to the menu
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
            Close();
        }

        //return to the precedent page
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new GuestRequestWindow().ShowDialog();
            Close();
        }
    }
}
