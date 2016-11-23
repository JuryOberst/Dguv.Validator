using System;
using System.Linq;
using System.Threading.Tasks;

using Dguv.Validator.Providers;

using Xunit;

namespace Dguv.Validator.Tests
{
    public class GenericTests
    {
        [Fact]
        public async Task TestStaticProvider()
        {
            var provider = new StaticCheckProvider();
            Assert.Equal(71, (await provider.LoadChecks()).Count());
        }

        [Fact(Skip = "Die Liste der Mitgliedsnummern wurde von der Web-Seite entfernt")]
        [Obsolete]
        public async Task TestWebProvider()
        {
            var provider = new DguvHtmlCheckProvider();
            Assert.Equal(57, (await provider.LoadChecks()).Count());
        }

        [Fact]
        public async Task TestWebPdfProvider()
        {
            var provider = new GkvAnlage20CheckProvider();
            Assert.Equal(71, (await provider.LoadChecks()).Count());
        }
    }
}
