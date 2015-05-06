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
            Assert.Equal(57, (await provider.LoadChecks()).Count());
        }

        [Fact]
        public async Task TestWebProvider()
        {
            var provider = new WebCheckProvider();
            Assert.Equal(57, (await provider.LoadChecks()).Count());
        }
    }
}
