using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Dguv.Validator.Checks;

namespace Dguv.Validator
{
    /// <summary>
    /// Hilfsfunktionen für den Aufbau der Prüffunktionen
    /// </summary>
    public static class TableParserUtilities
    {
        private static readonly Regex _isNumber = new Regex("^[0-9]+$");

        /// <summary>
        /// Einfache Prüfung, ob <paramref name="number"/> nur Zahlen enthält
        /// </summary>
        /// <param name="number">Der Text, der überprüft wird, ob er nur Zahlen enthält</param>
        /// <returns>true, wenn <paramref name="number"/> nur Zahlen enthält</returns>
        public static bool IsNumber(string number)
        {
            return _isNumber.IsMatch(number);
        }

        /// <summary>
        /// Erstellt ein <see cref="CharacterMapCheck"/>-Objekt, das alle übergebenen Werte enthält.
        /// </summary>
        /// <param name="bbnrUv">Die UV-Betriebsnummer</param>
        /// <param name="name">Der Name des UV-Trägers</param>
        /// <param name="minLength">Minimale Länge der Mitgliedsnummer</param>
        /// <param name="maxLength">Maximale Länge der Mitgliedsnummer</param>
        /// <param name="validCharsList">Die Liste der gültigen Zeichen</param>
        /// <returns>das neue <see cref="CharacterMapCheck"/>-Objekt</returns>
        public static CharacterMapCheck CreateCharacterMapCheck(string bbnrUv, string name, string minLength, string maxLength, string validCharsList)
        {
            var minLengthAsInt = _isNumber.IsMatch(minLength) ? Convert.ToInt32(minLength, 10) : -1;
            var maxLengthAsInt = _isNumber.IsMatch(maxLength) ? Convert.ToInt32(maxLength, 10) : -1;
            var validChars = ParseValidChars(validCharsList.Trim());
            return new CharacterMapCheck(bbnrUv, name, minLengthAsInt, maxLengthAsInt, validChars);
        }

        /// <summary>
        /// Ermittelt die gültigen Zeichen der Mitgliedsnummer aus der Tabelle der DGUV.
        /// </summary>
        /// <param name="info">Die Information über die gültigen Zeichen</param>
        /// <returns>Die gültigen Zeichen für die Mitgliedsnummer</returns>
        private static string ParseValidChars(string info)
        {
            if (string.IsNullOrEmpty(info) || string.Equals(info, "keine Prüfung", StringComparison.OrdinalIgnoreCase))
                return null;

            var result = new StringBuilder();
            var parts = info.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
            foreach (var part in parts)
            {
                switch (part)
                {
                    case "0-10":
                        result.Append("0123456789A");
                        break;
                    case "Blank":
                    case "Leerzeichen":
                        result.Append(" ");
                        break;
                    case "Punkt":
                        result.Append(".");
                        break;
                    case "Komma":
                        result.Append(",");
                        break;
                    default:
                        if (part.Length == 1)
                        {
                            // Einzelnes Zeichen
                            result.Append(part);
                            break;
                        }

                        if (part.Length == 3 && part[1] == '-')
                        {
                            // Zeichen-Bereich
                            var startChar = char.ConvertToUtf32(part, 0);
                            var endChar = char.ConvertToUtf32(part, 2);
                            for (int i = startChar; i <= endChar; i++)
                            {
                                result.Append(char.ConvertFromUtf32(i));
                            }

                            break;
                        }

                        throw new NotSupportedException();
                }
            }

            return result.ToString();
        }
    }
}
