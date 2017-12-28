// <copyright file="Check08.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 8
    /// </summary>
    public class Check08 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 8;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string concated = string.Empty;
            var trimmed = membershipNumber.ExtractDigits();
            trimmed = trimmed.Length == 8 ? trimmed.Substring(2, 5) : trimmed.Substring(0, 5);
            if (trimmed.Length == 5)
            {
                var mgnr_numbers = trimmed.ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();
                var concatenated = new int[12];
                var concatenatedIndex = 0;

                AddDigits(concatenated, ref concatenatedIndex, 1 * mgnr_numbers[0]);
                AddDigits(concatenated, ref concatenatedIndex, 2 * mgnr_numbers[1]);
                AddDigits(concatenated, ref concatenatedIndex, 1 * mgnr_numbers[2]);
                AddDigits(concatenated, ref concatenatedIndex, 2 * mgnr_numbers[3]);
                AddDigits(concatenated, ref concatenatedIndex, 1 * mgnr_numbers[4]);
                sum = concatenated.Sum();

                calculatedCheckNumber = sum % 10;
            }

            return new[] { calculatedCheckNumber.ToString("D", CultureInfo.InvariantCulture) };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AddDigits(int[] concatenated, ref int concatenatedIndex, int number)
        {
            concatenated[concatenatedIndex++] = number % 10;
            concatenated[concatenatedIndex++] = number / 10;
        }
    }
}
