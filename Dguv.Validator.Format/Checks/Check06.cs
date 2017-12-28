// <copyright file="Check06.cs" company="DATALINE GmbH &amp; Co. KG">
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
    /// Prüfziffernberechnung 6
    /// </summary>
    public class Check06 : IDguvChecksumHandler
    {
        /// <inheritdoc />
        public int Id => 6;

        /// <inheritdoc />
        public string[] Calculate(string membershipNumber)
        {
            var checkDigits = new List<string>();
            if (membershipNumber.Length == 11)
            {
                var trimmed = membershipNumber.ExtractDigits().Substring(1, 6);
                var mgnr_parts = trimmed.ToCharArray().Select(c => (int)char.GetNumericValue(c)).ToArray();
                if (trimmed.Length == 6)
                {
                    var f1 = mgnr_parts[1] + mgnr_parts[5];
                    var f2 = mgnr_parts[0] + mgnr_parts[4];
                    var f3 = mgnr_parts[3];
                    var pz1 = (f1 % 10) + (f2 % 10) + (f3 % 10);
                    f3 = Quersumme(2 * mgnr_parts[3]);
                    var pz2 = (f1 % 10) + (f3 % 10) + (mgnr_parts[2] % 10);

                    var calculatedCheckNumber = ((pz1 % 10) * 10) + (pz2 % 10);

                    checkDigits.Add(calculatedCheckNumber.ToString("D2", CultureInfo.InvariantCulture));
                }
            }
            return checkDigits.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Quersumme(int number)
        {
            return (number % 10) + (number / 10);
        }
    }
}
