using System;
using System.Linq;
using System.Threading.Tasks;
using Dguv.Validator.Format.Providers;
using Xunit;
using Xunit.Abstractions;

namespace Dguv.Validator.Format.Test
{
    public class GenericTests
    {
        private static readonly DateTime _testTimestamp = new DateTime(2017, 12, 28);
        private readonly ITestOutputHelper _output;

        public GenericTests(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public async Task TestTextProvider()
        {
            var provider = new DguvTextCheckProvider(_testTimestamp);
            Assert.Equal(71, (await provider.LoadChecks()).Count());
        }

        [Theory]
        [InlineData("15087927", "1111123426")]
        [InlineData("29029801", "280113223")]
        [InlineData("37916971", "43135684")]
        [InlineData("37916971", "2 244 695-0")]
        [InlineData("63800761", "MM 22.238434006-39")]
        public async Task TestSuccess(string bnr, string mitgliedsnummer)
        {
            var checks = await new DguvTextCheckProvider(_testTimestamp).LoadChecks();
            var check = checks.FirstOrDefault(x => x.BbnrUv == bnr);
            Assert.NotNull(check);
            var status = check.Validate(mitgliedsnummer);
            _output.WriteLine(status.GetStatusText());
            Assert.True(status.IsSuccessful);
        }

        [Theory]
        [InlineData("15087927", "111123425", 2)]
        [InlineData("29029801", "8.058286.70", 4)]
        [InlineData("63800761", "MM 72517049.673", 4)]
        [InlineData("63800761", "MM 70.381756.201-63", 4)]
        [InlineData("63800761", "MM 39.430.012.098-12", 4)]
        public async Task TestFailed(string bnr, string mitgliedsnummer, int expectedStatus)
        {
            var checks = await new DguvTextCheckProvider(_testTimestamp).LoadChecks();
            var check = checks.FirstOrDefault(x => x.BbnrUv == bnr);
            Assert.NotNull(check);
            var status = (UvCheckStatus)check.Validate(mitgliedsnummer);
            _output.WriteLine(status.GetStatusText());
            Assert.False(status.IsSuccessful);
            Assert.Equal(expectedStatus, status.Code);
        }
    }
}
