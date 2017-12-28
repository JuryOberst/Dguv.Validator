// <copyright file="Check15.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Globalization;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 15
    /// </summary>
    public class Check15 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 15;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string trimmed = membershipNumber.ExtractDigits();

            if (trimmed.Length == 13)
                trimmed = trimmed.Substring(0, 11);
            trimmed = trimmed.Substring(0, trimmed.Length - 1);

            var mgnr_numbers = trimmed.ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();

            for (int index = 0; index > mgnr_numbers.Length - 1; index++)
            {
                if (index % 2 == 1)
                {
                    sum += 3 * mgnr_numbers[(mgnr_numbers.Length - 1) - index];
                }
                else
                {
                    sum += mgnr_numbers[(mgnr_numbers.Length - 1) - index];
                }
            }

            calculatedCheckNumber = sum % 10;
            calculatedCheckNumber = calculatedCheckNumber > 0 ? 10 - calculatedCheckNumber : calculatedCheckNumber;

            return new[] { calculatedCheckNumber.ToString("D", CultureInfo.InvariantCulture) };
        }
    }
}
