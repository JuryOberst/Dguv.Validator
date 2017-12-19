// <copyright file="Check1.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 1
    /// </summary>
    public class Check1 : ICheckNumberValidator
    {
        /// <summary>
        /// Berechnung der Prüfziffer anhand der Mitgliedsnummer
        /// </summary>
        /// <param name="membershipNumber">Die Mitgliedsnummer</param>
        /// <returns>Die errechnete Prüfziffer</returns>
        public object Calculate(string membershipNumber)
        {
            int mgnrCheckNumber, calculatedCheckNumber, lenght, multiplier = 2, sum = 0;
            CharEnumerator chopped;
            var trimmed = membershipNumber.Trim();
            mgnrCheckNumber = ExtractCheckNumber(membershipNumber);
            lenght = trimmed.Length - 1;
            chopped = trimmed.Substring(0, lenght).GetEnumerator();
            while (chopped.MoveNext())
            {
                int number = Convert.ToUInt16(chopped.Current);
                sum += number * multiplier;
                multiplier = multiplier == 7 ? 2 : multiplier + 1;
            }
            calculatedCheckNumber = sum % 11;
            if (calculatedCheckNumber > 1)
            {
                calculatedCheckNumber = 11 - calculatedCheckNumber;
            }
            else
            {
                if (calculatedCheckNumber == 0)
                {
                    calculatedCheckNumber = 1;
                }
                else
                {
                    calculatedCheckNumber = 0;
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
            var mgnr = membershipNumber.Trim();
            return Convert.ToUInt16(mgnr.Substring(mgnr.Length - 1, 1));
        }
    }
}
