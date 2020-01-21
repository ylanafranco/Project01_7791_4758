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
    /// Interaction logic for QuestionHostWindow.xaml
    /// </summary>
    public partial class QuestionHostWindow : Window
    {
        public QuestionHostWindow()
        {
            InitializeComponent();
        }

        private void New_Checked(object sender, RoutedEventArgs e)
        {
            new NewHost().Show();
            this.Close();


        }

        private void Old_Checked(object sender, RoutedEventArgs e)
        {

            new AddHostingUnitWindow().Show();
            this.Close();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MenuHostingUnit().Show();
            Close();
        }
    }
}
