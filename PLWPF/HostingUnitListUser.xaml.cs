using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for HostingUnitListUser.xaml
    /// </summary>
    public partial class HostingUnitListUser : Window
    {
        private ObservableCollection<HostingUnit> HostingunitCollection;
        public ObservableCollection<HostingUnit> HostingUnitCollection
        {
            get
            {
                return HostingunitCollection;
            }

            set
            {
                HostingunitCollection = value;
            }
        }
        IBL bl;
        List<HostingUnit> listhu;

        public HostingUnitListUser()
        {
            HostingunitCollection = new ObservableCollection<HostingUnit>();
            InitializeComponent();
            bl = FactoryBL.GetBL();
            
            List<string> mylist = new List<string>();
            mylist.Add("All Hosting Unit");
            mylist.Add("Hosting Unit Group By ");

            mycombobox.ItemsSource = mylist;
            listhu = new List<HostingUnit>();
            listhu = bl.GetAllHostingUnitCollection().ToList();
            foreach (HostingUnit hu in listhu)
                HostingunitCollection.Add(hu);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new UserWindow().Show();
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> mylist2 = new List<string>();
            mylist2.Add("Area");
            mylist2.Add("Type");
            mylist2.Add("Rooms");
            mylist2.Add("Price Per night");
            if (mycombobox.SelectedIndex == 0)
            {
                findGrid.ItemsSource = bl.GetAllHostingUnitCollection();
            }
            else if (mycombobox.SelectedIndex == 1)
            {

                comboboxgroup.ItemsSource = mylist2;
                findGrid.ItemsSource = bl.GetAllHostingUnitCollection();
            }

        }

        private void comboboxgroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxgroup.SelectedIndex == 0 && mycombobox.SelectedIndex == 1)
            {
                ICollectionView x = CollectionViewSource.GetDefaultView(findGrid.ItemsSource);
                if (x != null && x.CanGroup)
                {
                    x.GroupDescriptions.Clear();
                    x.GroupDescriptions.Add(new PropertyGroupDescription("Areaa"));
                }
            }
            if (comboboxgroup.SelectedIndex == 1 && mycombobox.SelectedIndex == 1)
            {
                ICollectionView x = CollectionViewSource.GetDefaultView(findGrid.ItemsSource);
                if (x != null && x.CanGroup)
                {
                    x.GroupDescriptions.Clear();
                    x.GroupDescriptions.Add(new PropertyGroupDescription("Typee"));
                }
            }
            if (comboboxgroup.SelectedIndex == 2 && mycombobox.SelectedIndex == 1)
            {
                ICollectionView x = CollectionViewSource.GetDefaultView(findGrid.ItemsSource);
                if (x != null && x.CanGroup)
                {
                    x.GroupDescriptions.Clear();
                    x.GroupDescriptions.Add(new PropertyGroupDescription("Room"));
                   // x.GroupDescriptions.OrderByDescending();
                }
            }
            if (comboboxgroup.SelectedIndex == 3 && mycombobox.SelectedIndex == 1)
            {
                ICollectionView x = CollectionViewSource.GetDefaultView(findGrid.ItemsSource);
                if (x != null && x.CanGroup)
                {
                    x.GroupDescriptions.Clear();
                    ////x.GroupDescriptions.Add(new PropertyGroupDescription("PricePerNight"));
                }
            }
        }
    }
}
