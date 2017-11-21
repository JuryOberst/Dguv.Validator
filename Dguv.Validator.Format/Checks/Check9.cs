using System;
using System.Collections.Generic;
using System.Text;

namespace Dguv.Validator.Format.Checks
{
    public class Check9 : ICheckNumberValidator
    {
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            var trimmed = membershipNumber.Trim();
            
            if(trimmed.Length >= 8)
            {
                if (trimmed.StartsWith("0"))
                {
                    var mgnr_numbers = Array.ConvertAll(trimmed.Substring(1, 6).ToCharArray(), c => (int)Char.GetNumericValue(c));
                    sum = 7 * mgnr_numbers[0] +
                        6 * mgnr_numbers[1] +
                        5 * mgnr_numbers[2] +
                        4 * mgnr_numbers[3] +
                        3 * mgnr_numbers[4] +
                        2 * mgnr_numbers[5];

                    calculatedCheckNumber = sum % 11;

                    calculatedCheckNumber = calculatedCheckNumber <= 1 ? 0 : 11 - calculatedCheckNumber;
                }
            }

            return calculatedCheckNumber;
        }

        public bool Validate(string membershipNumber)
        {
            var originChecknumber = ExtractCheckNumber(membershipNumber);
            var calculatedChecknumber = (int)Calculate(membershipNumber);
            return originChecknumber == calculatedChecknumber;
        }

        private int ExtractCheckNumber(string membershipNumber)
        {
            var mgnr = membershipNumber.Trim();
            return Convert.ToUInt16(mgnr.Substring(7, 1));
        }
    }
}
