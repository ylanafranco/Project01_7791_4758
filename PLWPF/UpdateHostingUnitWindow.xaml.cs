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
    public partial class UpdateHostingUnitWindow : Window
    {
        IBL bl;

        //List<HostingUnit> mylist;

        HostingUnit myhosting;
        public UpdateHostingUnitWindow(Host myhost)
        {// List<HostingUnit> mylist = new List<HostingUnit>();
            InitializeComponent();
            bl = FactoryBL.GetBL();
            IEnumerable<HostingUnit> myliste = bl.HostingUnitPerHost(myhost);
            comboBox.ItemsSource = myliste;
            comboBox.DisplayMemberPath = "HostingUnitName";
            this.myhosting = new HostingUnit();
            this.DataContext = myhosting;



        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HostingUnit myhosting = comboBox.SelectedItem as HostingUnit;
            if (myhosting.PetsAccepted == true)
                pets.IsChecked = true;
            else
                pets.IsChecked = false;
            if (myhosting.Parking == true)
                parkingbox.IsChecked = true;
            else
                parkingbox.IsChecked = false;

            if (myhosting.KidsClub == true)
                kids.IsChecked = true;
            else
                kids.IsChecked = false;
            if (myhosting.Pool == true)
                pool.IsChecked = true;
            else
                pool.IsChecked = false;
            if (myhosting.Spa == true)
                spabox.IsChecked = true;
            else
                spabox.IsChecked = false;
            if (myhosting.WifiIncluded == true)
                wifi.IsChecked = true;
            else
                wifi.IsChecked = false;
            if (myhosting.HandicapAccess == true)
                handip.IsChecked = true;
            else
                handip.IsChecked = false;
            if (myhosting.FoodIncluded == true)
                food.IsChecked = true;
            else
                food.IsChecked = false;
            if (myhosting.AirConditionner == true)
                clim.IsChecked = true;
            else
                clim.IsChecked = false;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (spabox.IsChecked == true)
                {
                    myhosting.Spa = true;
                }
                else
                    myhosting.Spa = false;
                if (sportbox.IsChecked == true)
                {
                    myhosting.SportRoom = true;
                }
                else
                {
                    myhosting.SportRoom = false;
                }
                if (parkingbox.IsChecked == true)
                {
                    myhosting.Parking = true;
                }
                else
                {
                    myhosting.Parking = false;
                }
                if (food.IsChecked == true)
                {
                    myhosting.FoodIncluded = true;
                }
                else
                {
                    myhosting.FoodIncluded = false;
                }
                if (clim.IsChecked == true)
                {
                    myhosting.AirConditionner = true;
                }
                else
                {
                    myhosting.AirConditionner = false;
                }
                if (wifi.IsChecked == true)
                {
                    myhosting.WifiIncluded = true;
                }
                else
                {
                    myhosting.WifiIncluded = false;
                }
                if (pets.IsChecked == true)
                {
                    myhosting.PetsAccepted = true;
                }
                else
                {
                    myhosting.PetsAccepted = false;
                }
                if (handip.IsChecked == true)
                {
                    myhosting.HandicapAccess = true;
                }
                else
                {
                    myhosting.HandicapAccess = false;
                }
                if (kids.IsChecked == true)
                {
                    myhosting.KidsClub = true;
                }
                else
                {
                    myhosting.KidsClub = false;
                }
                if (pool.IsChecked == true)
                {
                    myhosting.Pool = true;
                }
                else
                {
                    myhosting.Pool = false;
                }
                bl.UpdateHostingUnit(myhosting);
                //faut faire bl.update
                MessageBox.Show("Your hosting unit has been updated\n" + myhosting.ToString(), "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
                Host host1 = new Host();
                host1 = myhosting.Owner;
                new MyAccountWindow(host1).ShowDialog();
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new MenuOrderWindow(myhosting.Owner).Show();
            Close();

        }
    }
}

