// <copyright file="StringExtensions.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System.Text.RegularExpressions;

namespace Dguv.Validator.Format
{
    internal static class StringExtensions
    {
        private static readonly RegexOptions _regexOptions =
#if !NETSTANDARD1_1
            RegexOptions.Compiled |
#endif
            RegexOptions.IgnoreCase;

        private static readonly Regex _extractDigitsRegex = new Regex(@"\D", _regexOptions);

        public static string ExtractDigits(this string s)
        {
            return _extractDigitsRegex.Replace(s, string.Empty);
        }
    }
}
