using System;
using System.Linq;
using System.Threading.Tasks;

using Dguv.Validator.Checks;
using Dguv.Validator.Providers;

namespace CreateStaticCheckProviderEntries
{
    internal class Program
    {
        private static void Main()
        {
            Execute().Wait();
        }

        private static async Task Execute()
        {
            var provider = new GkvAnlage20CheckProvider();
            foreach (var check in (await provider.LoadChecks()).Cast<CharacterMapCheck>().OrderBy(x => x.BbnrUv))
            {
                string validChars;
                if (check.ValidCharacters.Count == 0)
                {
                    validChars = "string.Empty";
                }
                else
                {
                    validChars = string.Format(
                        "\"{0}\"",
                        string.Join(string.Empty, check.ValidCharacters.Select(x => x.ToString())).Replace("\"", @"\"""));
                }

                var expression = string.Format(
                    "new CharacterMapCheck(\"{0}\", \"{1}\", {2}, {3}, {4})",
                    check.BbnrUv,
                    check.Name.Replace("\"", @"\"""),
                    check.MinLength,
                    check.MaxLength,
                    validChars);

                Console.WriteLine(@"{1}{0},", expression, @"                ");
            }
        }
    }
}
