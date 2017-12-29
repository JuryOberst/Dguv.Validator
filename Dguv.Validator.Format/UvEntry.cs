// <copyright file="UvEntry.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Dguv.Validator.Format
{
    public class UvEntry : IDguvNumberCheck
    {
        private static readonly IReadOnlyDictionary<int, IDguvChecksumHandler> _checksumHandlers =
            CreateChecksumHandlers().ToDictionary(x => x.Id);

        private static readonly RegexOptions _regexOptions =
#if !NETSTANDARD1_1
            RegexOptions.Compiled |
#endif
            RegexOptions.IgnoreCase;

        private readonly FormatRecord _record;
        private readonly IReadOnlyCollection<FormatItem> _formatItems;
        private readonly List<Regex> _regexes;
        private readonly ISet<char> _validChars;

        internal UvEntry(FormatRecord record, IEnumerable<FormatItem> formatItems)
        {
            _record = record;
            _formatItems = formatItems.ToList();
            _regexes = _formatItems
                .Select(x => new Regex($"^{x.RegExPattern}$", _regexOptions))
                .ToList();
            _validChars = new HashSet<char>(record.ValidChars.ToUpperInvariant().ToCharArray());
            if (_formatItems.Any(x => x.AllowWhitespace))
            {
                _validChars.Add(' ');
            }
        }

        public string BbnrUv => _record.Bnr;

        public string Name => _record.Name;

        public int MinLength => _record.MinLength ?? _formatItems.Min(i => i.MinLength);

        public int MaxLength => _record.MaxLength ?? _formatItems.Max(i => i.MaxLength);

        public IEnumerable<string> Patterns => _formatItems.Select(x => x.RegExPattern);

        public IStatus Validate(string memberId)
        {
            if (memberId.Length < MinLength || memberId.Length > MaxLength)
                return new UvCheckStatus(2, "Die Mitgliedsnummer hat eine falsche Länge");

            if (!memberId.ToUpperInvariant().ToCharArray().All(x => _validChars.Contains(x)))
                return new UvCheckStatus(1, "Die Mitgliedsnummer enthält unzulässige Zeichen");

            if (_regexes.Count == 0)
            {
                Debug.Assert(_record.CheckCalculation == null, "_record.CheckCalculation == null");
                return new UvCheckStatus(0, "Die Mitgliedsnummer ist OK");
            }

            foreach (var regex in _regexes)
            {
                var match = regex.Match(memberId);
                if (match.Success)
                {
                    if (_record.CheckCalculation != null)
                    {
                        // Prüfung hier
                        var checksumGroup = match.Groups["checksum"];
                        if (checksumGroup.Success)
                        {
                            var checksum = checksumGroup.Value;
                            var check = _checksumHandlers[_record.CheckCalculation.Value];
                            var calculatedChecksums = check.Calculate(memberId);
                            if (calculatedChecksums.Length != 0 && calculatedChecksums.All(x => x != checksum))
                            {
                                return new UvCheckStatus(4, "Die Mitgliedsnummer hat eine falsche Prüfziffer");
                            }
                        }
                    }

                    return new UvCheckStatus(0, "Die Mitgliedsnummer ist OK");
                }
            }

            return new UvCheckStatus(3, "Die Mitgliedsnummer hat eine falsche Formatierung");
        }

        private static IEnumerable<IDguvChecksumHandler> CreateChecksumHandlers()
        {
            var checkClasses = typeof(UvEntry)
                .GetTypeInfo()
                .Assembly
                .DefinedTypes
                .Where(x => x.ImplementedInterfaces.Any(i => i == typeof(IDguvChecksumHandler)))
                .ToList();

            foreach (var checkClass in checkClasses)
            {
                yield return (IDguvChecksumHandler)Activator.CreateInstance(checkClass.AsType());
            }
        }
    }
}
