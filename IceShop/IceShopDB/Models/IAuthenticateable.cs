namespace IceShopDB.Models
{
    internal interface IAuthenticateable
    {


        string Email { get; set; }

        string Password { get; set; }
    }
}
