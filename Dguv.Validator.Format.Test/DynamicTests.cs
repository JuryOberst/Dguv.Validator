using Dguv.Validator.Checks;
using Dguv.Validator.Format.Providers;
using Fare;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Dguv.Validator.Format.Tests
{
    public class DynamicTests
    {
        private readonly ITestOutputHelper output;

        public DynamicTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task TestFormat()
        {
            var checks = await new DguvTextCheckProvider().LoadChecks();
            foreach (CharacterMapCheckFormat check in checks)
            {
                if (check.Patterns != null)
                    foreach (string pattern in check.Patterns)
                    {
                        var membershipNumber = new Xeger(pattern).Generate();

                        var result = de.werum.dguv.mgnr.plausi.PlausiFacade.getInstance().doPlausi(2, check.BbnrUv, membershipNumber);
                        var status = check.GetStatus(membershipNumber);

                        Assert.True(status == null && result.getErrorStatus() == 1 ||
                            status == Dguv.Validator.Properties.Resources.StatusMemberIdInvalidChecknumber && result.getErrorStatus() == 5,
                            $"Mitglidsnummer {membershipNumber} zu der BNr {check.BbnrUv} (Prüfung fehlerhaft)");
                    }
            }
        }
    }
}
