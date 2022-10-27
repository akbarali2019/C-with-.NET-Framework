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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sqlite_CRUD
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public List<User> DatabaseUsers { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }


        

        public void Read()
        {
            using (DataContext context = new DataContext())
            {
                DatabaseUsers = context.Users.ToList();
                ItemList.ItemsSource = DatabaseUsers;   
            }

        }


        public void Delete()
        {


            using (DataContext context = new DataContext())
            {

                User selectedUser = ItemList.SelectedItem as User;

                if (selectedUser != null)
                {
                    User user = context.Users.Single(x=> x.Id == selectedUser.Id);

                    context.Remove(user);
                    
                    context.SaveChanges();
                    
                }
                                
            }
        }

        
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ItemList.SelectedItem == null) return;
            UpdatePage addmember = new UpdatePage(ItemList.SelectedItem as User);
            addmember.ShowDialog();
            Read();
            //Update();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            Read();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            ItemList.Items.Clear();

        }

        private void EnableCheckButton_Click(object sender, RoutedEventArgs e)
        {
            AddMember addmember = new AddMember();
            addmember.ShowDialog();
            Read();
        }
    }
}
