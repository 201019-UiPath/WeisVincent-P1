using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceShopDB.Models
{
    public abstract class User
    {

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        [Key]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        // TODO: Try using SecureString instead of string for user passwords.
        [Column("password")]
        public string Password { get; set; }


    }
}
