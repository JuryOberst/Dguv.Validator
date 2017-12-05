using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Dguv.Validator.Format.Providers;
using Xunit.Abstractions;
using Dguv.Validator.Checks;
using Fare;

namespace Dguv.Validator.Format.Test
{
    public class StaticTest
    {

        private readonly ITestOutputHelper output;

        public StaticTest(ITestOutputHelper output)
        {
            this.output = output;
        }       

        [Fact]
        public async Task TestTextProvider()
        {
            var provider = new DguvTextCheckProvider();
            Assert.Equal(226, (await provider.LoadChecks()).Count());
        }

        [Theory]
        [InlineData("15087927", "1111123425")]
        public async Task TestCheckers(string bnr, string mitgliedsnummer)
        {
            var checks = await new DguvTextCheckProvider().LoadChecks();
            var check = checks.FirstOrDefault(x => x.BbnrUv == bnr);
            var status = check.GetStatus(mitgliedsnummer);
            Assert.True(check.IsValid(mitgliedsnummer));
        }


        [Theory]
        [InlineData("15087927", "111123425")]
        public async Task TestLength(string bnr, string mitgliedsnummer)
        {
            var checks = await new DguvTextCheckProvider().LoadChecks();
            var check = checks.FirstOrDefault(x => x.BbnrUv == bnr);
            var status = check.GetStatus(mitgliedsnummer);
            output.WriteLine(status);
            Assert.False(check.IsValid(mitgliedsnummer));
        }

        [Fact]
        public async Task TestFormat()
        {
            var checks = await new DguvTextCheckProvider().LoadChecks();
            foreach (CharacterMapCheckFormat check in checks)
            {
                foreach (string pattern in check.Patterns)
                {
                    var membershipNumber = new Xeger(pattern).Generate();

                    Assert.True(check.CheckWithPatterns(membershipNumber));
                }
            }
        }
    }
}
