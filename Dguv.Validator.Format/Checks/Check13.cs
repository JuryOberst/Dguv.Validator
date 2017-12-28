// <copyright file="Check13.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 13
    /// </summary>
    public class Check13 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 13;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            string trimmed = membershipNumber.ExtractDigits();

            if (trimmed.Length == 13)
                trimmed = trimmed.Substring(0, 11);

            // Prüfziffer aus dem Mitgliednummern Teil entfernen
            var mgnr_numbers = trimmed.Substring(0, trimmed.Length - 1).ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();

            var result1 = CalculateVariant1(mgnr_numbers);
            var result2 = CalculateVariant2(mgnr_numbers);

            return new[] { result1.ToString("D", CultureInfo.InvariantCulture), result2.ToString("D", CultureInfo.InvariantCulture) };
        }

        private static int CalculateVariant1(int[] mgnr_numbers)
        {
            var sum = 0;
            var multiplier = 3;
            for (int index = 0; index != mgnr_numbers.Length; index++)
            {
                sum += multiplier * mgnr_numbers[(mgnr_numbers.Length - 1) - index];
                multiplier = 4 - multiplier;
            }

            var calculatedCheckNumber = sum % 10;
            return calculatedCheckNumber == 10 ? 0 : calculatedCheckNumber;
        }

        private static int CalculateVariant2(int[] mgnr_numbers)
        {
            var sum = 0;
            var multiplier = 7;
            for (int index = 0; index != mgnr_numbers.Length; index++)
            {
                sum += multiplier * mgnr_numbers[(mgnr_numbers.Length - 1) - index];
                if (multiplier == 7)
                {
                    multiplier = 3;
                }
                else if (multiplier == 3)
                {
                    multiplier = 1;
                }
                else
                {
                    multiplier = 7;
                }
            }

            var calculatedCheckNumber = 10 - (sum % 10);
            return calculatedCheckNumber == 10 ? 0 : calculatedCheckNumber;
        }
    }
}
