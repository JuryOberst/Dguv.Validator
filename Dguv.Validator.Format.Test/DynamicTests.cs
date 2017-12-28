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

                        var pi = new System.Diagnostics.ProcessStartInfo(@"lib\pl_mgnr.exe", $"-p 2 -u {check.BbnrUv} -m {membershipNumber}");
                        pi.CreateNoWindow = true;
                        pi.RedirectStandardOutput = true;
                        pi.UseShellExecute = false;
                        var process = new System.Diagnostics.Process
                        {
                            StartInfo = pi
                            
                        };
                        process.Start();
                        process.WaitForExit();
                        var processExitCode = process.ExitCode;

                        // FIXIT J.Oberst: Ein Fehler tritt auf. 
                        //var plausi = de.werum.dguv.mgnr.plausi.PlausiFacade.getInstance();
                        //var result = plausi.doPlausi(2, check.BbnrUv, membershipNumber);
                        //var result = de.werum.dguv.mgnr.plausi.PlausiFacade.getInstance().doPlausi(2, check.BbnrUv, membershipNumber);
                        var status = check.GetStatus(membershipNumber);

                        Assert.True((status.StatusCode == 0 && processExitCode == 0) || (processExitCode != 0 && status.StatusCode != 0),
                            $"Mitglidsnummer {membershipNumber} zu der BNr {check.BbnrUv} (Prüfung fehlerhaft)");
                    }
            }
        }
    }
}
