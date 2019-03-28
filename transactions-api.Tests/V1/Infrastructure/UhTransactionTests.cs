using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NUnit.Framework;
using transactions_api.V1.Domain;
using UnitTests.V1.Helper;

namespace UnitTests.V1.Infrastructure
{
    [TestFixture]
    public class UhTransactionTests
    {
        [Test]
        public void canBeEqual()
        {
            UhTransaction a = UhTransactionHelper.CreateUhTransaction();
            UhTransaction b = new UhTransaction
            {
                PropRef = a.PropRef,
                TagRef = a.TagRef,
                Amount = a.Amount,
                Code = a.Code,
                Date = a.Date,
                FinancialYear = a.FinancialYear,
                PeriodNumber = a.PeriodNumber,
                Comments = a.Comments,
                Id = a.Id,
                batchno = a.batchno,
                transno = a.transno,
                line_no = a.line_no,
                adjustment = a.adjustment,
                apportion = a.apportion,
                prop_deb = a.prop_deb,
                none_rent = a.none_rent,
                receipted = a.receipted
            };

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
            Assert.AreEqual(a, b);
        }

    }
 }
