using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IceShopWeb.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        [DataType(DataType.Text, ErrorMessage = "A user's name should be just text.")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "A user's email should be a proper email address format.")]
        public string Email { get; set; }

        // TODO: Try using SecureString instead of string for user passwords.
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DisplayName("Address")]
        [DataType(DataType.Text, ErrorMessage = "A user's address should be just text.")]
        public string Address { get; set; }

    }
}
