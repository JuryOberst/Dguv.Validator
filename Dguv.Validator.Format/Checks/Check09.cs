// <copyright file="Check09.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 9
    /// </summary>
    public class Check09 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 9;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            var trimmed = membershipNumber.ExtractDigits();

            var result = new List<string>();
            if (trimmed.Length >= 8)
            {
                if (trimmed.StartsWith("0"))
                {
                    var mgnr_numbers = trimmed.Substring(1, 6).ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();
                    sum = (7 * mgnr_numbers[0]) +
                        (6 * mgnr_numbers[1]) +
                        (5 * mgnr_numbers[2]) +
                        (4 * mgnr_numbers[3]) +
                        (3 * mgnr_numbers[4]) +
                        (2 * mgnr_numbers[5]);

                    calculatedCheckNumber = sum % 11;

                    calculatedCheckNumber = calculatedCheckNumber <= 1 ? 0 : 11 - calculatedCheckNumber;
                    result.Add(calculatedCheckNumber.ToString("D", CultureInfo.InvariantCulture));
                }
            }

            return result.ToArray();
        }
    }
}
