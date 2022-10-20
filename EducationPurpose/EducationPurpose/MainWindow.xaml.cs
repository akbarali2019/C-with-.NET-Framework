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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EducationPurpose
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string myname { get; set; } = "Akbarali";
        private int clickedtimes;
        private string name = "";
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void MainButton_Clicked(object sender, RoutedEventArgs e)
        {
            clickedtimes++;

            MainLabel.Content = "You Clicked The Button " + clickedtimes + " Times!";
        }

        private void MainButton2_Clicked(object sender, RoutedEventArgs e)
        {
            exampleWindow secondWindow = new exampleWindow();
            secondWindow.Show();
        }


        private void MainButtonLast1_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void MainButtonLast2_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void MainButtonLast3_Clicked(object sender, RoutedEventArgs e)
        {
            name = textBox.Text;
            MainLabelLast3.Content = $"Hello, {name}";
        }

    }
}
