using IceShopBL;
using IceShopDB.Models;
using IceShopDB.Repos;
using IceShopUI.Menus.ManagerMenus;
using System;
using System.Collections.Generic;

namespace IceShopUI.Menus
{
    public class SignUpMenu : Menu, IMenu
    {
        private readonly IMenu loginMenu;

        

        public SignUpMenu(ref IRepository repo) : base(ref repo)
        {
            loginMenu = new LoginMenu(ref Repo);
            
        }

        public override void SetStartingMessage()
        {
            StartMessage = "Are you signing up as a Customer or Manager?";
        }

        public override void SetUserChoices()
        {
            PossibleOptions = new List<string>() {
                "Customer",
                "Manager"
            };
        }




        public override void ExecuteUserChoice()
        {
            // Sign Up functionality
            StartService startService = new StartService(ref Repo);
            // 1: Ask for email
            string newEmail = UserRequestUtility.QueryEmail();

            // 2: check if email matches with any other customer
            if (startService.DoesUserExistWithEmail(newEmail))
            {
                Console.WriteLine("That email already exists for a user of the program. You really should be logging in instead. \n Taking you back to the start..");
                MenuManager.Instance.ReadyNextMenu(new StartMenu(ref Repo));
                return;
            }

            // 3: ask for password, then name and address
            string newPassword = UserRequestUtility.QueryPasswordAndConfirmation();

            string newName = UserRequestUtility.QueryName();

            // When SignUp() ends, add the new customer data to DB/file
            switch (selectedChoice)
            {
                case 1:
                    // Update a database with an added customer using BL.
                    string newAddress = UserRequestUtility.QueryAddress();

                    Customer newCustomer = new Customer(newName, newEmail, newPassword, newAddress);

                    CustomerService customerService = new CustomerService(ref Repo);
                    customerService.AddCustomerToRepo(newCustomer);
                    break;
                case 2:
                    Manager newManager = new Manager(newName, newEmail, newPassword);

                    ManagerSignUpSubMenu managerSignUpMenu = new ManagerSignUpSubMenu(ref Repo, ref newManager);
                    Manager updatedManager = managerSignUpMenu.RunAndReturnManagerWithSelectedLocation();

                    ManagerService managerService = new ManagerService(ref Repo);
                    managerService.AddManager(updatedManager);

                    break;
                default:
                    throw new NotImplementedException();
                    //break;
            }
            Console.WriteLine("You've now signed up! Now type all that garbage again to login!");
            MenuManager.Instance.ReadyNextMenu(loginMenu);

        }


    }
}
