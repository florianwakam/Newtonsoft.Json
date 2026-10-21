using System;
using Newtonsoft.Json.Linq;
#if DNXCORE50
using Xunit;
using Test = Xunit.FactAttribute;
using Assert = Newtonsoft.Json.Tests.XUnitAssert;
#else
using NUnit.Framework;
#endif

namespace Newtonsoft.Json.Tests.Issues
{
    [TestFixture]
    public class Issue3136
    {
        [Test]
        public void ToObject_DoubleValue_ReturnsDecimalWrittenInJson()
        {
            JToken token = JObject.Parse(@"{""rate"":0.1}")["rate"];

            Assert.AreEqual(0.1m, token.ToObject<decimal>());
            Assert.AreEqual(0.1m, token.ToObject<decimal?>());
        }

        [Test]
        public void ToObject_ObjectWithDecimalProperty_ReturnsDecimalWrittenInJson()
        {
            JObject o = JObject.Parse(@"{""Rate"":2.675}");

            Assert.AreEqual(2.675m, o.ToObject<Rated>().Rate);
        }

        [Test]
        public void ExplicitConversion_DoubleValue_ReturnsDecimalWrittenInJson()
        {
            JToken token = JObject.Parse(@"{""rate"":1E-7}")["rate"];

            Assert.AreEqual(0.0000001m, (decimal)token);
            Assert.AreEqual(0.0000001m, (decimal?)token);
        }

        [Test]
        public void ExplicitConversion_ComputedDouble_RoundsToFifteenSignificantDigits()
        {
            Assert.AreEqual(0.3m, (decimal)new JValue(0.1d + 0.2d));
        }

        [Test]
        public void ExplicitConversion_DoubleOutsideDecimalRange_Throws()
        {
            ExceptionAssert.Throws<OverflowException>(() => { decimal _ = (decimal)new JValue(double.MaxValue); });
        }

        public class Rated
        {
            public decimal Rate { get; set; }
        }
    }
}
