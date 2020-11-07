using IceShopDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace IceShopDB.Repos.FileRepos
{
    internal class ManagerFileRepo : IManagerRepo
    {
        private const string filepathManagers = "IceShopDB/SampleData/Managers.txt";

        public async void AddManager(Manager manager)
        {
            using (FileStream fs = File.Create(filepathManagers))
            {
                await JsonSerializer.SerializeAsync(fs, manager);
                Console.WriteLine("Customer is being written to file");
            }
        }

        public List<Manager> GetAllManagers()
        {
            throw new NotImplementedException();
        }

        public Task<List<Manager>> GetAllManagersAsync()
        {
            throw new NotImplementedException();
        }

        public Manager GetManagerByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Manager> GetManagerByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
