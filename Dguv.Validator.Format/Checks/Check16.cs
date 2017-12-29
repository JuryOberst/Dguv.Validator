// <copyright file="Check16.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 16
    /// </summary>
    public class Check16 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 16;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string trimmed = membershipNumber.ExtractDigits();
            trimmed = trimmed.Substring(0, 7);

            var mgnr_numbers = trimmed.Substring(0, trimmed.Length - 1).ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();
            var concatenated = new int[12];
            var concatenatedIndex = 0;
            if (mgnr_numbers.Length == 6)
            {
                AddDigits(concatenated, ref concatenatedIndex, 2 * mgnr_numbers[0]);
                AddDigits(concatenated, ref concatenatedIndex, 1 * mgnr_numbers[1]);
                AddDigits(concatenated, ref concatenatedIndex, 2 * mgnr_numbers[2]);
                AddDigits(concatenated, ref concatenatedIndex, 5 * mgnr_numbers[3]);
                AddDigits(concatenated, ref concatenatedIndex, 7 * mgnr_numbers[4]);
                AddDigits(concatenated, ref concatenatedIndex, 1 * mgnr_numbers[5]);
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
