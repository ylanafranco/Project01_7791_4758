using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for NewOrdeWindow.xaml
    /// </summary>
    public partial class NewOrdeWindow : Window
    {
        IBL bl;
        public static Order myorder;
        public static GuestRequest myrequest;
        public static List<HostingUnit> myliste;
        public static HostingUnit myhosting;

        public NewOrdeWindow(long guestreqKey, IEnumerable<HostingUnit> mylist)
        {
            InitializeComponent();
            bl = FactoryBL.GetBL();
            myrequest = new GuestRequest();
            myrequest = bl.GetGuestRequest(guestreqKey);
            myorder = new Order();
            myliste = new List<HostingUnit>();
            myliste = mylist.ToList();
            comboHU.ItemsSource = myliste;
            comboHU.DisplayMemberPath = "HostingUnitName";
            this.DataContext = myrequest;
            myhosting = new HostingUnit();

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
            try
            {
                myorder = new Order();
                myorder.OrderKey = BE.Configuration.NumStaticOrder;
                myorder.GuestRequestKey = myrequest.GuestRequestKey;
                myhosting = comboHU.SelectedItem as HostingUnit;
                myorder.HostingUnitKey = myhosting.HostingUnitKey;
                myorder.Status = Enumeration.OrderStatus.NotAddressed;
                myorder.CreateDate = DateTime.Now;
                bl.AddOrder(myorder);
                email.IsEnabled = true;

                //MessageBox.Show(TostringMatrice(myhosting.Diary));

                MessageBox.Show("The order has been created :\n" + myorder.ToString(), "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
             }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateOrder(myorder);
                bl.UpdateOrder(myorder);
                //bl.UpdateOrderAndGuestReq(myorder);
                //MessageBox.Show(myrequest.ToString());
                SendEmail mailSender = new SendEmail();

                string to = myrequest.MailAddress;
                string subject = "Proposition of Hosting Unit";
                string body = string.Format("Hi {0} {1}, this is a propostion for your request :\n" +
                    "We have a beautiful place that meets all your requests.\n" +
                    "The price for the stay is {2}.\n" +
                    "For more informations, you can contact me by mail {3} or by phone {4}." +
                    "Hope to see you soon in our places...", myrequest.FamilyName,myrequest.PrivateName, myorder.PriceOfTheStay, myhosting.Owner.MailAddress, myhosting.Owner.PhoneNumber);

                Thread thread = new Thread(() => mailSender.SendMail(to, subject, body));
                thread.Start();

                MessageBox.Show("A mail proposition has been sent to the client !", "Validation", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new MenuOrderWindow(myhosting.Owner).ShowDialog();
            Close();
        }
    }
}
