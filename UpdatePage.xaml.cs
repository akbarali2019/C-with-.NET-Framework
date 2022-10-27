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


namespace Sqlite_CRUD
{
    /// <summary>
    /// Interaction logic for UpdatePage.xaml
    /// </summary>
    public partial class UpdatePage : Window
    {
        
        public User SelectedUser { get; private set; }
        public UpdatePage(User selected)
        {
            SelectedUser = selected;
            InitializeComponent();
            NameTextBox.Text = selected.Name;
            AddressTextBox.Text = selected.Address;
            AgeTextBox.Text = selected.Age;
        }


        private void Update()
        {


            using (DataContext context = new DataContext())
            {

                User selectedUser = SelectedUser;

                var name = NameTextBox.Text;
                var address = AddressTextBox.Text;
                var age = AgeTextBox.Text;

                if (name != null && address != null && age != null)
                {
                    User user = context.Users.Find(selectedUser.Id);
                    user.Name = name;
                    user.Address = address;
                    user.Age = age;

                    context.SaveChanges();
                }
                
                Close();
                
            }
            
        }

        private void Update_Selected_User(object sender, RoutedEventArgs e)
        {
            Update();

            
        }
    }
}
