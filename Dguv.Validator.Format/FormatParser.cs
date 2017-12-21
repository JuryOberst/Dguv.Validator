// <copyright file="FormatParser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dguv.Validator.Format
{
    internal static class FormatParser
    {
        internal static (int?, int?, string[]) ParseFormat(string format)
        {
            int? minLength = null;
            int? maxLength = null;

            List<string> regExPatterns = new List<string>();

            var chars = new char[] { '(', ')' };
            if (format != string.Empty)
            {
                // Trennen der einzelnen Formate falls mehrere vorhanden sind
                string[] splitted = format.Split(':');

                minLength = splitted.OrderBy(s => s.Length).First().Length;
                maxLength = splitted.OrderByDescending(s => s.Length).First().Length;

                foreach (var split in splitted)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("^");
                    string part = string.Empty;
                    var enumerator = split.GetEnumerator();
                    enumerator.MoveNext();
                    int charCount = 0;
                    char lastChar = enumerator.Current;
                    do
                    {
                        if (lastChar != enumerator.Current || chars.Contains(enumerator.Current))
                        {
                            builder.Append(part);
                            if (charCount > 1)
                                builder.Append($"{{{charCount}}}");
                            charCount = 0;
                        }

                        switch (enumerator.Current)
                        {
                            case '#':
                            case '@':
                                part = @"\d";
                                break;
                            case '%':
                                part = "[0-9a-zA-Z]";
                                break;
                            case ' ':
                                part = @" ";
                                break;
                            case '.':
                                part = @"\.";
                                break;
                            case ',':
                                part = @"\,";
                                break;
                            case '[':
                                part = @"(";
                                break;
                            case ']':
                                part = @")";
                                break;
                            case ')':
                                part = $"{enumerator.Current.ToString()}?";
                                charCount--;
                                break;
                            default:
                                part = $"{enumerator.Current.ToString()}";
                                break;
                        }

                        lastChar = enumerator.Current;
                        charCount++;
                    }
                    while (enumerator.MoveNext());

                    builder.Append(part);
                    if (charCount > 0)
                        builder.Append($"{{{charCount}}}");
                    builder.Append("$");
                    regExPatterns.Add(builder.ToString());
                }
            }
            return (minLength, maxLength, regExPatterns.ToArray());
        }
    }
}
