using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain
{
   public static class CommonMethods
    {
        public static int[] cantDigitsPoint(double number)
        {
            int[] result = new int[2];
            int dec0, dec1;
            string[] digits = new string[2];

            if (number.ToString().Contains('.'))
                digits = number.ToString().Split('.');
            else if (number.ToString().Contains(","))
                digits = number.ToString().Split(',');
            else {
                digits[0] = number.ToString();
                digits[1] = "";
            }
           

            dec0 = digits[0] is null ? 0 : digits[0].Length;

            if (digits.Length == 2)
            {
                dec1 = digits[1] is null ? 0 : digits[1].Length;
            }
            else
            {
                dec1 = 0;
            }

            result[0] = dec0;
            result[1] = dec1;
            return result;
        }

        public  struct Complex
        {
            public int real;
            public int imaginary;
            public Complex(int real, int imaginary)
            {
                this.real = real;
                this.imaginary = imaginary;
            }
            public static Complex operator +(Complex one, Complex two)
            {
                return new Complex(one.real + two.real, one.imaginary + two.imaginary);
            }
            public override string ToString()
            {
                return (String.Format("{0} + {1}i", real, imaginary));
            }
        }

        /// <summary>
        /// Returns first part of number.
        /// </summary>
        /// <param name="number">Initial number</param>
        /// <param name="N">Amount of digits required</param>
        /// <returns>First part of number</returns>
        public static int TakeNDigits(int number, int N)
        {
            // this is for handling negative numbers, we are only insterested in postitve number
            number = Math.Abs(number);
            // special case for 0 as Log of 0 would be infinity
            if (number == 0)
                return number;
            // getting number of digits on this input number
            int numberOfDigits = (int)Math.Floor(Math.Log10(number) + 1);

            // check if input number has more digits than the required get first N digits
            if (numberOfDigits >= N)
                return (int)Math.Truncate(number / Math.Pow(10, numberOfDigits - N));
            else
                return number;
        }

        /// <summary>
        /// Splits a double number
        /// </summary>
        /// <param name="value">The number to split</param>
        /// <param name="integerPlaces">The integer place digits</param>
        /// <param name="value">The decimal place digits</param>
        /// <returns>int array with numbers before and after decimal point</returns>
        public static int[] SplitDouble(double value, int integerPlaces, int decimalPlaces)
        {
            var left = (int)Math.Truncate(value);
            var right = (int)((value - left) * Math.Pow(10, decimalPlaces));

            // getting number of digits on left number
            var leftNumberOfDigits = (int)Math.Floor(Math.Log10(left) + 1);
            if (leftNumberOfDigits > integerPlaces)
                left = (int)Math.Truncate(left / Math.Pow(10, leftNumberOfDigits - integerPlaces));

            return new int[] { left, right };
        }

        /// <summary>
        /// Get the integral and floating point portions of a Double
        /// as separate integer values, where the floating point value is 
        /// raised to the specified power of ten, given by 'places'.
        /// </summary>
        public static void Split(double value, int places, out int left, out int right)
        {
            left = (int)Math.Truncate(value);
            right = (int)((value - left) * Math.Pow(10, places));
        }

        /// <summary>
        /// Get the integral and floating point portions of a Double
        /// as separate integer values, where the floating point value is 
        /// raised to the specified power of ten, given by 'places'.
        /// </summary>
        public static void Split(double value, out int left, out int right)
        {
            Split(value, 1, out left, out right);
        }
    }
}