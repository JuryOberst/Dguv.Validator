using System;
using System.Collections.Generic;
using System.Text;

namespace Dguv.Validator.Format.Checks
{
    public class Check2 : ICheckNumberValidator
    {
        public object Calculate(string membershipNumber)
        {
            int sum = 0, index = 0, calculatedCheckNumber;
            for(var i = 9; i >= 2; i--)
            {
                sum += i * Convert.ToUInt16(membershipNumber.Substring(index, 1));
                index++;
            }

            calculatedCheckNumber = 11 - sum % 11;
            if(calculatedCheckNumber >= 10)
            {
                calculatedCheckNumber -= 10;
            }

            return calculatedCheckNumber;
        }

        public bool Validate(string membershipNumber)
        {
            var originChecknumber = ExtractCheckNumber(membershipNumber);
            var calculatedChecknumber = (int)Calculate(membershipNumber);
            return originChecknumber == calculatedChecknumber;
        }

        public int ExtractCheckNumber(string membershipNumber)
        {
            return Convert.ToUInt16(membershipNumber.Substring(8, 1));
        }
    }
}
