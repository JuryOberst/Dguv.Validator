using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

using CenterCLR.Sgml;

namespace Dguv.Validator.Providers
{
    /// <summary>
    /// Lädt eine Liste von Prüfungen für Unfallversicherungsträger aus dem Internet.
    /// </summary>
    public class DguvHtmlCheckProvider : IDguvCheckProvider
    {
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
                    var doc = SgmlReader.Parse(respStream);
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
        /// <param name="table">
        /// Die zu verarbeitende Tabelle
        /// </param>
        /// <returns>
        /// Die Prüfungen, die anhand der Tabelle erstellt wurden
        /// </returns>
        private static IEnumerable<IDguvNumberCheck> ParseTable(XElement table)
        {
            var items = new List<IDguvNumberCheck>();
            foreach (var row in table.Elements("tr"))
            {
                var cols = row.Elements("td").ToList();
                if (cols.Count != 5)
                    continue;
                var bbnrUv = cols[0].Value.Trim();
                if (!TableParserUtilities.IsNumber(bbnrUv))
                    continue;
                var name = cols[1].Value;
                var minLengthText = cols[2].Value.Trim();
                var maxLengthText = cols[3].Value.Trim();
                var check = TableParserUtilities.CreateCharacterMapCheck(bbnrUv, name, minLengthText, maxLengthText, cols[4].Value.Trim());
                items.Add(check);
            }

            return items;
        }
    }
}
