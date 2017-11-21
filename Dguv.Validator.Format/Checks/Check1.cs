using System;
using System.Collections.Generic;
using System.Text;

namespace Dguv.Validator.Format.Checks
{
    public class Check1 : ICheckNumberValidator
    {
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
            if(calculatedCheckNumber > 1)
            {
                calculatedCheckNumber = 11 - calculatedCheckNumber;
            }
            else
            {
                if(calculatedCheckNumber == 0)
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
