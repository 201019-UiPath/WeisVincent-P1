using IceShopLib.Validation;
using System.Data;
using Xunit;

namespace IceShopTest
{
    public class InputValidatorTest
    {
        #region One Digit Condition tests (IsOneDigitCondition)
        [Theory]
        [InlineData("1")]
        [InlineData("3")]
        public void OneDigitInputShouldReturnTrue(string input)
        {
            InputValidator validator = new InputValidator(new IsOneDigitCondition());

            Assert.True(validator.ValidateInput(input));

        }

        [Theory]
        [InlineData("12")]
        [InlineData("345")]
        public void NotOneDigitInputShouldReturnFalse(string input)
        {
            InputValidator validator = new InputValidator(new IsOneDigitCondition());

            Assert.False(validator.ValidateInput(input));

        }
        #endregion

        #region One or Two Digit Condition tests (IsOneOrTwoDigitsCondition)
        [Theory]
        [InlineData("12")]
        [InlineData("3")]
        public void TwoOrOneDigitInputShouldReturnTrue(string input)
        {
            InputValidator validator = new InputValidator(new IsOneOrTwoDigitsCondition());

            Assert.True(validator.ValidateInput(input));

        }

        [Theory]
        [InlineData("12345")]
        [InlineData("678")]
        public void MoreThanTwoDigitInputShouldReturnFalse(string input)
        {
            InputValidator validator = new InputValidator(new IsOneOrTwoDigitsCondition());

            Assert.False(validator.ValidateInput(input));

        }
        #endregion

        #region Two Words Condition tests (IsTwoWordsCondition)
        [Theory]
        [InlineData("Yes Mr")]
        [InlineData("Mr Moist")]
        public void TwoWordInputShouldReturnTrue(string input)
        {
            InputValidator validator = new InputValidator(new IsTwoWordsCondition());

            Assert.True(validator.ValidateInput(input));
        }

        [Theory]
        [InlineData("Yes sir ree")]
        [InlineData("ILostMySpaceBar")]
        public void NotTwoWordInputShouldReturnFalse(string input)
        {
            InputValidator validator = new InputValidator(new IsTwoWordsCondition());

            Assert.False(validator.ValidateInput(input));
        }
        #endregion

        #region Empty/Null Condition tests (NotEmptyInputCondition)
        [Theory]
        [InlineData("some")]
        [InlineData("body once told me")]
        public void NotEmptyInputShouldReturnTrue(string input)
        {
            InputValidator validator = new InputValidator(new NotEmptyInputCondition());

            Assert.True(validator.ValidateInput(input));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NullOrEmptyInputShouldReturnFalse(string input)
        {
            InputValidator validator = new InputValidator(new NotEmptyInputCondition());

            Assert.False(validator.ValidateInput(input));
        }
        #endregion

        #region Email Condition tests (IsEmailCondition)
        [Theory]
        [InlineData("email@email.xyz")]
        [InlineData("I.Am.Ironman@gmail.com")]
        public void EmailInputShouldReturnTrue(string input)
        {
            InputValidator validator = new InputValidator(new IsEmailCondition());

            Assert.True(validator.ValidateInput(input));
        }

        [Theory]
        [InlineData("em@ail@email.com")]
        [InlineData("I.Am.Ironman@g@mail.com")]
        [InlineData("Vincent.Weis")]
        public void NotEmailInputShouldReturnFalse(string input)
        {
            InputValidator validator = new InputValidator(new IsEmailCondition());

            Assert.False(validator.ValidateInput(input));
        }
        #endregion


        [Fact]
        public void InputValidatorWithNullInputConditionsShouldThrowNoNullArgumentException()
        {
            InputValidator validator = new InputValidator(new IsEmailCondition());


            Assert.Throws<NoNullAllowedException>(() => validator.InputConditions = null);
        }



        #region Valid Password Test
        [Theory]
        [InlineData("password")]
        [InlineData("WordOfthepasSVariety1")]
        public void EightWordCharactersInputShouldReturnTrue(string input)
        {
            InputValidator validator = new InputValidator(new IsValidPassword());

            Assert.True(validator.ValidateInput(input));

        }

        [Theory]
        [InlineData("12345")]
        [InlineData("678444 3")]
        public void LessThanEightCharacterInputShouldReturnFalse(string input)
        {
            InputValidator validator = new InputValidator(new IsValidPassword());

            Assert.False(validator.ValidateInput(input));

        }


        #endregion

    }
}
