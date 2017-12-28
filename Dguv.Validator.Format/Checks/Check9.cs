// <copyright file="Check9.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 9
    /// </summary>
    public class Check9 : ICheckNumberValidator
    {
        /// <summary>
        /// Berechnung der Prüfziffer anhand der Mitgliedsnummer
        /// </summary>
        /// <param name="membershipNumber">Die Mitgliedsnummer</param>
        /// <returns>Die errechnete Prüfziffer</returns>
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            var trimmed = membershipNumber.Trim();

            if (trimmed.Length >= 8)
            {
                if (trimmed.StartsWith("0"))
                {
                    var mgnr_numbers = Array.ConvertAll(trimmed.Substring(1, 6).ToCharArray(), c => (int)char.GetNumericValue(c));
                    sum = (7 * mgnr_numbers[0]) +
                        (6 * mgnr_numbers[1]) +
                        (5 * mgnr_numbers[2]) +
                        (4 * mgnr_numbers[3]) +
                        (3 * mgnr_numbers[4]) +
                        (2 * mgnr_numbers[5]);

                    calculatedCheckNumber = sum % 11;

                    calculatedCheckNumber = calculatedCheckNumber <= 1 ? 0 : 11 - calculatedCheckNumber;
                }
            }

            return calculatedCheckNumber;
        }

        /// <summary>
        /// Validierung der Prüfziffer in einer Mitgliedsnummer
        /// </summary>
        /// <param name="membershipNumber">Die Mitgliedsnummmer</param>
        /// <returns><code>TRUE</code>, wenn die errechnete und die in der Mitgliedsnummer enthaltene Prüfziffer gleich ist. Sonst <code>FALSE</code></returns>
        public bool Validate(string membershipNumber)
        {
            var originChecknumber = ExtractCheckNumber(membershipNumber);
            var calculatedChecknumber = (int)Calculate(membershipNumber);
            return originChecknumber == calculatedChecknumber;
        }

        private int ExtractCheckNumber(string membershipNumber)
        {
            int checkNumber = 0;
            var mgnr = membershipNumber.Trim();
            if (mgnr.StartsWith("0"))
            {
                checkNumber = Convert.ToUInt16(mgnr.Substring(7, 1));
            }
            return checkNumber;
        }
    }
}
