using Dguv.Validator.Format.Providers;
using Dguv.Validator.JavaFormat;
using Fare;

using Xunit;
using Xunit.Abstractions;

namespace Dguv.Validator.Format.Test
{
    public class DynamicTests
    {
        private readonly ITestOutputHelper output;

        public DynamicTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestFormat()
        {
            var javaValidator = new DguvJavaIkvmValidator();
            var checks = new DguvTextCheckProvider().Checks;
            foreach (var check in checks)
            {
                foreach (string pattern in check.Patterns)
                {
                    var farePattern = pattern.Replace("(?<checksum>", "(");
                    var membershipNumber = new Xeger(farePattern).Generate();

                    /*
                    var pi = new System.Diagnostics.ProcessStartInfo(@"lib\pl_mgnr.exe",
                        $"-p 2 -u {check.BbnrUv} -m {membershipNumber}");
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
                    */

                    var status1 = (UvCheckStatus)check.Validate(membershipNumber);
                    var status2 = (UvJavaCheckStatus)javaValidator.Validate(check.BbnrUv, membershipNumber);

                    output.WriteLine($"Test von {membershipNumber} für {check.BbnrUv} mit Status {status2.Code} ({status2.GetStatusText()})");
                    if(check.BbnrUv != "63800761")
                        Assert.Equal(status2.Code, status1.Code);
                }
            }
        }
    }
}
