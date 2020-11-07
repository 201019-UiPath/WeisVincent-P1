using IceShopBL;
using IceShopDB.Models;
using IceShopDB.Repos;
using IceShopUI.Menus.CustomerMenus;
using IceShopUI.Menus.ManagerMenus;
using System;
using System.Collections.Generic;

namespace IceShopUI.Menus
{
    public sealed class LoginMenu : Menu, IMenu
    {

        public LoginMenu(ref IRepository repo) : base(ref repo) { }

        public override void SetStartingMessage()
        {
            StartMessage = "Are you logging in as a Customer or Manager?";
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
            switch (selectedChoice)
            {
                case 1:
                    LoginAsCustomer();
                    break;
                case 2:
                    LoginAsManager();
                    break;
                default:
                    throw new NotImplementedException();
                    //break;
            }


        }






        public void LoginAsCustomer()
        {
            CustomerService customerService = new CustomerService(ref Repo);
            Customer userLoggingIn;


            Console.WriteLine("Please put in your email, then your password to login.");

            userLoggingIn = customerService.GetCustomerByEmail(UserRequestUtility.QueryEmail());


            if (userLoggingIn is null)
            {
                Console.WriteLine("Couldn't find a customer matching that email. You can try again if you want, that'll be fun.");
                LoginMenu loginMenuButAgain = new LoginMenu(ref Repo);
                MenuManager.Instance.ReadyNextMenu(loginMenuButAgain);
                return;
            }
            else if (userLoggingIn is Customer)
            {
                string userInputPassword;
                userInputPassword = UserRequestUtility.QueryPassword();
                while (userInputPassword != userLoggingIn.Password)
                {
                    Console.WriteLine("Wrong password. You can try again, though. Why not?");
                    userInputPassword = UserRequestUtility.QueryPassword();
                }
            }


            // TODO: Move to Customer menu.
            CustomerStartMenu customerMenu = new CustomerStartMenu(userLoggingIn, ref Repo);
            MenuManager.Instance.ReadyNextMenu(customerMenu);
        }

        public void LoginAsManager()
        {
            ManagerService managerService = new ManagerService(ref Repo);

            Manager managerLoggingIn;

            Console.WriteLine("Please put in your email, then your password to login.");


            managerLoggingIn = managerService.GetManagerByEmail(UserRequestUtility.QueryEmail());

            string userInputPassword;

            if (managerLoggingIn is null)
            {
                Console.WriteLine("Couldn't find a manager matching that email. You can try again if you want, that'll be fun.");
                LoginMenu loginMenuButAgain = new LoginMenu(ref Repo);
                MenuManager.Instance.ReadyNextMenu(loginMenuButAgain);
            }
            else if (managerLoggingIn is Manager)
            {
                userInputPassword = UserRequestUtility.QueryPassword();
                while (userInputPassword != managerLoggingIn.Password)
                {
                    Console.WriteLine("Wrong password. You can try again, though. Why not?");
                    userInputPassword = UserRequestUtility.QueryPassword();
                }

                // TODO: Move to Manager menu.
                ManagerStartMenu managerMenu = new ManagerStartMenu(managerLoggingIn, ref Repo);
                MenuManager.Instance.ReadyNextMenu(managerMenu);

            }

        }




        public void LoginAsUser()
        {

            StartService startService = new StartService(ref Repo);


            User userLoggingIn;

            // TODO: Implement Login functionality for existing users

            Console.WriteLine("Please put in your email, then your password to login.");


            userLoggingIn = startService.GetUserByEmail(UserRequestUtility.QueryEmail());

            if (userLoggingIn is null)
            {
                Console.WriteLine("Couldn't find a user matching that email. You can try again if you want, that'll be fun.");
                LoginMenu loginMenuButAgain = new LoginMenu(ref Repo);
                MenuManager.Instance.ReadyNextMenu(loginMenuButAgain);
            }
            else if (userLoggingIn is Customer || userLoggingIn is Manager)
            {
                string userInputPassword = UserRequestUtility.QueryPassword();
                while (userInputPassword != userLoggingIn.Password)
                {
                    userInputPassword = UserRequestUtility.QueryPassword();
                }
            }



            // TODO: When SignUp() ends, add the new customer data to DB/file
            switch (selectedChoice)
            {
                case 1:
                    //TODO: Update a database with an added customer using BL.

                    break;
                case 2:
                    //TODO: Update a database with an added manager using BL.

                    break;
                default:
                    throw new NotImplementedException();
                    //break;
            }

            //TODO: Check at Login() if the inputted email and password match any existing customer or Manager, then make the current user either customer or manager.

            // TODO: Move to next menu.


        }

    }
}
