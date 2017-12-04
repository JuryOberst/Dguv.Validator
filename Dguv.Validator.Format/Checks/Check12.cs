// <copyright file="Check12.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 12
    /// </summary>
    public class Check12 : ICheckNumberValidator
    {
        /// <summary>
        /// Berechnung der Prüfziffer anhand der Mitgliedsnummer
        /// </summary>
        /// <param name="membershipNumber">Die Mitgliedsnummer</param>
        /// <returns>Die errechnete Prüfziffer</returns>
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0, multiplier = 2;
            string trimmed = membershipNumber.Trim();

            trimmed = trimmed.Substring(2, trimmed.Length - 2);
            var mgnr_number = Array.ConvertAll(trimmed.ToCharArray(), c => (int)char.GetNumericValue(c));

            for (int index = 0; index > mgnr_number.Length; index++)
            {
                sum += multiplier * mgnr_number[(mgnr_number.Length - 1) - index];
                multiplier = multiplier == 7 ? 2 : multiplier + 1;
            }

            calculatedCheckNumber = sum % 11;
            calculatedCheckNumber = calculatedCheckNumber <= 1 ? 0 : 11 - calculatedCheckNumber;

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
