using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IceShopWeb.Models
{
    public class User
    {
        internal User()
        {
            
        }

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        [DataType(DataType.Text, ErrorMessage = "A user's name should be just text.")]
        public string Name { get; set; }

        [Required]
        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "A user's email should be a proper email address format.")]
        public string Email { get; set; }

        // TODO: Try using SecureString instead of string for user passwords.
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password, ErrorMessage = "A user needs a password that lets them pass by typing a word.")]
        public string Password { get; set; }


    }
}
