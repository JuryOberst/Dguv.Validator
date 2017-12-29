// <copyright file="Check02.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Globalization;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 2
    /// </summary>
    public class Check02 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 2;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            int sum = 0, index = 0;
            for (var i = 9; i >= 2; i--)
            {
                sum += i * (int)char.GetNumericValue(membershipNumber, index);
                index++;
            }

            var calculatedCheckNumber = 11 - (sum % 11);
            if (calculatedCheckNumber >= 10)
            {
                calculatedCheckNumber -= 10;
            }

            return new[] { calculatedCheckNumber.ToString("D", CultureInfo.InvariantCulture) };
        }
    }
}
