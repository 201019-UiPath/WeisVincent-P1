using System.Text.RegularExpressions;

namespace IceShopLib.Validation
{
    public class IsTwoWordsCondition : IInputCondition
    {
        public bool ValidateInput(string input)
        {
            if (Regex.IsMatch(input, "^\\w+\\s{1}\\w+$")) // TODO: Make this not return true if the input takes a number (replace \w)
            {
                return true;
            }
            return false;

        }
    }
}
