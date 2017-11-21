using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Dguv.Validator.Format.Checks
{
    public class Check8 : ICheckNumberValidator
    {
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string concated = String.Empty;
            var trimmed = membershipNumber.Trim();
            trimmed = trimmed.Length == 8 ? trimmed.Substring(1, 5) : trimmed.Substring(0, 5);
            if(trimmed.Length == 5)
            {
                var mgnr_parts = trimmed.ToCharArray();
                var mgnr_numbers = Array.ConvertAll(trimmed.ToCharArray(), c => (int)Char.GetNumericValue(c));
                concated += mgnr_numbers[0];
                concated += 2 * mgnr_numbers[1];
                concated += mgnr_numbers[2];
                concated += 2 * mgnr_numbers[3];
                concated += mgnr_numbers[4];

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
            var mgnr = membershipNumber.Trim();
            return Convert.ToUInt16(mgnr.Substring(mgnr.Length - 1, 1));
        }
    }
}
