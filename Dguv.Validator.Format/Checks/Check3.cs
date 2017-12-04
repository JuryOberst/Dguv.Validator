// <copyright file="Check3.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;

namespace Dguv.Validator.Format.Checks
{
    /// <summary>
    /// Prüfziffernberechnung 3
    /// </summary>
    public class Check3 : ICheckNumberValidator
    {
        /// <summary>
        /// Berechnung der Prüfziffer anhand der Mitgliedsnummer
        /// </summary>
        /// <param name="membershipNumber">Die Mitgliedsnummer</param>
        /// <returns>Die errechnete Prüfziffer</returns>
        public object Calculate(string membershipNumber)
        {
            int p1 = 0, p2 = 0, sum = 0, prz = 0;
            string calculatedCheckNumber = string.Empty;
            bool oldAlgorithmus = false, newSummaryAlgorithmus = false;
            string firstChar = membershipNumber.ToUpper().Substring(0, 1);
            int[] mgnr_numbers = Array.ConvertAll(membershipNumber.Substring(1, 6).ToCharArray(), c => (int)char.GetNumericValue(c));

            if (firstChar == "M")
            {
                p1 = 13;
                oldAlgorithmus = true;
            }
            if (firstChar == "S")
            {
                p1 = 13;
                if (Convert.ToUInt16($"{mgnr_numbers[0]}{mgnr_numbers[1]}") <= 23)
                {
                    oldAlgorithmus = true;
                }
            }

            if (oldAlgorithmus)
            {
                sum = (2 * p1) +
                    (7 * Convert.ToUInt16(mgnr_numbers[0])) +
                    (6 * Convert.ToUInt16(mgnr_numbers[1])) +
                    (5 * Convert.ToUInt16(mgnr_numbers[2])) +
                    (4 * Convert.ToUInt16(mgnr_numbers[3])) +
                    (3 * Convert.ToUInt16(mgnr_numbers[4])) +
                    (2 * Convert.ToUInt16(mgnr_numbers[5]));

                prz = sum > 0 ? 11 - (sum % 11) : 0;

                switch (prz)
                {
                    case 1:
                        calculatedCheckNumber = "A";
                        break;
                    case 2:
                        calculatedCheckNumber = "B";
                        break;
                    case 3:
                        calculatedCheckNumber = "C";
                        break;
                    case 4:
                        calculatedCheckNumber = "D";
                        break;
                    case 5:
                        calculatedCheckNumber = "E";
                        break;
                    case 6:
                        calculatedCheckNumber = "F";
                        break;
                    case 7:
                        calculatedCheckNumber = "G";
                        break;
                    case 8:
                        calculatedCheckNumber = "H";
                        break;
                    case 9:
                        calculatedCheckNumber = "J";
                        break;
                    case 10:
                        calculatedCheckNumber = "K";
                        break;
                    case 11:
                        calculatedCheckNumber = "L";
                        break;
                }
            }
            else
            {
                if (firstChar[0] == 'A')
                {
                    p2 = 1;
                    newSummaryAlgorithmus = true;
                }

                if (firstChar[0] == 'F' && mgnr_numbers[0] == 9)
                {
                    p2 = 6;
                    newSummaryAlgorithmus = true;
                }

                if (newSummaryAlgorithmus)
                    sum = 2 * p2;

                sum += (7 * mgnr_numbers[0]) +
                    (6 * mgnr_numbers[1]) +
                    (5 * mgnr_numbers[2]) +
                    (4 * mgnr_numbers[3]) +
                    (3 * mgnr_numbers[4]) +
                    (2 * mgnr_numbers[5]);

                if (sum > 0)
                {
                    prz = sum % 11;
                    prz = prz <= 1 ? 0 : 11 - prz;
                }

                switch (prz)
                {
                    case 1:
                        calculatedCheckNumber = "A";
                        break;
                    case 2:
                        calculatedCheckNumber = "B";
                        break;
                    case 3:
                        calculatedCheckNumber = "C";
                        break;
                    case 4:
                        calculatedCheckNumber = "D";
                        break;
                    case 5:
                        calculatedCheckNumber = "E";
                        break;
                    case 6:
                        calculatedCheckNumber = "F";
                        break;
                    case 7:
                        calculatedCheckNumber = "G";
                        break;
                    case 8:
                        calculatedCheckNumber = "H";
                        break;
                    case 9:
                        calculatedCheckNumber = "K";
                        break;
                    case 0:
                        calculatedCheckNumber = "L";
                        break;
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
            var calculatedChecknumber = (string)Calculate(membershipNumber);
            return originChecknumber == calculatedChecknumber;
        }

        private string ExtractCheckNumber(string membershipNumber)
        {
            return membershipNumber.Substring(8, 1);
        }
    }
}
