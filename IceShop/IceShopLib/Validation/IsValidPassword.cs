using System.Text.RegularExpressions;

namespace IceShopLib.Validation
{
    public class IsValidPassword : IInputCondition
    {
        // TODO: https://stackoverflow.com/questions/3466850/regular-expression-to-enforce-complex-passwords-matching-3-out-of-4-rules
        public bool ValidateInput(string input)
        {

            return Regex.IsMatch(input, "^\\w{8,}$");//TODO: Establish Regex to check valid password

        }
    }
}
