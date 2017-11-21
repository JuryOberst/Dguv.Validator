using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    public class Check16 : ICheckNumberValidator
    {
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string trimmed = membershipNumber.Trim();
            trimmed = trimmed.Substring(0, 7);
            
            var mgnr_numbers = Array.ConvertAll(trimmed.Substring(0, trimmed.Length - 1).ToCharArray(), c => (int)Char.GetNumericValue(c));
            string concated = String.Empty;
            if(mgnr_numbers.Length == 6)
            {
                concated = concated + (2 * mgnr_numbers[0]);
                concated = concated + mgnr_numbers[1];
                concated = concated + (2 * mgnr_numbers[2]);
                concated = concated + (5 * mgnr_numbers[3]);
                concated = concated + (7 * mgnr_numbers[4]);
                concated = concated + mgnr_numbers[0];
                sum = Array.ConvertAll(concated.ToCharArray(), c => (int)Char.GetNumericValue(c)).Sum();
                calculatedCheckNumber = sum % 10;
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
            string mgnr = membershipNumber.Trim();
            return Convert.ToUInt16(mgnr.Substring(mgnr.Length - 1, 1));
        }
    }
}
