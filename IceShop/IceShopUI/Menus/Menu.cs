using IceShopBL;
using IceShopDB.Repos;
using IceShopLib.Validation;
using System;
using System.Collections.Generic;
using Xunit.Sdk;

namespace IceShopUI.Menus
{
    /// <summary>
    /// This class represents each menu in the system, using a Run() method to execute the 
    /// SetStartingMessage, SetUserChoices, Start, QueryUserChoice, and ExecuteUserChoice methods in that order.
    /// Each menu has a starting message to the user, lists off their choices, and takes in user input in the form of a number
    /// to determine which choice the user made.
    /// </summary>
    public abstract class Menu : IMenu
    {

        protected IRepository Repo;

        public Menu(ref IRepository repo)
        {
            Repo = repo;
        }


        public string StartMessage;

        public abstract void SetStartingMessage();

        public abstract void SetUserChoices();

        protected List<string> possibleOptions;
        public List<string> PossibleOptions
        {
            get
            {
                if (possibleOptions != null) return possibleOptions; else throw new NotNullException();
            }
            set
            {
                possibleOptions = value;
            }
        }


        public int selectedChoice;



        public void Run()
        {
            SetStartingMessage();
            SetUserChoices();
            Start();
            QueryUserChoice();
            ExecuteUserChoice();
        }



        public void Start()
        {
            Console.WriteLine();
            UserResponseUtility.DisplayPossibleChoicesToUser(StartMessage, PossibleOptions);
        }

        /// <summary>
        /// This method handles each interaction the user has with the menu.
        /// </summary>
        public void QueryUserChoice()
        {
            selectedChoice = UserRequestUtility.ProcessUserInputAgainstPossibleChoices(PossibleOptions);
            Console.WriteLine();
        }


        public abstract void ExecuteUserChoice();

    }
}
