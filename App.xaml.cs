using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace Sqlite_CRUD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
            //...Db Management extension logic is needed to be added
            DatabaseFacade facade = new DatabaseFacade(new DataContext());
            facade.EnsureCreated();
            
        }

    }
}
