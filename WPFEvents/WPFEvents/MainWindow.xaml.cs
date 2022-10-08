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

namespace WPFEvents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            button.Click += button_MyOtherClick;
            button.Click += button_MyLastClick;

        }

        private void button_MyLastClick(object sender, RoutedEventArgs e)
        {
            myLastLabel.Content = "No Hello";
        }

        private void button_MyOtherClick(object sender, RoutedEventArgs e)
        {
            myOtherLabel.Content = "Hello World";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MyLabel.Content = "Hello World";
        }
    }
}
