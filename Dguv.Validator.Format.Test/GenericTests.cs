using System.Linq;
using System.Threading.Tasks;
using Dguv.Validator.Format.Providers;
using Xunit;
using Xunit.Abstractions;

namespace Dguv.Validator.Tests
{
    public class GenericTests
    {
        private readonly ITestOutputHelper output;

        public GenericTests(ITestOutputHelper output)
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
            output.WriteLine(status.GetStatusText());
            Assert.True(check.IsValid(mitgliedsnummer));
        }

        [Theory]
        [InlineData("15087927", "111123425")]
        public async Task TestLength(string bnr, string mitgliedsnummer)
        {
            var checks = await new DguvTextCheckProvider().LoadChecks();
            var check = checks.FirstOrDefault(x => x.BbnrUv == bnr);
            var status = check.GetStatus(mitgliedsnummer);
            output.WriteLine(status.GetStatusText());
            Assert.False(check.IsValid(mitgliedsnummer));
        }
    }
}
