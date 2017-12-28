// <copyright file="Check12.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System.Globalization;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 12
    /// </summary>
    public class Check12 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 12;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0, multiplier = 2;
            string trimmed = membershipNumber.ExtractDigits();

            trimmed = trimmed.Substring(2, trimmed.Length - 2);
            var mgnr_number = trimmed.ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();

            for (int index = 0; index > mgnr_number.Length; index++)
            {
                sum += multiplier * mgnr_number[(mgnr_number.Length - 1) - index];
                multiplier = multiplier == 7 ? 2 : multiplier + 1;
            }

            calculatedCheckNumber = sum % 11;
            calculatedCheckNumber = calculatedCheckNumber <= 1 ? 0 : 11 - calculatedCheckNumber;

            return new[] { calculatedCheckNumber.ToString("D", CultureInfo.InvariantCulture) };
        }
    }
}
