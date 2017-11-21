using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dguv.Validator.Format.Checks
{
    public class Check4 : ICheckNumberValidator
    {
        public object Calculate(string membershipNumber)
        {
            int calculatedCheckNumber = 0, sum = 0;
            var trimmed = membershipNumber.Trim();
            var mgnr_parts = trimmed.ToCharArray();
            if (trimmed.StartsWith("97"))
            {
                var number = Convert.ToInt64(trimmed);
                if (number < 9700000010 || number > 9700000890)
                {
                    var multiplier = 9;
                    for (int index = 0; index <= 9; index++)
                    {
                        sum += multiplier * Convert.ToUInt16(mgnr_parts[index]);
                        multiplier--;
                    }
                    calculatedCheckNumber = sum % 11;
                    if(calculatedCheckNumber == 1)
                    {
                        calculatedCheckNumber = sum % 10;
                        if(calculatedCheckNumber > 0)
                        {
                            calculatedCheckNumber = 10 - calculatedCheckNumber;
                        }
                        else
                        {
                            if(calculatedCheckNumber > 1)
                            {
                                calculatedCheckNumber = 11 - calculatedCheckNumber;
                            }
                        }
                    }
                }
            }
            else
            {
                sum = 4 * Convert.ToUInt16(mgnr_parts[0]) +
                    3 * Convert.ToUInt16(mgnr_parts[1]) +
                    2 * Convert.ToUInt16(mgnr_parts[2]) +
                    7 * Convert.ToUInt16(mgnr_parts[3]) +
                    6 * Convert.ToUInt16(mgnr_parts[4]) +
                    5 * Convert.ToUInt16(mgnr_parts[5]) +
                    4 * Convert.ToUInt16(mgnr_parts[6]) +
                    3 * Convert.ToUInt16(mgnr_parts[7]) +
                    2 * Convert.ToUInt16(mgnr_parts[8]);
                calculatedCheckNumber = sum % 11;
                calculatedCheckNumber = calculatedCheckNumber > 1 ? 11 - calculatedCheckNumber : 0;
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
            return Convert.ToUInt16(mgnr.Substring(9, 1));
        }
    }
}
