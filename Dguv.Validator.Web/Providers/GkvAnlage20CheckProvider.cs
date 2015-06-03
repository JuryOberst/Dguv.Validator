using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using CenterCLR.Sgml;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Dguv.Validator.Providers
{
    /// <summary>
    /// Lädt eine Liste von Prüfungen für Unfallversicherungsträger aus dem Internet.
    /// </summary>
    /// <remarks>
    /// Im Gegensatz zu <see cref="DguvHtmlCheckProvider"/> wird eine PDF-Datei von GKV Datenaustausch
    /// geladen und ausgewertet.
    /// </remarks>
    public class GkvAnlage20CheckProvider : IDguvCheckProvider
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
            var pdfLink = await LoadLinkForAnlage20();
            return await ParsePdf(pdfLink);
        }

        /// <summary>
        /// Erstellt einen neuen HTTP Web-Request.
        /// </summary>
        /// <returns>Der neue HTTP Web-Request</returns>
        protected virtual HttpWebRequest CreateRequestGemeinsameRundschreiben()
        {
            var wr = WebRequest.CreateHttp(new Uri("http://www.gkv-datenaustausch.de/arbeitgeber/deuev/gemeinsame_rundschreiben/gemeinsame_rundschreiben.jsp"));
            if (Proxy != null)
                wr.Proxy = Proxy;
            return wr;
        }

        /// <summary>
        /// Verarbeitet die PDF von der angegebenen URL
        /// </summary>
        /// <param name="pdfLink">
        /// Die URL zu der PDF für die Anlage 20
        /// </param>
        /// <returns>
        /// Die Prüfungen, die anhand der Tabelle erstellt wurden
        /// </returns>
        protected virtual async Task<IEnumerable<IDguvNumberCheck>> ParsePdf(Uri pdfLink)
        {
            var request = WebRequest.CreateHttp(pdfLink);
            if (Proxy != null)
                request.Proxy = Proxy;
            using (var response = await request.GetResponseAsync())
            {
                using (var respStream = response.GetResponseStream())
                {
                    var temp = new MemoryStream();
                    respStream.CopyTo(temp);
                    temp.Position = 0;
                    using (var reader = new PdfReader(temp))
                    {
                        return ParsePdf(reader);
                    }
                }
            }
        }

        /// <summary>
        /// Verarbeitung der PDF, die aus einem <see cref="PdfReader"/> gelesen wird.
        /// </summary>
        /// <param name="reader">Der <see cref="PdfReader"/> aus dem gelesen wird</param>
        /// <returns>Die Prüfungen, die aus der PDF erstellt wurden</returns>
        protected IEnumerable<IDguvNumberCheck> ParsePdf(PdfReader reader)
        {
            var parser = new PdfTableParser();

            for (int pageIndex = 0; pageIndex != reader.NumberOfPages; ++pageIndex)
            {
                var pageContent = PdfTextExtractor
                    .GetTextFromPage(reader, pageIndex + 1, new LocationTextExtractionStrategy());
                parser.Parse(pageContent);
            }

            var result = parser.GetChecks().ToList();

            return result;
        }

        /// <summary>
        /// Holt die URL für den Download der Anlage 20 der gemeinsamen Rundschreiben.
        /// </summary>
        /// <returns>Die URL für den Download der Anlage 20 als PDF-Datei</returns>
        private async Task<Uri> LoadLinkForAnlage20()
        {
            var request = CreateRequestGemeinsameRundschreiben();
            using (var response = await request.GetResponseAsync())
            {
                using (var respStream = response.GetResponseStream())
                {
                    var doc = SgmlReader.Parse(respStream);
                    var pdfLink = doc.Descendants("a").First(x => x.Value.Contains("Anlage 20"));
                    var href = new Uri(pdfLink.Attribute("href").Value, UriKind.RelativeOrAbsolute);
                    var baseUri = request.RequestUri;
                    return new Uri(baseUri, href);
                }
            }
        }
    }
}
