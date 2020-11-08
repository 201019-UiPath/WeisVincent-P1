using IceShopDB.Models;
using System;

namespace IceShopBL
{
    public interface IStartService
    {
        bool CheckIfCustomer(User user);
        bool CheckIfManager(User user);
        bool DoesUserExistWithEmail(string email);
        Type GetTypeOfUser(User user);
        User GetUserByEmail(string email);
    }
}