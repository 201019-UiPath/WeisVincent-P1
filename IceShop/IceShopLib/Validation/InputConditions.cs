using System.Collections.Generic;

namespace IceShopLib.Validation
{
    public static class InputConditions
    {
        public static readonly List<IInputCondition> NameConditions = new List<IInputCondition>(2) {
            new NotEmptyInputCondition() , new IsTwoWordsCondition()
        };

        public static readonly List<IInputCondition> EmailConditions = new List<IInputCondition>(2) {
            new NotEmptyInputCondition() , new IsEmailCondition()
        };

        public static readonly List<IInputCondition> PasswordConditions = new List<IInputCondition>(2)
        {
            new NotEmptyInputCondition() , new IsValidPassword()
        };

        public static readonly List<IInputCondition> AddressConditions = new List<IInputCondition>(1)
        {
            new NotEmptyInputCondition() //TODO: Add more address conditions if possible
        };


    }
}
