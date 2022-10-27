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
    /// Interaction logic for AddMember.xaml
    /// </summary>
    public partial class AddMember : Window
    {
        
        public List<User> DatabaseUsers { get; private set; }
        
        public AddMember()
        {
            InitializeComponent();
            
        }
        public void Create()
        {

            using (DataContext context = new DataContext())
            {
                var name = NameTextBox.Text;
                var address = AddressTextBox.Text;
                var age = AgeTextBox.Text;
                try
                {
                    if (int.Parse(age) <= 0 || int.Parse(age) >= 100)
                    {
                        throw new Exception(); 
                    }
                    context.Users.Add(new User() { Name = name, Address = address, Age = age });
                    context.SaveChanges();
                    Close();

                }
                catch (Exception)
                {
                    MessageBox.Show(age + 
                        " is an invalid input for an Age. " +
                        "Please enter a valid input!");
                }

                /*
                else if(name != null && address != null && age != null)
                {
                    context.Users.Add(new User() { Name = name, Address = address, Age = age});
                    context.SaveChanges();
                }
                */
                

            }
        }


        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Create();
            
        }  
    }

}
