// <copyright file="FormatRecord.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System;
using System.Globalization;

namespace Dguv.Validator.Format
{
    internal class FormatRecord
    {
        private FormatRecord(
            DateTime validSince,
            string bnr,
            string name,
            string validChars,
            int? minLength,
            int? maxLength,
            string format,
            int? checkCalculation)
        {
            ValidSince = validSince;
            Bnr = bnr;
            Name = name;
            ValidChars = validChars;
            MinLength = minLength;
            MaxLength = maxLength;
            Format = format;
            CheckCalculation = checkCalculation;
        }

        public DateTime ValidSince { get; }
        public string Bnr { get; }
        public string Name { get; }
        public string ValidChars { get; }
        public int? MinLength { get; }
        public int? MaxLength { get; }
        public string Format { get; }
        public int? CheckCalculation { get; }

        public static FormatRecord Parse(string line)
        {
            var validSince = DateTime.ParseExact(line.Substring(4, 8), "yyyyMMdd", CultureInfo.InvariantCulture);
            var bnr = line.Substring(27, 15).TrimEnd();
            var name = line.Substring(56, 35).TrimEnd();
            var validChars = line.Substring(292, 50).TrimEnd();
            var format = line.Substring(342, 100).TrimEnd();
            var minLengthText = line.Substring(288, 2);
            var maxLengthText = line.Substring(290, 2);
            var minLength = string.IsNullOrWhiteSpace(minLengthText) ? (int?)null : Convert.ToInt32(minLengthText, 10);
            var maxLength = string.IsNullOrWhiteSpace(maxLengthText) ? (int?)null : Convert.ToInt32(maxLengthText, 10);
            var checkCalculationText = line.Substring(442, 2).Trim();
            var checkCalculation = string.IsNullOrEmpty(checkCalculationText)
                ? (int?)null
                : Convert.ToInt32(checkCalculationText, 10);
            return new FormatRecord(validSince, bnr, name, validChars, minLength, maxLength, format, checkCalculation);
        }
    }
}
