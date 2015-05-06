using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

using Dguv.Validator.Checks;

namespace Dguv.Validator.Providers
{
    /// <summary>
    /// Lädt eine Liste von Prüfungen für Unfallversicherungsträger aus dem Internet.
    /// </summary>
    public class WebCheckProvider : IDguvCheckProvider
    {
        private static readonly Regex _isNumber = new Regex("^[0-9]+$");

        /// <summary>
        /// Holt oder setzt den zu verwendenden Proxy für das Laden der Daten aus dem Internet.
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        /// Lädt die Prüfungen von Unfallversicherungsträgern.
        /// </summary>
        /// <returns>Ein <see cref="Task"/>, in dem das Laden der Prüfungen
        /// von Unfallversicherungsträgern ausgeführt wird.</returns>
        public async Task<IEnumerable<IDguvNumberCheck>> LoadChecks()
        {
            var request = CreateRequest();
            using (var response = await request.GetResponseAsync())
            {
                using (var respStream = response.GetResponseStream())
                {
                    var doc = CenterCLR.Sgml.SgmlReader.Parse(respStream);
                    var table = doc.Descendants("table").First(x => x.Attributes("class").Any(y => y.Value == "basic"));
                    return ParseTable(table);
                }
            }
        }

        /// <summary>
        /// Erstellt einen neuen HTTP Web-Request.
        /// </summary>
        /// <returns>Der neue HTTP Web-Request</returns>
        protected virtual HttpWebRequest CreateRequest()
        {
            var wr = WebRequest.CreateHttp(new Uri("http://www.dguv.de/de/mediencenter/hintergrund/meldeverfahren/mitgliedsnr/index.jsp"));
            if (Proxy != null)
                wr.Proxy = Proxy;
            return wr;
        }

        /// <summary>
        /// Verarbeitet die Tabelle mit den Informationen zu den Unfallversicherungsträgern
        /// </summary>
        /// <param name="table">Die zu verarbeitende Tabelle</param>
        /// <returns>Die Prüfungen, die anhand der Tabelle erstellt wurden</returns>
        private static IEnumerable<IDguvNumberCheck> ParseTable(XElement table)
        {
            var items = new List<IDguvNumberCheck>();
            foreach (var row in table.Elements("tr"))
            {
                var cols = row.Elements("td").ToList();
                if (cols.Count != 5)
                    continue;
                var bbnrUv = cols[0].Value.Trim();
                if (!_isNumber.IsMatch(bbnrUv))
                    continue;
                var name = cols[1].Value;
                var minLengthText = cols[2].Value.Trim();
                var maxLengthText = cols[3].Value.Trim();
                var minLength = _isNumber.IsMatch(minLengthText) ? Convert.ToInt32(minLengthText, 10) : -1;
                var maxLength = _isNumber.IsMatch(maxLengthText) ? Convert.ToInt32(maxLengthText, 10) : -1;
                var validChars = ParseValidChars(cols[4].Value.Trim());
                items.Add(new CharacterMapCheck(bbnrUv, name, minLength, maxLength, validChars));
            }

            return items;
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
            var parts = info.Split(',').Select(x => x.Trim()).ToList();
            foreach (var part in parts)
            {
                switch (part)
                {
                    case "0-10":
                        result.Append("0123456789A");
                        break;
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
