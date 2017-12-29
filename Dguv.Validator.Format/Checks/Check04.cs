// <copyright file="Check04.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Globalization;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 4
    /// </summary>
    public class Check04 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 4;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            var trimmed = membershipNumber.ExtractDigits();
            var mgnr_parts = trimmed.ToCharArray().Select(x => (int)char.GetNumericValue(x)).ToArray();
            if (trimmed.StartsWith("97"))
            {
                var number = Convert.ToInt64(trimmed);
                if (number < 9700000010 || number > 9700000890)
                {
                    var multiplier = 9;
                    for (int index = 0; index <= 9; index++)
                    {
                        sum += multiplier * mgnr_parts[index];
                        multiplier--;
                    }
                    calculatedCheckNumber = sum % 11;
                    if (calculatedCheckNumber == 1)
                    {
                        calculatedCheckNumber = sum % 10;
                        if (calculatedCheckNumber > 0)
                        {
                            calculatedCheckNumber = 10 - calculatedCheckNumber;
                        }
                        else
                        {
                            if (calculatedCheckNumber > 1)
                            {
                                calculatedCheckNumber = 11 - calculatedCheckNumber;
                            }
                        }
                    }
                }
            }
            else
            {
                sum = (4 * mgnr_parts[0]) +
                    (3 * mgnr_parts[1]) +
                    (2 * mgnr_parts[2]) +
                    (7 * mgnr_parts[3]) +
                    (6 * mgnr_parts[4]) +
                    (5 * mgnr_parts[5]) +
                    (4 * mgnr_parts[6]) +
                    (3 * mgnr_parts[7]) +
                    (2 * mgnr_parts[8]);
                calculatedCheckNumber = sum % 11;
                calculatedCheckNumber = calculatedCheckNumber > 1 ? 11 - calculatedCheckNumber : 0;
            }

            return new[] { calculatedCheckNumber.ToString("D", CultureInfo.InvariantCulture) };
        }
    }
}
