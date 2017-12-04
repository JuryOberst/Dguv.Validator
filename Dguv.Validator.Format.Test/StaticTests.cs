using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Dguv.Validator.Format.Providers;
using System.Diagnostics;
using System.Linq;
using Xunit.Abstractions;
using System.Collections.Generic;

namespace Dguv.Validator.Format.Test
{   
    public class StaticTest
    {

        private readonly ITestOutputHelper output;

        public StaticTest(ITestOutputHelper output)
        {
            this.output = output;
        }
                
        public void Init()
        {

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


        //public async Task TestGenaeral()
        //{
        //    var generated = new Dictionary<string, string[]>();
        //    var checks = await new DguvTextCheckProvider().LoadChecks();
        //    foreach(var check in checks)
        //    {
        //        foreach(string pattern in check.Patter)
        //        {
        //            string[] membershipNumbers = null;
        //            generated.TryGetValue(check.BbnrUv, out membershipNumbers);
        //            if (membershipNumbers == null)
        //                membershipNumbers = new string[] { };
        //            var membershipNumber = new Fare.RegExp(pattern).ToString();
        //            membershipNumbers.Append(membershipNumber);

        //        }
        //    }
        //}


    }
}
