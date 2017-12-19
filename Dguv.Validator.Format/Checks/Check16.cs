// <copyright file="Check16.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 16
    /// </summary>
    public class Check16 : ICheckNumberValidator
    {
        /// <summary>
        /// Berechnung der Prüfziffer anhand der Mitgliedsnummer
        /// </summary>
        /// <param name="membershipNumber">Die Mitgliedsnummer</param>
        /// <returns>Die errechnete Prüfziffer</returns>
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string trimmed = membershipNumber.Trim();
            trimmed = trimmed.Substring(0, 7);

            var mgnr_numbers = Array.ConvertAll(trimmed.Substring(0, trimmed.Length - 1).ToCharArray(), c => (int)char.GetNumericValue(c));
            string concated = string.Empty;
            if (mgnr_numbers.Length == 6)
            {
                concated = concated + (2 * mgnr_numbers[0]);
                concated = concated + mgnr_numbers[1];
                concated = concated + (2 * mgnr_numbers[2]);
                concated = concated + (5 * mgnr_numbers[3]);
                concated = concated + (7 * mgnr_numbers[4]);
                concated = concated + mgnr_numbers[0];
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
            string mgnr = membershipNumber.Trim();
            return Convert.ToUInt16(mgnr.Substring(mgnr.Length - 1, 1));
        }
    }
}
