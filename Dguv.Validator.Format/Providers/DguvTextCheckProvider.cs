// <copyright file="DguvTextCheckProvider.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dguv.Validator.Format.Providers
{
    /// <summary>
    /// Eine Liste der der Prüfungen, zu den Patterns aus einem Textdokument nachgeladen werden
    /// </summary>
    public class DguvTextCheckProvider : IDguvCheckProvider
    {
        private readonly Lazy<List<UvEntry>> _checks;

        public DguvTextCheckProvider()
            : this(DateTime.Now)
        {
        }

        public DguvTextCheckProvider(DateTime currentDate)
        {
            _checks = new Lazy<List<UvEntry>>(() => CreateChecks(currentDate.Date));
        }

        public IReadOnlyCollection<UvEntry> Checks => _checks.Value;

        /// <inheritdoc/>
        public Task<IEnumerable<IDguvNumberCheck>> LoadChecks()
        {
            return Task.FromResult((IEnumerable<IDguvNumberCheck>)_checks.Value);
        }

        private static List<UvEntry> CreateChecks(DateTime currentDate)
        {
            var asm = typeof(DguvTextCheckProvider).GetTypeInfo().Assembly;
            var resName = $"Dguv.Validator.Format.Data.formats.txt";

            var usedBbnr = new Dictionary<string, FormatRecord>();

            // Eventuel sollte man die Textdatei auf einen Server legen und diese von dort laden.
            using (var formatsStream = asm.GetManifestResourceStream(resName))
            {
                using (var streamReader = new StreamReader(formatsStream))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                            continue;

                        if (!line.StartsWith("UVSD"))
                            continue;

                        var record = FormatRecord.Parse(line);
                        if (record.ValidSince <= currentDate)
                        {
                            if (usedBbnr.TryGetValue(record.Bnr, out var prevRecord))
                            {
                                if (prevRecord.ValidSince < record.ValidSince)
                                {
                                    usedBbnr[record.Bnr] = record;
                                }
                            }
                            else
                            {
                                usedBbnr.Add(record.Bnr, record);
                            }
                        }
                    }
                }
            }

            var checks = usedBbnr.Values.Select(x => new UvEntry(x, FormatParser.ParseFormat(x.Format))).ToList();

            return checks;
        }
    }
}
