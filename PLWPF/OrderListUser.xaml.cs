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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for OrderListUser.xaml
    /// </summary>
    public partial class OrderListUser : Window
    {
        private ObservableCollection<Order> orderCollection;
        public ObservableCollection<Order> OrderCollection
        {
            get
            {
                return orderCollection;
            }

            set
            {
                orderCollection = value;
            }
        }
        IBL bl;
        List<Order> listor;

        public OrderListUser()
        {
            OrderCollection = new ObservableCollection<Order>();
            InitializeComponent();
            bl = FactoryBL.GetBL();
            List<string> mylist = new List<string>();
            mylist.Add("All Orders");
            mylist.Add("Orders Group By ");

            mycombobox.ItemsSource = mylist;
            listor = new List<Order>();
            listor = bl.GetAllOrder().ToList();
            foreach (Order or in listor)
                OrderCollection.Add(or);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new UserWindow().Show();
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> mylist2 = new List<string>();
            mylist2.Add("Create Date");
            mylist2.Add("Price of the Stay");
            mylist2.Add("Status");
            if (mycombobox.SelectedIndex == 0)
            {
                findGrid.ItemsSource = bl.GetAllOrder();
            }
            else if (mycombobox.SelectedIndex == 1)
            {

                comboboxgroup.ItemsSource = mylist2;
                findGrid.ItemsSource = bl.GetAllOrder();
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
                    x.GroupDescriptions.Add(new PropertyGroupDescription("CreateDate"));
                }
            }
            if (comboboxgroup.SelectedIndex == 1 && mycombobox.SelectedIndex == 1)
            {
                ICollectionView x = CollectionViewSource.GetDefaultView(findGrid.ItemsSource);
                if (x != null && x.CanGroup)
                {
                    x.GroupDescriptions.Clear();
                    x.GroupDescriptions.Add(new PropertyGroupDescription("PriceOfTheStay"));
                }
            }
            if (comboboxgroup.SelectedIndex == 2 && mycombobox.SelectedIndex == 1)
            {
                ICollectionView x = CollectionViewSource.GetDefaultView(findGrid.ItemsSource);
                if (x != null && x.CanGroup)
                {
                    x.GroupDescriptions.Clear();
                    x.GroupDescriptions.Add(new PropertyGroupDescription("Status"));
                }
            }
        }
    }
}
