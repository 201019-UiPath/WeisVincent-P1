using System.Text.RegularExpressions;

namespace IceShopLib.Validation
{
    public class IsEmailCondition : IInputCondition
    {
        public bool ValidateInput(string input)
        {
            return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        }
    }
}
