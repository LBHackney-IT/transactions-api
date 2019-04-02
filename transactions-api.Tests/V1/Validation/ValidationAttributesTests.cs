using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using transactions_api.V1.Boundary;
using transactions_api.V1.Validation;

namespace transactions_api.Tests.V1.Validation
{
    [TestFixture]
    public class ValidationAttributesTests
    {
        [TestCase("01/01/2020", "01/01/2019")] //from date is grater than today
        [TestCase("03/04/1998","05/03/1996")] //to date is before from date
        [TestCase("03/04/1998", "05/03/2023")] //to date is greater than today
        public void ValidationShouldFailWhenInputsDontMatchCriteria(string fromDate, string toDate)
        { 
            var request = new ListTransactionsRequest()
            {
                TagRef = "tagRef",
                fromDate = DateTime.Parse(fromDate),
                toDate = DateTime.Parse(toDate)
            };
            var context = new ValidationContext(request);
            var isValid = Validator.TryValidateObject(request, context, new List<System.ComponentModel.DataAnnotations.ValidationResult>(), true);
            Assert.False(isValid);
        }

        [TestCase("01/01/2018", "01/01/2019")] 
        [TestCase("03/03/2019", "05/03/2019")] 
        public void ValidationShouldPassWhenInputMatchesCriteria(string fromDate, string toDate)
        {
            var request = new ListTransactionsRequest()
            {
                TagRef = "tagRef",
                fromDate = DateTime.Parse(fromDate),
                toDate = DateTime.Parse(toDate)
            };
            var context = new ValidationContext(request);
            var isValid = Validator.TryValidateObject(request, context, new List<System.ComponentModel.DataAnnotations.ValidationResult>(), true);
            Assert.True(isValid);
        }

        [Test]
        public void ModelShouldBeValidIfNoDatesArePassed()
        {
            var request = new ListTransactionsRequest()
            {
                TagRef = "tagRef"
            };
            var context = new ValidationContext(request);
            var isValid = Validator.TryValidateObject(request, context, new List<System.ComponentModel.DataAnnotations.ValidationResult>(), true);
            Assert.True(isValid);
        }

        [Test]
        public void ModelShouldbeValidIfNoToDateIsPassed()
        {
            var request = new ListTransactionsRequest()
            {
                TagRef = "tagRef",
                fromDate = DateTime.Today
            };
            var context = new ValidationContext(request);
            var isValid = Validator.TryValidateObject(request, context, new List<System.ComponentModel.DataAnnotations.ValidationResult>(), true);
            Assert.True(isValid);
        }

        [Test]
        public void ModelShouldBeValidIfNoFromDateIsPassed()
        {
            var request = new ListTransactionsRequest()
            {
                TagRef = "tagRef",
                toDate = DateTime.Today
            };
            var context = new ValidationContext(request);
            var isValid = Validator.TryValidateObject(request, context, new List<System.ComponentModel.DataAnnotations.ValidationResult>(), true);
            Assert.True(isValid);
        }
    }
}
