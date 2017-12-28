// <copyright file="FormatParser.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dguv.Validator.Format
{
    internal static class FormatParser
    {
        public static IEnumerable<FormatItem> ParseFormat(string format)
        {
            return format.Split(':').Select(ParseItem).Where(x => x != null);
        }

        private static FormatItem ParseItem(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return null;

            int minLength = 0;
            int maxLength = 0;

            var allowsWhitespace = false;
            var lastWasChecksum = false;
            var optionalLevel = 0;
            var inCharacterClass = false;
            var regexFormat = new StringBuilder();
            var formatLength = format.Length;
            for (int charIndex = 0; charIndex < formatLength; charIndex++)
            {
                var ch = format[charIndex];

                string charactersToAdd = null;

                if (ch == '@')
                {
                    if (!lastWasChecksum)
                    {
                        regexFormat.Append("(?<checksum>");
                        lastWasChecksum = true;
                    }

                    charactersToAdd = @"\d";
                }
                else
                {
                    if (lastWasChecksum)
                    {
                        regexFormat.Append(")");
                        lastWasChecksum = false;
                    }

                    switch (ch)
                    {
                        case '#':
                            charactersToAdd = @"\d";
                            break;
                        case '%':
                            charactersToAdd = "[a-zA-Z0-9]";
                            break;
                        case ' ':
                            charactersToAdd = " ";
                            allowsWhitespace = true;
                            break;
                        case '[':
                            regexFormat.Append("[");

                            inCharacterClass = true;

                            if (optionalLevel == 0)
                            {
                                minLength += 1;
                            }

                            maxLength += 1;

                            break;
                        case ']':
                            regexFormat.Append("]");
                            inCharacterClass = false;
                            break;
                        case '|':
                            if (!inCharacterClass)
                            {
                                throw new NotSupportedException($"Das Zeichen {ch} außerhalb von '[' und ']' wird nicht unterstützt.");
                            }

                            break;
                        case '(':
                            regexFormat.Append("(");
                            optionalLevel += 1;
                            break;
                        case ')':
                            regexFormat.Append(")?");
                            optionalLevel -= 1;
                            break;
                        default:
                            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9') ||
                                ch == '-' || ch == ',' || ch == '/')
                            {
                                charactersToAdd = ch.ToString();
                            }
                            else if (ch == '.')
                            {
                                charactersToAdd = @"\.";
                            }
                            else
                            {
                                throw new NotSupportedException($"Die Zeichenklasse {ch} wird nicht unterstützt.");
                            }

                            break;
                    }
                }

                if (charactersToAdd != null)
                {
                    if (optionalLevel == 0 && !inCharacterClass)
                    {
                        minLength += 1;
                    }

                    if (!inCharacterClass)
                    {
                        maxLength += 1;
                    }

                    regexFormat.Append(charactersToAdd);
                }
            }

            if (lastWasChecksum)
            {
                regexFormat.Append(")");
            }

            return new FormatItem(minLength, maxLength, regexFormat.ToString(), allowsWhitespace);
        }
    }
}
