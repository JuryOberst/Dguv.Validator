using System;

using Xunit;

namespace Dguv.Validator.Tests
{
    public class ValidationTests
    {
        private static readonly IDguvValidator _validator = new DguvValidator();

        [Theory]
        [InlineData("15250094", "1234567890")]
        public void TestSuccess1(string bbnrUv, string memberId)
        {
            Assert.True(_validator.IsValid(bbnrUv, memberId));
            Assert.Null(_validator.GetStatus(bbnrUv, memberId));
            _validator.Validate(bbnrUv, memberId);
        }

        [Theory]
        [InlineData("15250094", "123456789")]
        [UseCulture("de")]
        public void TestTooShort(string bbnrUv, string memberId)
        {
            const string errorMessage = "Die Mitgliedsnummer ist zu kurz. Sie muss eine Länge von mindestens 10 Zeichen haben.";
            Assert.False(_validator.IsValid(bbnrUv, memberId));
            Assert.Equal(errorMessage, _validator.GetStatus(bbnrUv, memberId));
            Assert.Equal(errorMessage, Assert.Throws<DguvValidationException>(() => _validator.Validate(bbnrUv, memberId)).GetFirstLine());
        }

        [Theory]
        [InlineData("15250094", "12345678901")]
        [UseCulture("de")]
        public void TestTooLong(string bbnrUv, string memberId)
        {
            const string errorMessage = "Die Mitgliedsnummer ist zu lang. Sie darf höchstens eine Länge von 10 Zeichen haben.";
            Assert.False(_validator.IsValid(bbnrUv, memberId));
            Assert.Equal(errorMessage, _validator.GetStatus(bbnrUv, memberId));
            Assert.Equal(errorMessage, Assert.Throws<DguvValidationException>(() => _validator.Validate(bbnrUv, memberId)).GetFirstLine());
        }

        [Theory]
        [InlineData("15250094", "12345678X0")]
        [UseCulture("de")]
        public void TestInvalidCharacter(string bbnrUv, string memberId)
        {
            const string errorMessage = "Die Mitgliedsnummer enthält ein oder mehrere ungültige Zeichen. Erlaubt sind nur folgende Zeichen: \"0123456789\"";
            Assert.False(_validator.IsValid(bbnrUv, memberId));
            Assert.Equal(errorMessage, _validator.GetStatus(bbnrUv, memberId));
            Assert.Equal(errorMessage, Assert.Throws<DguvValidationException>(() => _validator.Validate(bbnrUv, memberId)).GetFirstLine());
        }

        [Fact]
        [UseCulture("de")]
        public void TestMissingMemberId()
        {
            const string bbnrUv = "15250094";
            const string errorMessage = "Es muss eine Mitgliedsnummer angegeben werden.";
            const string memberId = "";
            Assert.False(_validator.IsValid(bbnrUv, memberId));
            Assert.Equal(errorMessage, _validator.GetStatus(bbnrUv, memberId));
            Assert.Equal(errorMessage, Assert.Throws<DguvValidationException>(() => _validator.Validate(bbnrUv, memberId)).GetFirstLine());
        }
    }
}
