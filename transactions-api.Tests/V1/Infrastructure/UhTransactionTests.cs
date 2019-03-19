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
                Code = a.Code,
                Date = a.Date,
                Id = a.Id,
                batchno = a.batchno,
                transno = a.transno,
                line_no = a.line_no,
                adjustment = a.adjustment,
                apportion = a.apportion,
                prop_deb = a.prop_deb,
                none_rent = a.none_rent,
                receipted = a.receipted,
                line_segno = a.line_segno,
                tag_ref = a.tag_ref,
                real_value = a.real_value,
                full_value = a.full_value
            };

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
            Assert.AreEqual(a, b);
        }

    }
 }
