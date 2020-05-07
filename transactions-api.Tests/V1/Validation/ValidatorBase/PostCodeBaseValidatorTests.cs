using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using transactions_api.V1.Validation.ValidatorBase;

namespace transactions_api.Tests.V1.Validation.ValidatorBase
{
    [TestFixture]
    public class PostCodeBaseValidatorTests
    {
        private IPostCodeBaseValidator _postCodeBaseValidator;

        [SetUp]
        public void SetUp()
        {
            _postCodeBaseValidator = new PostCodeBaseValidator();
        }

        #region Postcode format validation

        // Below explanations all use the postcodes IG11 7QD and E5 3XW 
        //"Incode" refers to the whole second part of the postcode (i.e. 3XW, 7QD) from (E11 3XW, W3 7QD)
        //"Outcode" refers to the whole first part of the postcode (Letter(s) and number(s) - i.e. IG11, E5) from (IG11 9LL, E5 2LL)
        //"Area" refers to the first letter(s) of the postcode (i.e.  IG, E) from (IG11 9LL, E5 2LL)
        //"District" refers to first number(s) to appear in the postcode (i.e. 11, 5) from (IG11 9LL, E5 2LL)
        //"Sector" refers to the number in the second part of the postcode (i.e. 9, 7) from (SW2 9DN, NE4 7JU)
        //"Unit" refers to the letters in the second part of the postcode (i.e. DN, JU) from (SW2 9DN, NE4 7JU)

        [TestCase("CR1 3ED")]
        public void GivenAPostCodeValueInUpperCase_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.True(validationResult);
        }

        [TestCase("w2 5jq")]
        public void GivenAPostCodeValueInLowerCase_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.True(validationResult);
        }

        [TestCase("w2 5JQ")]
        [TestCase("E11 5ra")]
        public void GivenAPostCodeValueInLowerCaseAndUpperCase_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.True(validationResult);
        }

        [TestCase("CR13ED")]
        [TestCase("RE15AD")]
        public void GivenPostCodeValueWithoutSpaces_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.True(validationResult);
        }

        [TestCase("NW")]
        [TestCase("E")]
        public void GivenOnlyAnAreaPartOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        [TestCase("17 9LL")]
        [TestCase("8 1LA")]
        public void GivenOnlyAnIncodeAndADistrictPartsOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        [TestCase("NW 9LL")]
        [TestCase("NR1LW")]
        public void GivenOnlyAnIncodeAndAnAreaPartsOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        [TestCase("1LL")]
        [TestCase(" 6BQ")]
        public void GivenOnlyAnIncodePartOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        [TestCase("E8 1LL")]
        [TestCase("SW17 1JK")]
        public void GivenBothPartsOfPostCode_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.True(validationResult);
        }

        [TestCase("IG117QDfdsfdsfd")]
        [TestCase("E1llolol")]
        public void GivenAValidPostcodeFolowedByRandomCharacters_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        [TestCase("EEE")]
        [TestCase("THE")]
        public void GivenThreeCharacters_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        [TestCase("N8 LL")]
        [TestCase("NW11 AE")]
        public void GivenAnOutcodeAndAUnit_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        [TestCase("S10 H")]
        [TestCase("W1 J")]
        public void GivenAnOutcodeAndOnlyOneLetterOfAUnit_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        [TestCase("w2 ")]
        [TestCase("E11 ")]
        public void GivenAnOutcodeWithSpace_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        [TestCase("E9")] //A9
        [TestCase("S5")]
        [TestCase("S11")] //A99
        [TestCase("W12")]
        [TestCase("NW9")] //AA9
        [TestCase("RH5")]
        [TestCase("SW17")] // AA99
        [TestCase("NE17")]
        [TestCase("W4R")] // A9A
        [TestCase("N1C")]
        [TestCase("NW1W")] // AA9A
        [TestCase("CR1H")]
        public void GivenOnlyAnOutcodePartOfPostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        [TestCase("SW11 9")]
        [TestCase("e14 2")]
        public void GivenAnOutcodeAndASector_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        [TestCase("SW7 9A")]
        [TestCase("n12 8F")]
        public void GivenAnOutcodeAndASectorAndTheFirstLetterOfTheUnit_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var validationResult = _postCodeBaseValidator.ValidatePostCodeFormat(postCode);

            Assert.False(validationResult);
        }

        #endregion
    }
}
