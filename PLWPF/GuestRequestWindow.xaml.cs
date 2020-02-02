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
    /// Interaction logic for GuestRequestWindow.xaml
    /// </summary>
    public partial class GuestRequestWindow : Window
    {

        public static GuestRequest gs;
        IBL bl;

        public GuestRequestWindow()
        {
            InitializeComponent();

            bl = FactoryBL.GetBL();
            gs = new GuestRequest();
            this.DataContext = gs;
            this.areacomboBox.ItemsSource = Enum.GetValues(typeof(Enumeration.Area));
            this.typecomboBox.ItemsSource = Enum.GetValues(typeof(Enumeration.Type));
            datePickerentry.DisplayDateStart = DateTime.Now;
            datePickerrelease.DisplayDateStart = DateTime.Now;
        }

        #region FOCUS
        private void TextBox_AdultsGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Insert the number...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_AdultsLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Insert the number...";
                tb.Foreground = Brushes.LightGray;
            }
        }


        private void TextBox_ChildrenGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Insert the number...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_ChildrenLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Insert the number...";
                tb.Foreground = Brushes.LightGray;
            }
        }

        #endregion

        #region COMBO BOX


        private void typecomboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            switch (typecomboBox.SelectedItem.ToString())
            {
                case "Zimmer":
                    gs.Type = Enumeration.Type.Zimmer;
                    break;
                case "Camping":
                    gs.Type = Enumeration.Type.Camping;
                    break;
                case "Appartment":
                    gs.Type = Enumeration.Type.Appartment;
                    break;
                case "House":
                    gs.Type = Enumeration.Type.House;
                    break;
                case "Hotel":
                    gs.Type = Enumeration.Type.Hotel;
                    break;
                case "Hostel":
                    gs.Type = Enumeration.Type.Hostel;
                    break;
                default:
                    break;
            }
        }
       

        private void AreacomboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            switch (areacomboBox.SelectedItem.ToString())
            {
                case "Center":
                    gs.Area = Enumeration.Area.Center;
                    break;
                case "Jerusalem":
                    gs.Area = Enumeration.Area.Jerusalem;
                    break;
                case "North":
                    gs.Area = Enumeration.Area.North;
                    break;
                case "South":
                    gs.Area = Enumeration.Area.South;
                    break;
                default:
                    break;
            }

        }


        #endregion

        
        private void search_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
            bool h = int.TryParse(textBox1.Text, out a);
            gs.Adults = a;
            h = int.TryParse(textBox2.Text, out a);
            gs.Children = a;
            gs.NumTotalPersons = bl.NumTotalPersonGR(gs.Adults, gs.Children);
            gs.RegistrationDate = DateTime.Now;
            string dateentry, daterelease;
            dateentry = datePickerentry.SelectedDate.ToString();
            daterelease = datePickerrelease.SelectedDate.ToString();
            gs.EntryDate = Convert.ToDateTime(dateentry);
            gs.ReleaseDate = Convert.ToDateTime(daterelease);
            new GRsecondWindow(gs).ShowDialog();
            Close();

        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
        }
    }
}
