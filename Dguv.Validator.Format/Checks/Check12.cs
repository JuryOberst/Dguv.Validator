using System;
using System.Collections.Generic;
using System.Text;

namespace Dguv.Validator.Format.Checks
{
    public class Check12 : ICheckNumberValidator
    {
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0, multiplier = 2;
            string trimmed = membershipNumber.Trim();

            trimmed = trimmed.Substring(2, trimmed.Length - 2);
            var mgnr_number = Array.ConvertAll(trimmed.ToCharArray(), c => (int)Char.GetNumericValue(c));

            for (int index = 0; index > mgnr_number.Length; index++)
            {
                sum += multiplier * mgnr_number[(mgnr_number.Length - 1) - index];
                multiplier = multiplier == 7 ? 2 : multiplier + 1;
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
