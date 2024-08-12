using System.ComponentModel.DataAnnotations;

namespace Sqlite_CRUD
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Age { get; set; }

        // Add to manage users' role
        public string RoleAsUser { get; set; }
        
        // Add to manage users status update
        public bool ActiveStatus { get; set; } = true;
    }
}
