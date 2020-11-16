using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IceShopWeb.Models
{
    public class AuthPack
    {

        public AuthPack()
        {

        }

        public AuthPack(string email, string password)
        {
            Email = email;
            Password = password;
        }


        [Required]
        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "A user's email should be a proper email address format.")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password, ErrorMessage = "A user password must be a password.")]
        public string Password { get; set; }
    }
}
