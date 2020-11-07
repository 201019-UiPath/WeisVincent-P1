using IceShopBL;
using IceShopLib.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceShopUI.Menus
{
    internal static class UserRequestUtility
    {

        #region User query and input methods



        public static string QueryName()
        {

            #region Ask User for Name

            Console.WriteLine("Enter your name:");
            string inputName = Console.ReadLine().Trim();
            InputValidator validator = new InputValidator(InputConditions.NameConditions);
            while (!validator.ValidateInput(inputName))
            {
                Console.WriteLine("Your input must be a valid name.");
                Console.WriteLine("Enter your name:");
                inputName = Console.ReadLine().Trim();
            }

            return inputName;

            #endregion
        }

        public static string QueryProductName()
        {

            #region Ask User for Name

            Console.WriteLine("Enter a name for the Product:");
            string inputName = Console.ReadLine().Trim();

            // TODO: Refine conditions for Product names.
            InputValidator validator = new InputValidator(new NotEmptyInputCondition());
            while (!validator.ValidateInput(inputName))
            {
                Console.WriteLine("Your input must be a valid product name.");
                Console.WriteLine("Enter a name for the Product:");
                inputName = Console.ReadLine().Trim();
            }

            return inputName;

            #endregion
        }


        public static string QueryEmail()
        {

            #region Ask User for email

            Console.WriteLine("Enter your email:");
            string inputEmail = Console.ReadLine().Trim();
            InputValidator validator = new InputValidator(InputConditions.EmailConditions);
            while (!validator.ValidateInput(inputEmail))
            {
                Console.WriteLine("Your input must be in email format.");
                Console.WriteLine("Enter your email:");
                inputEmail = Console.ReadLine().Trim();
            }

            return inputEmail;

            #endregion
        }


        public static string QueryPasswordAndConfirmation()
        {
            string newPassword = QueryPassword();
            Console.WriteLine("Enter the same password to prove to my satisfaction that you can type.");
            string confirmationPassword = QueryPassword();
            while (newPassword != confirmationPassword)
            {
                Console.WriteLine("Your confirmation password did not match. I'm telling mom. \n Try again.");
                newPassword = QueryPassword();
                confirmationPassword = QueryPassword();
            }

            Console.WriteLine("Your password has been confirmed!");
            return newPassword;
        }

        public static string QueryPassword()
        {
            #region Ask User for Password

            Console.WriteLine("Enter your password:");
            string inputPassword = Console.ReadLine().Trim();

            InputValidator validator = new InputValidator(InputConditions.PasswordConditions);

            while (!validator.ValidateInput(inputPassword))
            {
                Console.WriteLine("Your input must be non-empty, with at least 8 word characters.");
                Console.WriteLine("Enter your password:");
                inputPassword = Console.ReadLine().Trim();
            }

            #endregion

            return inputPassword;

        }

        public static string QueryAddress()
        {

            #region Ask User for address

            Console.WriteLine("Enter your address:");
            string inputAddress = Console.ReadLine().Trim();
            InputValidator validator = new InputValidator(InputConditions.AddressConditions);
            while (!validator.ValidateInput(inputAddress))
            {
                Console.WriteLine("Your input must be a valid address.");
                Console.WriteLine("Enter your name:");
                inputAddress = Console.ReadLine().Trim();
            }

            return inputAddress;

            #endregion
        }


        internal static int ProcessUserInputAgainstPossibleChoices(List<string> possibleOptions)
        {
            IInputCondition condition;
            if (possibleOptions.Count <= 9)
            {
                condition = new IsOneDigitCondition();
            }
            else
            {
                condition = new IsOneOrTwoDigitsCondition();
            }

            InputValidator validator = new InputValidator(condition);

            bool userInputIsInRange = false;
            do
            {
                string userInput = Console.ReadLine().Trim();
                while (!validator.ValidateInput(userInput))
                {
                    Console.WriteLine("That input wasn't it, sufferer. Give it another go, it needs to be a whole number.");
                    userInput = Console.ReadLine().Trim();
                }

                int userChoice = int.Parse(userInput);
                if (userChoice < 1 || userChoice > possibleOptions.Count)
                {
                    Console.WriteLine("That input wasn't it, sufferer. Give it another go, it needs to be one of the offered choices.");
                    continue;
                }
                else
                {
                    userInputIsInRange = true;
                    return userChoice;
                }

            } while (!userInputIsInRange);
            throw new Exception("User input was processed incorrectly, validated a false input.");
        }

        internal static string QueryDescription()
        {
            Console.WriteLine("Enter your description:");
            string inputDescription = Console.ReadLine().Trim();

            InputValidator validator = new InputValidator(new NotEmptyInputCondition());
            const int maxLength = 140;
            while (!validator.ValidateInput(inputDescription) || inputDescription.Length > maxLength)
            {
                Console.WriteLine($"Your input must be a valid description, no more than {maxLength} characters.");
                Console.WriteLine("Enter your description:");
                inputDescription = Console.ReadLine().Trim();
            }

            return inputDescription;
        }

        internal static int QueryQuantity()
        {
            IInputCondition condition;
            condition = new IsOneOrTwoDigitsCondition();

            InputValidator validator = new InputValidator(condition);

            bool userInputIsInRange = false;
            do
            {
                string userInput = Console.ReadLine().Trim();
                while (!validator.ValidateInput(userInput))
                {
                    Console.WriteLine("That input wasn't it, sufferer. Give it another go, it needs to be a whole number.");
                    userInput = Console.ReadLine().Trim();
                }

                int userChoice = int.Parse(userInput);
                if (userChoice < 1 && userChoice > 80)
                {
                    Console.WriteLine("That input wasn't it, sufferer. Give it another go, it needs to be between 0 and 50.");
                    continue;
                }
                else
                {
                    userInputIsInRange = true;
                    return userChoice;
                }
            } while (!userInputIsInRange);
            throw new Exception("User input was processed incorrectly, validated a false input.");
        }

        #endregion

    }
}
