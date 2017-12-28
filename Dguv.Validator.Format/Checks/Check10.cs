// <copyright file="Check10.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 10
    /// </summary>
    public class Check10 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 10;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            var trimmed = (membershipNumber.Substring(1, 1) == "/" ? membershipNumber.Substring(2) : membershipNumber).ExtractDigits();

            var mgnr_number = trimmed.ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();

            if (trimmed.Length >= 8)
            {
                sum = (2 * mgnr_number[0]) +
                    (3 * mgnr_number[1]) +
                    (7 * mgnr_number[2]) +
                    (6 * mgnr_number[3]) +
                    (5 * mgnr_number[4]) +
                    (4 * mgnr_number[5]) +
                    (3 * mgnr_number[6]);

                calculatedCheckNumber = sum % 11;

                calculatedCheckNumber = calculatedCheckNumber <= 1 ? 0 : 11 - calculatedCheckNumber;
            }

            return new[] { calculatedCheckNumber.ToString("D", CultureInfo.InvariantCulture) };
        }
    }
}
