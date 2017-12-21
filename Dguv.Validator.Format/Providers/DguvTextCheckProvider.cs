// <copyright file="DguvTextCheckProvider.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Dguv.Validator.Format.Providers
{
    /// <summary>
    /// Eine Liste der der Prüfungen, zu den Patterns aus einem Textdokument nachgeladen werden
    /// </summary>
    public class DguvTextCheckProvider : IDguvCheckProvider
    {
        private static readonly Dictionary<string, ICheckNumberValidator[]> _checkNumberValidators = new Dictionary<string, ICheckNumberValidator[]>()
        {
           { "15087927", new ICheckNumberValidator[] { new Checks.Check1(), new Checks.Check15() } },
           { "29036720", new ICheckNumberValidator[] { new Checks.Check1(), new Checks.Check15() } },
           { "44888436", new ICheckNumberValidator[] { new Checks.Check1(), new Checks.Check15() } },
           { "15141364", new ICheckNumberValidator[] { new Checks.Check2() } },
           { "15186676", new ICheckNumberValidator[] { new Checks.Check3() } },
           { "15250094", new ICheckNumberValidator[] { new Checks.Check4() } },
           { "29029801", new ICheckNumberValidator[] { new Checks.Check6() } },
           { "34364294", new ICheckNumberValidator[] { new Checks.Check8() } },
           { "37916971", new ICheckNumberValidator[] { new Checks.Check9() } },
           { "14066582", new ICheckNumberValidator[] { new Checks.Check10(), new Checks.Check15() } },
           { "42884688", new ICheckNumberValidator[] { new Checks.Check10(), new Checks.Check15() } },
           { "62279404", new ICheckNumberValidator[] { new Checks.Check12(), new Checks.Check15() } },
           { "63800761", new ICheckNumberValidator[] { new Checks.Check13() } },
           { "63886548", new ICheckNumberValidator[] { new Checks.Check14() } },
           { "67350937", new ICheckNumberValidator[] { new Checks.Check15() } },
           { "87661138", new ICheckNumberValidator[] { new Checks.Check15(), new Checks.Check16() } },
           { "87661183", new ICheckNumberValidator[] { new Checks.Check15(), new Checks.Check17() } }
        };

        private readonly IEnumerable<IDguvNumberCheck> _checks;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public DguvTextCheckProvider()
        {
            List<IDguvNumberCheck> checks = new List<IDguvNumberCheck>();

            // Eventuel sollte man die Textdatei auf einen Server legen und diese von dort laden.
            using (FileStream fs = File.Open("Data/uv171001_v4.txt", FileMode.Open, FileAccess.Read))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (var streamReader = new StreamReader(bs))
                    {
                        string line = string.Empty, bnr, name, format, validCharacters;

                        while (!streamReader.EndOfStream)
                        {
                            bnr = string.Empty;
                            name = string.Empty;
                            validCharacters = string.Empty;
                            format = string.Empty;
                            line = streamReader.ReadLine();
                            if (line.Length >= 28)
                                bnr = line.Substring(27, Math.Min(15, line.Length - 27)).Trim();
                            if (line.Length >= 57)
                                name = line.Substring(56, Math.Min(35, line.Length - 56)).TrimStart().TrimEnd();
                            if (line.Length >= 288)
                                validCharacters = line.Substring(287, Math.Min(55, line.Length - 287)).TrimStart().TrimEnd();
                            if (line.Length >= 343)
                                format = line.Substring(342, Math.Min(100, line.Length - 342)).TrimStart().TrimEnd();

                            var result = FormatParser.ParseFormat(format);

                            if (bnr != string.Empty && name != string.Empty)
                            {
                                _checkNumberValidators.TryGetValue(bnr, out ICheckNumberValidator[] checkers);
                                checks.Add(new Dguv.Validator.Checks.CharacterMapCheckFormat(bnr, name, result.Item1, result.Item2, validCharacters, result.Item3, checkers));
                            }
                        }
                    }
                }
            }
            _checks = checks;
        }

        /// <summary>
        /// Holt eine Liste von Prüfungen für Unfallversicherungsträger
        /// </summary>
        public IEnumerable<IDguvNumberCheck> Checks => _checks;

        /// <summary>
        /// Lädt die Prüfungen von Unfallversicherungsträgern.
        /// </summary>
        /// <returns>Ein <see cref="Task"/>, in dem das Laden der Prüfungen
        /// von Unfallversicherungsträgern ausgeführt wird.</returns>
        public Task<IEnumerable<IDguvNumberCheck>> LoadChecks()
        {
            var result = new TaskCompletionSource<IEnumerable<IDguvNumberCheck>>();
            result.SetResult(_checks);
            return result.Task;
        }
    }
}
