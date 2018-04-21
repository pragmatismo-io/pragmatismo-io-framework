/*****************************************************************************\
*
* Copyright (c) Pragmatismo.io. All rights reserved.
* Licensed under the MIT license.
*
* Pragmatismo.io: http://pragmatismo.io
*
* MIT License:
* Permission is hereby granted, free of charge, to any person obtaining
* a copy of this software and associated documentation files (the
* "Software"), to deal in the Software without restriction, including
* without limitation the rights to use, copy, modify, merge, publish,
* distribute, sublicense, and/or sell copies of the Software, and to
* permit persons to whom the Software is furnished to do so, subject to
* the following conditions:
*
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
* LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
* OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
* WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*
* 
\*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pragmatismo.Io.Framework.Helpers
{
    /// <summary>
    /// Helper methods for the String class.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// A dictionary of synonyms.
        /// </summary>
        public static Dictionary<string, string[]> Synonyms = new Dictionary<string, string[]>();

        /// <summary>
        /// Gets the first name of a string containing multiple names separated by the space character.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        public static string GetFirstName(this string fullName)
        {
            string result = fullName;
            string[] names = fullName.Split(' ');
            result = names[0];
            return result;
        }

        /// <summary>
        /// Sets a string to the "N/A" string if it is empty.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns></returns>
        public static string SetToNAIfEmpty(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                s = "N/A";
            }
            return s;
        }

        /// <summary>
        /// Gets the alias from SIP string.
        /// </summary>
        /// <param name="sip">The SIP string.</param>
        /// <returns></returns>
        public static string GetAliasFromSip(string sip)
        {
            string result = string.Empty;
            Regex emailRegex = new Regex("((?<alias>(.)*)@(.)?)|(?<fullName>(.)*)");
            Match match = emailRegex.Match(sip);
            if (match.Groups["alias"].Success)
            {
                result = match.Groups["alias"].Value;
            }
            else if (match.Groups["fullName"].Success)
            {
                result = match.Groups["fullName"].Value;
            }
            return result;
        }

        /// <summary>
        /// Removes tags from a string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns></returns>
        public static string RemoveTags(this string s)
        {
            return Regex.Replace(s, @"<(.|:|\n)*?>", "");
        }

        /// <summary>
        /// Converts a string to a array. If the string is empty, an array of one element containing the
        /// space character is returned.
        /// </summary>
        /// <param name="s">The string.</param>
        public static byte[] ToByteArray(this string s)
        {
            byte[] result;
            if (string.IsNullOrEmpty(s))
            {
                result = new byte[1];
                result[0] = (byte) ' ';
            }
            else
            {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                result = encoding.GetBytes(s);
            }
            return result;
        }

        /// <summary>
        /// Encloses a string in RTF markers.
        /// </summary>
        /// <param name="s">The string.</param>
        public static string EncloseRtf(this string s)
        {
            return @"{\rtf1 " + s + "}";
        }

        /// <summary>
        /// Encloses a string in RTF markers for italics.
        /// </summary>
        /// <param name="s">The string.</param>
        public static string EncloseRtfItalics(this string s)
        {
            return @"\i " + s + @"\i0";
        }

        /// <summary>
        /// Encloses a string in RTF markers for bold.
        /// </summary>
        /// <param name="s">The sstring.</param>
        public static string EncloseRtfBold(this string s)
        {
            return @"\b " + s + @"\b0";
        }

        /// <summary>
        /// Checks whether a string equals to a given list of synonyms.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="synonyms">The synonyms.</param>
        public static bool EqualsToSynonym(this string s, params string[] synonyms)
        {
            return IsEqualToSynonym(s, StringComparison.CurrentCultureIgnoreCase, synonyms);
        }

        /// <summary>
        /// Checks whether a string starts with a synonym from a list.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="synonyms">The synonyms.</param>
        /// <returns></returns>
        public static bool StartsWithSynonym(this string s, params string[] synonyms)
        {
            return StartsWithSynonym(s, StringComparison.CurrentCultureIgnoreCase, synonyms);
        }

        /// <summary>
        /// Checks whether a string starts with a synonym from a list.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="synonyms">The synonyms.</param>
        /// <returns></returns>
        public static bool StartsWithSynonym(this string s, StringComparison comparisonType, params string[] synonyms)
        {
            bool result = false;
            foreach (string synonym in synonyms)
            {
                if (s.StartsWith(synonym, comparisonType))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether the specified string is equal to any synonym from a list of synonyms.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="synonyms">The synonyms.</param>
        public static bool IsEqualToSynonym(this string s, StringComparison comparisonType, params string[] synonyms)
        {
            bool result = false;
            foreach (string synonym in synonyms)
            {
                if (s.Equals(synonym, comparisonType))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
