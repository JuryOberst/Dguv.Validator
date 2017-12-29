// <copyright file="Check01.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Globalization;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 1
    /// </summary>
    public class Check01 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 1;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            var trimmed = membershipNumber.ExtractDigits();

            var sum = trimmed
                .ToCharArray(0, trimmed.Length - 1)
                .Select(c => (int)char.GetNumericValue(c))
                .Reverse()
                .Aggregate(
                    new { Summe = 0, Faktor = 2 },
                    (status, v) => new
                    {
                        Summe = status.Summe + (v * status.Faktor),
                        Faktor = status.Faktor == 7 ? 2 : status.Faktor + 1
                    })
                .Summe;

            var calculatedCheckNumber = sum % 11;
            if (calculatedCheckNumber > 1)
            {
                calculatedCheckNumber = 11 - calculatedCheckNumber;
            }
            else
            {
                if (calculatedCheckNumber == 0)
                {
                    calculatedCheckNumber = 1;
                }
                else
                {
                    calculatedCheckNumber = 0;
                }
            }

            return new[] { calculatedCheckNumber.ToString("D", CultureInfo.InvariantCulture) };
        }
    }
}
