using IceShopDB.Models;
using System.Collections.Generic;

namespace IceShopBL
{
    public interface IManagerService
    {
        void AddManager(Manager newManager);
        List<Manager> GetAllManagers();
        Manager GetManagerByEmail(string newEmail);
    }
}