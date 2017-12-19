// <copyright file="Check8.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 8
    /// </summary>
    public class Check8 : ICheckNumberValidator
    {
        /// <summary>
        /// Berechnung der Prüfziffer anhand der Mitgliedsnummer
        /// </summary>
        /// <param name="membershipNumber">Die Mitgliedsnummer</param>
        /// <returns>Die errechnete Prüfziffer</returns>
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string concated = string.Empty;
            var trimmed = membershipNumber.Trim();
            trimmed = trimmed.Length == 8 ? trimmed.Substring(1, 5) : trimmed.Substring(0, 5);
            if (trimmed.Length == 5)
            {
                var mgnr_parts = trimmed.ToCharArray();
                var mgnr_numbers = Array.ConvertAll(trimmed.ToCharArray(), c => (int)char.GetNumericValue(c));
                concated += mgnr_numbers[0];
                concated += 2 * mgnr_numbers[1];
                concated += mgnr_numbers[2];
                concated += 2 * mgnr_numbers[3];
                concated += mgnr_numbers[4];

                sum = Array.ConvertAll(concated.ToCharArray(), c => (int)char.GetNumericValue(c)).Sum();
                calculatedCheckNumber = sum % 10;
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
            var mgnr = membershipNumber.Trim();
            return Convert.ToUInt16(mgnr.Substring(mgnr.Length - 1, 1));
        }
    }
}
