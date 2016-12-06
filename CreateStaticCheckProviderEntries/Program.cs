using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Dguv.Validator.Checks;
using Dguv.Validator.Providers;

using Microsoft.Extensions.CommandLineUtils;

namespace CreateStaticCheckProviderEntries
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var app = new CommandLineApplication()
            {
                Name = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]),
                Description = "Erstellung eines C#-Quelltext-Fragments anhand der Anlage 20",
            };
            app.HelpOption("-h|--help|-?");
            app.OptionHelp.Description = "Zeigt die Hilfe";
            var outputOption = app.Option("-o|--output", "Ausgabe-Datei", CommandOptionType.SingleValue);
            app.OnExecute(async () =>
            {
                if (outputOption.HasValue())
                {
                    using (var writer = new StreamWriter(outputOption.Value()))
                    {
                        return await Execute(writer);
                    }
                }

                return await Execute(Console.Out);
            });
            app.Execute(args);
        }

        private static async Task<int> Execute(TextWriter writer)
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

                await writer.WriteLineAsync($"                {expression},");
            }

            return 0;
        }
    }
}
