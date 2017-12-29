// <copyright file="Check11.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Globalization;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 11
    /// </summary>
    public class Check11 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 11;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string trimmed = membershipNumber.ExtractDigits();
            trimmed = trimmed.Substring(trimmed.Length - 6, 6);

            var mgnr_number = trimmed.ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();

            sum = (7 * mgnr_number[0]) +
                (6 * mgnr_number[1]) +
                (5 * mgnr_number[2]) +
                (4 * mgnr_number[3]) +
                (3 * mgnr_number[4]) +
                (2 * mgnr_number[5]);

            calculatedCheckNumber = sum % 11;

            calculatedCheckNumber = calculatedCheckNumber <= 1 ? 0 : 11 - calculatedCheckNumber;

            return new[] { calculatedCheckNumber.ToString("D", CultureInfo.InvariantCulture) };
        }
    }
}
