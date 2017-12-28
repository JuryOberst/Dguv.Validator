// <copyright file="Check14.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System.Globalization;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 14
    /// </summary>
    public class Check14 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 14;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string trimmed = membershipNumber.ExtractDigits();

            var mgnr_numbers = trimmed.Substring(0, trimmed.Length - 1).ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();

            if (mgnr_numbers.Length == 7)
            {
                sum += (8 * mgnr_numbers[0]) +
                    (7 * mgnr_numbers[1]) +
                    (6 * mgnr_numbers[2]) +
                    (5 * mgnr_numbers[3]) +
                    (4 * mgnr_numbers[4]) +
                    (3 * mgnr_numbers[5]) +
                    (2 * mgnr_numbers[6]);
            }

            calculatedCheckNumber = sum % 11;
            calculatedCheckNumber = calculatedCheckNumber <= 1 ? 0 : 11 - calculatedCheckNumber;

            return new[] { calculatedCheckNumber.ToString("D", CultureInfo.InvariantCulture) };
        }
    }
}
