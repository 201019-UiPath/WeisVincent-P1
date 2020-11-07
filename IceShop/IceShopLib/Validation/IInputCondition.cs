namespace IceShopLib.Validation
{
    /// <summary>
    /// This interface establishes a convention for input condition classes in the same namespace 
    /// that allow the modularization of individual string validations utilized by the UserInputValidator class.
    /// This enforces the Dependency Inversion Principle by separating the low-level validations from the high-level module using them.
    /// </summary>
    public interface IInputCondition
    {
        /// <summary>
        /// This method should implement input validation that is unique for each individual class that implements this interface.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool ValidateInput(string input);

    }
}
