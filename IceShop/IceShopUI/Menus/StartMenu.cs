using IceShopDB.Repos;
using System;
using System.Collections.Generic;

namespace IceShopUI.Menus
{

    /// <summary>
    /// This is the starting menu for the program, implementing the IMenu interface in order to enforce menu structure.
    /// </summary> 
    public sealed class StartMenu : Menu, IMenu
    {

        private readonly IMenu signupMenu;
        private readonly IMenu loginMenu;

        public StartMenu(ref IRepository repo) : base(ref repo)
        {
            signupMenu = new SignUpMenu(ref Repo);
            loginMenu = new LoginMenu(ref Repo);
        }


        #region Set up menu options
        public override void SetStartingMessage()
        {
            StartMessage = "Welcome! Please select which operation you'd like to perform!";
        }

        public override void SetUserChoices()
        {
            PossibleOptions = new List<string>() {
                "Sign-up for a new account.",
                "Log-in to an existing account."
            };
        }
        #endregion

        public override void ExecuteUserChoice()
        {
            switch (selectedChoice)
            {
                case 1:
                    MenuManager.Instance.ReadyNextMenu(signupMenu);
                    break;
                case 2:
                    MenuManager.Instance.ReadyNextMenu(loginMenu);
                    break;
                default:
                    throw new NotImplementedException();
                    //break;
            }
        }




    }
}