using System;
using System.Collections.Generic;
using System.Text;

namespace Dguv.Validator.Format.Checks
{
    public class Check10 : ICheckNumberValidator
    {
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string trimmed = String.Empty;
            if(membershipNumber.Substring(1, 1) == "/")
                trimmed = membershipNumber.Substring(2, membershipNumber.Length - 2).Trim();

            var mgnr_number = Array.ConvertAll(trimmed.ToCharArray(), c => (int)char.GetNumericValue(c));
            
            if(trimmed.Length >= 8)
            {
                sum = 2 * mgnr_number[0] +
                    3 * mgnr_number[1] +
                    7 * mgnr_number[2] +
                    6 * mgnr_number[3] +
                    5 * mgnr_number[4] +
                    4 * mgnr_number[5] +
                    3 * mgnr_number[6];

                calculatedCheckNumber = sum % 11;

                calculatedCheckNumber = calculatedCheckNumber <= 1 ? 0 : 11 - calculatedCheckNumber;
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
            int checkNumber = 0;
            string mgnr = membershipNumber;
            if (membershipNumber.Substring(1, 1) == "/")
                mgnr = membershipNumber.Substring(2, mgnr.Length - 2).Trim();
            if (mgnr.Length >= 8)
                checkNumber = Convert.ToUInt16(mgnr.Substring(7, 1));
            return checkNumber;
        }
    }
}
