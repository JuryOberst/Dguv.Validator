using System;
using System.Globalization;
using System.Threading;

using Dguv.Validator.Checks;

using Xunit;

namespace Dguv.Validator.Tests
{
    public class CultureTests
    {
        [Theory]
        [InlineData(
            "de",
            "Der Wert von minLength darf nicht kleiner als 1 sein.",
            "Der Wert von maxLength darf nicht kleiner als 1 oder minLength sein.",
            "Es muss eine BBNR-UV angegeben werden.",
            "Es muss ein Name für eine BBNR-UV angegeben werden.")]
        [InlineData(
            "en",
            "The value of minLength must be not less than 1.",
            "The value of maxLength must be not less than 1 or minLength.",
            "A BBNR-UV must be specified.",
            "A name for a BBNR-UV must be specified.")]
        public void TestCharacterMapCheckExceptions(
            string cultureId,
            string errorMessageMinLength,
            string errorMessageMaxLength,
            string errorMessageBbnrUv,
            string errorMessageBbnrUvName)
        {
            const string bbnrUv = "15250094";
            const string bbnrUvName = "Verwaltungs-Berufsgenossenschaft";
            const int minLength = 10;
            const int maxLength = 10;
            const string validCharacters = "0123456789";

            var oldCulture = Thread.CurrentThread.CurrentCulture;
            var oldUiCulture = Thread.CurrentThread.CurrentUICulture;
            var newCulture = new CultureInfo(cultureId);
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
            try
            {
                Assert.Equal(
                    errorMessageMinLength,
                    Assert.Throws<ArgumentOutOfRangeException>("minLength", () => new CharacterMapCheck(bbnrUv, bbnrUvName, -2, maxLength, validCharacters)).GetFirstLine());
                Assert.Equal(
                    errorMessageMaxLength,
                    Assert.Throws<ArgumentOutOfRangeException>("maxLength", () => new CharacterMapCheck(bbnrUv, bbnrUvName, minLength, minLength - 1, validCharacters)).GetFirstLine());
                Assert.Equal(
                    errorMessageBbnrUv,
                    Assert.Throws<ArgumentOutOfRangeException>("bbnrUv", () => new CharacterMapCheck(string.Empty, bbnrUvName, minLength, maxLength, validCharacters)).GetFirstLine());
                Assert.Equal(
                    errorMessageBbnrUvName,
                    Assert.Throws<ArgumentOutOfRangeException>("name", () => new CharacterMapCheck(bbnrUv, string.Empty, minLength, maxLength, validCharacters)).GetFirstLine());
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = oldCulture;
                Thread.CurrentThread.CurrentUICulture = oldUiCulture;
            }
        }
    }
}
