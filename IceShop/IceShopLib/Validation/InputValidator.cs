using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using PresetConditions = IceShopLib.Validation.InputConditions;

namespace IceShopLib.Validation
{
    /// <summary>
    /// This is an Input Validation class designed to work with any condition or number of conditions, defined in classes that implement IInputCondition.
    /// This class can be instantiated with any number of conditions greater than 0, and can then be used with the ValidateInput() method to validate a string against those conditions.
    /// </summary>
    public sealed class InputValidator
    {

        public InputValidator() : this(new NotEmptyInputCondition()) { }

        public InputValidator(List<IInputCondition> conditions)
        {
            InputConditions = conditions;
            Input = null;
        }

        public InputValidator(IInputCondition condition)
        {
            if (condition == null)
            {
                throw new Exception("No null elements allowed for the InputConditions in the InputValidator class.");
            }
            InputConditions = new List<IInputCondition>(1) { condition };
            Input = null;
        }

        public InputValidator(string input, List<IInputCondition> conditions)
        {
            InputConditions = conditions;
            Input = input;
        }


        public InputValidator(string input, IInputCondition condition)
        {
            if (condition == null)
            {
                throw new Exception("No null elements allowed for the InputConditions in the InputValidator class.");
            }
            InputConditions = new List<IInputCondition>(1) { condition };
            Input = input;
        }

        private List<IInputCondition> inputConditions;
        public List<IInputCondition> InputConditions
        {
            get
            {
                return inputConditions;
            }
            set
            {
                if (value == null)
                {
                    throw new NoNullAllowedException();
                }
                else
                {
                    inputConditions = value;
                }
            }
        }



        public string Input { get; set; }

        public bool ValidateInput(string input)
        {
            Input = input;
            foreach (IInputCondition condition in InputConditions)
            {
                if (!condition.ValidateInput(Input))
                {
                    return false;
                }
            }

            return true;
        }

        


        #region Input Validation Static Methods
        public static bool ValidateEmailInput(string email)
        {
            Log.Logger.Information("Validating the input of an email..");
            InputValidator inputValidator = new InputValidator(PresetConditions.EmailConditions);
            return inputValidator.ValidateInput(email);
        }

        public static bool ValidatePasswordInput(string password)
        {
            Log.Logger.Information("Validating the input of a password..");
            InputValidator inputValidator = new InputValidator(PresetConditions.PasswordConditions);
            return inputValidator.ValidateInput(password);
        }

        public static bool ValidateAddressInput(string address)
        {
            Log.Logger.Information("Validating the input of an address..");
            InputValidator inputValidator = new InputValidator(PresetConditions.AddressConditions);
            return inputValidator.ValidateInput(address);
        }

        #endregion


    }
}
