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
    /// Interaction logic for NextAddHostingU.xaml
    /// </summary>
    public partial class NextAddHostingU : Window
    {
        public HostingUnit hostingU;
        IBL bl;

        public NextAddHostingU(HostingUnit HU)
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            this.hostingU = new HostingUnit();
            this.hostingU = HU;
            this.DataContext = hostingU;



        }

        

        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (spa.IsChecked == true)
                {
                    hostingU.Spa = true;
                }
                else
                    hostingU.Spa = false;
                if (sportR.IsChecked == true)
                {
                    hostingU.SportRoom = true;
                }
                else
                {
                    hostingU.SportRoom = false;
                }
                if (parking.IsChecked == true)
                {
                    hostingU.Parking = true;
                }
                else
                {
                    hostingU.Parking = false;
                }
                if (Food.IsChecked == true)
                {
                    hostingU.FoodIncluded = true;
                }
                else
                {
                    hostingU.FoodIncluded = false;
                }
                if (aircond.IsChecked == true)
                {
                    hostingU.AirConditionner = true;
                }
                else
                {
                    hostingU.AirConditionner = false;
                }
                if (wifi.IsChecked == true)
                {
                    hostingU.WifiIncluded = true;
                }
                else
                {
                    hostingU.WifiIncluded = false;
                }
                if (pets.IsChecked == true)
                {
                    hostingU.PetsAccepted = true;
                }
                else
                {
                    hostingU.PetsAccepted = false;
                }
                if (handic.IsChecked == true)
                {
                    hostingU.HandicapAccess = true;
                }
                else
                {
                    hostingU.HandicapAccess = false;
                }
                if (kidclub.IsChecked == true)
                {
                    hostingU.KidsClub = true;
                }
                else
                {
                    hostingU.KidsClub = false;
                }
                if (pool.IsChecked == true)
                {
                    hostingU.Pool = true;
                }
                else
                {
                    hostingU.Pool = false;
                }
                
                bl.AddHostingUnit(hostingU);
                MessageBox.Show("Your hosting unit has been saved\n" + hostingU.ToString() , "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
                new MenuHostingUnit().Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }
    }
}
