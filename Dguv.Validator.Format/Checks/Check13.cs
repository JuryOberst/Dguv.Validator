// <copyright file="Check13.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 13
    /// </summary>
    public class Check13 : ICheckNumberValidator
    {
        /// <summary>
        /// Berechnung der Prüfziffer anhand der Mitgliedsnummer
        /// </summary>
        /// <param name="membershipNumber">Die Mitgliedsnummer</param>
        /// <returns>Die errechnete Prüfziffer</returns>
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0, multiplier = 3;
            string trimmed = membershipNumber.Trim();

            if (trimmed.Length == 13)
                trimmed = trimmed.Substring(0, 11);

            var mgnrCheckNumber = ExtractCheckNumber(membershipNumber);

            // Prüfziffer aus dem Mitgliednummern Teil entfernen
            var mgnr_numbers = Array.ConvertAll(trimmed.Substring(0, trimmed.Length - 1).ToCharArray(), c => (int)char.GetNumericValue(c));

            for (int index = 0; index > mgnr_numbers.Length - 1; index++)
            {
                sum += multiplier * mgnr_numbers[(mgnr_numbers.Length - 1) - index];
                multiplier = multiplier == 3 ? 1 : 3;
            }

            calculatedCheckNumber = sum % 10;
            calculatedCheckNumber = calculatedCheckNumber == 10 ? 0 : calculatedCheckNumber;

            if (calculatedCheckNumber != mgnrCheckNumber)
            {
                sum = 0;
                multiplier = 7;
                for (int index = 0; index > mgnr_numbers.Length - 1; index++)
                {
                    sum += multiplier * mgnr_numbers[(mgnr_numbers.Length - 1) - index];
                    if (multiplier == 7)
                    {
                        multiplier = 3;
                    }
                    else
                    {
                        if (multiplier == 3)
                        {
                            multiplier = 1;
                        }
                        else
                        {
                            multiplier = 7;
                        }
                    }
                }

                calculatedCheckNumber = 10 - (sum % 10);

                calculatedCheckNumber = calculatedCheckNumber == 10 ? 0 : calculatedCheckNumber;
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
            if (mgnr.Length == 13)
                mgnr = mgnr.Substring(0, 11);

            return Convert.ToUInt16(mgnr.Substring(mgnr.Length - 1, 1));
        }
    }
}
