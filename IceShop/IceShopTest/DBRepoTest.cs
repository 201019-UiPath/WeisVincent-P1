using IceShopDB;
using IceShopDB.Models;
using IceShopDB.Repos;
using IceShopDB.Repos.DBRepos;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace IceShopTest
{
    public class DBRepoTest
    {

        private DBRepo repo;

        private readonly Customer testCustomer = new Customer("Mister Sample", "sample.email@sample.email", "samplepassword", "Sample Place");

        private readonly Manager testManager = new Manager("Master Sample", "really.cool@manager.email", "samplepassword") { Id = -50, LocationId = -2 };

        [Fact]
        public void AddCustomerShouldAdd()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<IceShopContext>().UseInMemoryDatabase("AddCustomerShouldAdd").Options;
            using var testContext = new IceShopContext(options);
            repo = new DBRepo(testContext);

            //Act
            repo.AddCustomer(testCustomer);
            repo.SaveChanges();

            //Assert
            using var assertContext = new IceShopContext(options);
            Assert.NotNull(assertContext.Customers.Single(c => c.Id == testCustomer.Id));
        }


        [Fact]
        public void GetCustomerByEmailShouldGetCustomer()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<IceShopContext>().UseInMemoryDatabase("GetCustomerByEmailShouldGetCustomer").Options;
            using var testContext = new IceShopContext(options);
            repo = new DBRepo(testContext);

            //Act
            repo.AddCustomer(testCustomer);
            repo.SaveChanges();
            Customer fetchedTestCustomer = repo.GetCustomerByEmail(testCustomer.Email);

            //Assert
            using var assertContext = new IceShopContext(options);

            Assert.NotNull(assertContext.Customers.Single(c => c.Id == fetchedTestCustomer.Id));
        }


        [Fact]
        public void AddManagerShouldAdd()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<IceShopContext>().UseInMemoryDatabase("AddManagerShouldAdd").Options;
            using var testContext = new IceShopContext(options);
            repo = new DBRepo(testContext);

            //Act
            repo.AddManager(testManager);
            repo.SaveChanges();

            //Assert
            using var assertContext = new IceShopContext(options);
            Assert.NotNull(assertContext.Managers.Single(m => m.Id == testManager.Id));
        }

        [Fact]
        public void GetManagerByEmailShouldGetManager()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<IceShopContext>().UseInMemoryDatabase("GetManagerByEmailShouldGetManager").Options;
            using var testContext = new IceShopContext(options);
            repo = new DBRepo(testContext);
            testContext.Locations.AddRange(SampleData.GetSampleLocations());

            //Act
            repo.AddManager(testManager);
            repo.SaveChanges();


            Manager fetchedTestManager = repo.GetManagerByEmail(testManager.Email);

            //Assert
            //using var assertContext = new IceShopContext(options);
            Assert.NotNull(testContext.Managers.Single(m => m.Id == fetchedTestManager.Id));
        }

        [Fact]
        public void UpdateInventoryLineItemShouldUpdateLineItem()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<IceShopContext>().UseInMemoryDatabase("UpdateInventoryLineItemShouldUpdateLineItem").Options;
            using var testContext = new IceShopContext(options);
            // add sample data to testContext
            testContext.InventoryLineItems.AddRange(SampleData.GetSampleInventoryLineItems());
            // This data needs to be added so that navigation properties of the Inventory Line Items aren't null.
            testContext.Locations.AddRange(SampleData.GetSampleLocations());
            testContext.Products.AddRange(SampleData.GetSampleProducts());
            testContext.SaveChanges();

            repo = new DBRepo(testContext);

            //Act
            // get a location that has an InventoryLineItem
            int sampleLocationId = repo.GetAllLocations().First().Id;
            InventoryLineItem sampleLineItem = repo.GetAllInventoryLineItemsAtLocation(sampleLocationId).FindLast(ili => ili.LocationId == sampleLocationId);
            int startingQuantity = sampleLineItem.ProductQuantity;
            sampleLineItem.ProductQuantity += 1;


            repo.UpdateInventoryLineItem(sampleLineItem);
            repo.SaveChanges();

            //Assert
            //using var assertContext = new IceShopContext(options);
            Assert.NotEqual(startingQuantity, sampleLineItem.ProductQuantity);
        }

    }
}
