using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Extensions
{
    public static class StringExtensions
    {
        public static string ToConstantCase(this string value) //TODO: rewrite to stackalloc/ string.Create()
        {
            int i;
            int strLength = value.Length;
            // iterate through each character in the string, stopping a character short of the end
            for (i = 0; i < strLength - 1; ++i)
            {
                var curChar = value[i];
                var nextChar = value[i + 1];
                // look for the pattern [a-z][A-Z]
                if (char.IsLower(curChar) && char.IsUpper(nextChar))
                {
                    InsertUnderscore();
                    // then skip the remaining match checks since we already found a match here
                    continue;
                }
                // look for the pattern [0-9][A-Za-z]
                if (char.IsDigit(curChar) && char.IsLetter(nextChar))
                {
                    InsertUnderscore();
                    continue;
                }
                // look for the pattern [A-Za-z][0-9]
                if (char.IsLetter(curChar) && char.IsDigit(nextChar))
                {
                    InsertUnderscore();
                    continue;
                }
                // if there's enough characters left, look for the pattern [A-Z][A-Z][a-z]
                if (i < strLength - 2 && char.IsUpper(curChar) && char.IsUpper(nextChar) && char.IsLower(value[i + 2]))
                {
                    InsertUnderscore();
                    continue;
                }
            }
            // convert the resulting string to uppercase
            return value.ToUpperInvariant();

            void InsertUnderscore()
            {
                // add an underscore between the two characters, increment i to skip the underscore, and increase strLength because the string is longer now
                value = value.Substring(0, ++i) + '_' + value.Substring(i);
                ++strLength;
            }
        }

    }
}
