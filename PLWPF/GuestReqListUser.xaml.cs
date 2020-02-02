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
    /// Interaction logic for GuestReqListUser.xaml
    /// </summary>
    public partial class GuestReqListUser : Window
    {
        private ObservableCollection<GuestRequest> GuestrequestCollection;
        public ObservableCollection<GuestRequest> GuestRequestCollection
        {
            get
            {
                return GuestrequestCollection;
            }

            set
            {
                GuestrequestCollection = value;
            }
        }
        IBL bl;
        List<GuestRequest> listgs;

        public GuestReqListUser()
        {
            GuestRequestCollection = new ObservableCollection<GuestRequest>();
            InitializeComponent();
            bl = FactoryBL.GetBL();
            //findGrid.ItemsSource = bl.GetAllGuestRequest();
            List<string> mylist = new List<string>();
            mylist.Add("All Guest Request");
            mylist.Add("Guest Request Group By ");
            
            mycombobox.ItemsSource = mylist;
            listgs = new List<GuestRequest>();
            listgs = bl.GetAllGuestRequest().ToList();
            foreach (GuestRequest gs in listgs)
                GuestRequestCollection.Add(gs);




        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new UserWindow().ShowDialog();
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> mylist2 = new List<string>();
            mylist2.Add("Area");
            mylist2.Add("Num Total of Persons");
            mylist2.Add("Type");
            if (mycombobox.SelectedIndex == 0)
            {
                findGrid.ItemsSource = listgs;
            }
            else if (mycombobox.SelectedIndex == 1)
            {

                comboboxgroup.ItemsSource = mylist2;
                findGrid.ItemsSource = listgs;
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
                    x.GroupDescriptions.Add(new PropertyGroupDescription("Area"));
                    }
            }
            if (comboboxgroup.SelectedIndex == 1 && mycombobox.SelectedIndex == 1) 
            {
                ICollectionView x = CollectionViewSource.GetDefaultView(findGrid.ItemsSource);
                if (x != null && x.CanGroup)
                {
                    x.GroupDescriptions.Clear();
                    x.GroupDescriptions.Add(new PropertyGroupDescription("NumTotalPersons"));
                      
                }
            }
            if (comboboxgroup.SelectedIndex == 2 && mycombobox.SelectedIndex == 1) 
            {
                ICollectionView x = CollectionViewSource.GetDefaultView(findGrid.ItemsSource);
                if (x != null && x.CanGroup)
                {
                    
                    x.GroupDescriptions.Clear();
                    x.GroupDescriptions.Add(new PropertyGroupDescription("Type"));
                   
                }
            }
        }

    }
}

