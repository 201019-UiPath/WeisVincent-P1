namespace IceShopLib.Validation
{
    public class NotEmptyInputCondition : IInputCondition
    {
        public bool ValidateInput(string input)
        {
            if (input == null)
            {
                return false;
            }
            else if (input.Equals(""))
            {
                return false;
            }
            else return true;

        }
    }
}
