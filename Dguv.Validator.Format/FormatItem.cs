// <copyright file="FormatItem.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

namespace Dguv.Validator.Format
{
    internal class FormatItem
    {
        public FormatItem(int minLength, int maxLength, string regExPattern, bool allowWhitespace)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            RegExPattern = regExPattern;
            AllowWhitespace = allowWhitespace;
        }

        public int MinLength { get; }
        public int MaxLength { get; }
        public string RegExPattern { get; }
        public bool AllowWhitespace { get; }
    }
}
