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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MenuHostingUnit.xaml
    /// </summary>
    public partial class MenuHostingUnit : Window
    {
        public MenuHostingUnit()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new QuestionHostWindow().ShowDialog();
            Close();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            new HostConfWindow().ShowDialog();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }

}
