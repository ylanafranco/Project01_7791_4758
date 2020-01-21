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
using Microsoft.Win32;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AddHostingUnitWindow.xaml
    /// </summary>
    public partial class AddHostingUnitWindow : Window
    {
        public static HostingUnit hostingU;
        private int imageIndex;
        private Viewbox vbImage;
        private Image MyImage;
        IBL bl;

        public AddHostingUnitWindow()
        {
            vbImage = new Viewbox();
            InitializeComponent();
            bl = FactoryBL.GetBL();
            hostingU = new HostingUnit();
            this.DataContext = hostingU;
            areacombobox.ItemsSource = Enum.GetValues(typeof(BE.Enumeration.Area));
            typecombobox.ItemsSource = Enum.GetValues(typeof(BE.Enumeration.Type));
            hostingU.Uris = new List<string>();

            imageIndex = 0;
            vbImage.Width = 200;
            vbImage.Height = 75;
            vbImage.Stretch = Stretch.Fill;
            mygrid.Children.Add(vbImage);
            Grid.SetColumn(vbImage, 1);
            
            vbImage.MouseUp += vbImage_MouseUp;
            vbImage.MouseEnter += vbImage_MouseEnter;
            vbImage.MouseLeave += vbImage_MouseLeave;



        }

        #region FOCUS
        private void TextBox_IdGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "enter your id...")
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
                tb.Text = "enter your id...";
                tb.Foreground = Brushes.LightGray;
            }
        }

        private void TextBox_NameGFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "enter a name...")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_NameLFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "enter a name...";
                tb.Foreground = Brushes.LightGray;
            }
        }

        #endregion

        private void areacombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ADD_Click(object sender, RoutedEventArgs e)
        {
            try 
            {

                int num = 0;
                bool temp = int.TryParse(this.id.Text, out num);
                Host host = bl.GetHost(num);
                hostingU.Owner = host;
                hostingU.HostingUnitKey = Configuration.NumStaticHostingUnit;
                hostingU.HostingUnitName = this.nameHU.Text;
                BE.Enumeration.Type Typetemp;
                Enum.TryParse<BE.Enumeration.Type>(typecombobox.SelectedValue.ToString(), out Typetemp);
                hostingU.Typee = Typetemp;
                BE.Enumeration.Area Areatemp;
                Enum.TryParse<BE.Enumeration.Area>(areacombobox.SelectedValue.ToString(), out Areatemp);
                hostingU.Areaa = Areatemp;
                temp = int.TryParse(this.boxnumroom.Text, out num);
                hostingU.Room = num;
                temp = int.TryParse(this.bedbox.Text, out num);
                hostingU.Bed = num;
                double value = 0;
                temp = double.TryParse(this.pricebox.Text, out value);
                hostingU.PricePerNight = value;
                hostingU.Diary = new bool[12, 31];
                bl.initMatrice(hostingU.Diary);
                NextAddHostingU nextwindow = new NextAddHostingU(hostingU);
                nextwindow.DataContext = hostingU;
                nextwindow.ShowDialog();
                Close();    
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                hostingU.Uris.Add(filename);
            }
            MyImage = CreateViewImage();
            vbImage.Child = null;
            vbImage.Child = MyImage;            
        }

        private Image CreateViewImage()
        {
            Image dynamicImage = new Image();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(hostingU.Uris[imageIndex]);
            bitmap.EndInit();
            // Set Image.Source
            dynamicImage.Source = bitmap;
            // Add Image to Window
            return dynamicImage;
        }

        private void vbImage_MouseLeave(object sender, MouseEventArgs e)
        {
            vbImage.Width = 200;
            vbImage.Height = 75;
        }
        private void vbImage_MouseEnter(object sender, MouseEventArgs e)
        {
            vbImage.Width = this.Width / 3;
            vbImage.Height = this.Height;
        }
        private void vbImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            vbImage.Child = null;
            imageIndex =
           (imageIndex + hostingU.Uris.Count - 1) % hostingU.Uris.Count;
            MyImage = CreateViewImage();
            vbImage.Child = MyImage;
        }

        
    }
}
