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

namespace EducationPurpose
{
    /// <summary>
    /// Interaction logic for exampleWindow.xaml
    /// </summary>
    public partial class exampleWindow : Window
    {
        public exampleWindow()
        {
            InitializeComponent();
        }

        private void MainButtonLast2_Clicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You clicked on the button!");
        }
    }
}
