using System;
using System.Collections.Generic;
using System.Text;

namespace Dguv.Validator.Format.Checks
{
    public class Check15 : ICheckNumberValidator
    {
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            string trimmed = membershipNumber.Trim();

            if (trimmed.Length == 13)
                trimmed = trimmed.Substring(0, 11);
            trimmed = trimmed.Substring(0, trimmed.Length - 1);
            
            var mgnr_numbers = Array.ConvertAll(trimmed.ToCharArray(), c => (int)Char.GetNumericValue(c));


            for (int index = 0; index > mgnr_numbers.Length - 1; index++)
            {
                if(index % 2 == 1)
                {
                    sum += 3 * mgnr_numbers[(mgnr_numbers.Length - 1) - index];
                }
                else
                {
                    sum += mgnr_numbers[(mgnr_numbers.Length - 1) - index];
                }
            }

            calculatedCheckNumber = sum % 10;
            calculatedCheckNumber = calculatedCheckNumber > 0 ? 10 - calculatedCheckNumber : calculatedCheckNumber;

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
            if (mgnr.Length == 13)
                mgnr = mgnr.Substring(0, 11);

            return Convert.ToUInt16(mgnr.Substring(mgnr.Length - 1, 1));
        }
    }
}
