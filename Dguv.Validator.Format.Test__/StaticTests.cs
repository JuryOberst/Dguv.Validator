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
                if(check.Patterns != null)
                    foreach (string pattern in check.Patterns)
                    {
                        var membershipNumber = new Xeger(pattern).Generate(); System.Diagnostics.Process process = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        startInfo.CreateNoWindow = true;
                        startInfo.UseShellExecute = false;
                        startInfo.RedirectStandardError = true;
                        startInfo.RedirectStandardInput = true;
                        startInfo.RedirectStandardOutput = true;
                        startInfo.FileName = "java.exe";
                        startInfo.Arguments = $@"-mx128m -cp lib\pl_mgnr.jar;lib\pl_generator.jar de.werum.dguv.mgnr.plausi.PlausiFacade -p 2 -u {check.BbnrUv} -m {membershipNumber}";
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();

                        var status = check.GetStatus(membershipNumber);

                        Assert.True(status == null && process.ExitCode == 0 ||
                            status == Dguv.Validator.Properties.Resources.StatusMemberIdInvalidChecknumber && process.ExitCode == 4,
                            $"Mitglidsnummer {membershipNumber} zu der BNr {check.BbnrUv} (Prüfung fehlerhaft)");
                    }
            }
        }
    }
}
