using System;
using System.Collections.Generic;
using System.Text;

namespace Dguv.Validator.Format.Checks
{
    public class Check14 : ICheckNumberValidator
    {
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string trimmed = membershipNumber.Trim();
            
            var mgnr_numbers = Array.ConvertAll(trimmed.Substring(0, trimmed.Length - 1).ToCharArray(), c => (int)Char.GetNumericValue(c));
            
            if(mgnr_numbers.Length == 7)
            {
                sum += 8 * mgnr_numbers[0] +
                    7 * mgnr_numbers[1] +
                    6 * mgnr_numbers[2] +
                    5 * mgnr_numbers[3] +
                    4 * mgnr_numbers[4] +
                    3 * mgnr_numbers[5] +
                    2 * mgnr_numbers[6];
            }

            calculatedCheckNumber = sum % 11;
            calculatedCheckNumber = calculatedCheckNumber <= 1 ? 0 : 11 - calculatedCheckNumber;

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
            string mgnr = membershipNumber.Trim();            
            return Convert.ToUInt16(mgnr.Substring(mgnr.Length - 1, 1));
        }
    }
}
