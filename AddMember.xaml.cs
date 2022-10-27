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
                

                if (name != null && address != null && age != null)
                {
                    context.Users.Add(new User() { Name = name, Address = address, Age = age});
                    context.SaveChanges();
                }                
                Close();

            }
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Create();
            
        }  
    }

}
