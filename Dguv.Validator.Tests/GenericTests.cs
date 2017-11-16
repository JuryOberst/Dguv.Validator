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
    }
}
