using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dguv.Validator
{
    /// <summary>
    /// Diese Klasse analysiert die Texte der PDF-Datei der Anlage 20 der gemeinsamen Rundschreiben
    /// </summary>
    public class PdfTableParser
    {
        private readonly List<Info> _infos = new List<Info>();

        private RowStatus _rowStatus = RowStatus.FirstLineExpected;

        private Info _currentInfo;

        private enum RowStatus
        {
            FirstLineExpected,
            FirstLineParsed,
        }

        /// <summary>
        /// Liefert eine Liste der Prüffunktionen, die anhand der Anlage 20 erstellt wurden.
        /// </summary>
        /// <returns>Eine Liste der Prüffunktionen, die anhand der Anlage 20 erstellt wurden</returns>
        public IReadOnlyList<IDguvNumberCheck> GetChecks()
        {
            var result = _infos
                .Select(x => TableParserUtilities.CreateCharacterMapCheck(x.BbnrUv, x.Name.ToString(), x.MinLength, x.MaxLength, x.ValidChars))
                .ToList();
            return result;
        }

        /// <summary>
        /// Analysiert den Text einer PDF-Seite und extrahiert daraus die Prüffunktionen, die über <see cref="GetChecks"/> abgefragt werden können.
        /// </summary>
        /// <param name="pageContent">Der Text der PDF-Seite, die analysiert wird</param>
        public void Parse(string pageContent)
        {
            var lines = pageContent.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim());
            var tableStarted = false;
            foreach (var line in lines)
            {
                if (!tableStarted)
                {
                    // Tabellen-Start ist, wenn wir BBNR-UV am Anfang einer Zeile finden
                    if (line.StartsWith("MNR MNR"))
                        tableStarted = true;
                    continue;
                }

                var parts = line.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (_rowStatus == RowStatus.FirstLineExpected)
                {
                    // Wenn der Inhalt der Zeile eindeutig ungültig ist, dann wird er ignoriert
                    if (parts.Length != 2 || !TableParserUtilities.IsNumber(parts[0]))
                        continue;
                }

                if (_rowStatus == RowStatus.FirstLineParsed)
                {
                    // Wenn der Wert in der ersten Spalte eine Nummer ist, dann gehen wir
                    // davon aus, dass wir eine Betriebsnummer gefunden haben
                    if (TableParserUtilities.IsNumber(parts[0]))
                    {
                        _rowStatus = RowStatus.FirstLineExpected;
                    }
                    else if (parts[0].StartsWith("Stand:", StringComparison.Ordinal))
                    {
                        // Ende der Tabelle
                        break;
                    }
                }

                if (_rowStatus == RowStatus.FirstLineExpected)
                {
                    // Neues Info-Objekt erstellen, anhand dessen wir die Prüffunktion erstellen
                    _currentInfo = new Info(parts[0], parts[1]);
                    _infos.Add(_currentInfo);
                    _rowStatus = RowStatus.FirstLineParsed;
                }
                else
                {
                    // Name verlängern
                    _currentInfo.Name.AppendFormat(" {0}", line.TrimEnd());
                }
            }
        }

        /// <summary>
        /// Das Informations-Objekt, das zum Sammeln der Prüf-Informationen verwendet wird
        /// </summary>
        private class Info
        {
            private const string KeinePruefung = "Keine Prüfung";

            /// <summary>
            /// Initialisiert eine neue Instanz der <see cref="Info"/> Klasse.
            /// </summary>
            /// <param name="bbnrUv">Die Betriebsnummer</param>
            /// <param name="remaining">Der Rest der Zeile, die nach der Betriebsnummer folgt</param>
            public Info(string bbnrUv, string remaining)
            {
                BbnrUv = bbnrUv;

                // Der Rest der Zeile wird von Hinten nach Vorne analysiert,
                // da in der PDF-Datei wir keine Information haben, wo eine
                // Spalte anfängt, bzw. aufhört.
                remaining = ParseValidChars(remaining.TrimEnd());
                remaining = ParseMaxLength(remaining);
                remaining = ParseMinLength(remaining);

                // Das, was zwischen minimaler Länge und Betriebsnummer ist, muss der Name sein.
                Name = new StringBuilder(remaining);
            }

            /// <summary>
            /// Holt die Betriebsnummer des UV-Trägers
            /// </summary>
            public string BbnrUv { get; private set; }

            /// <summary>
            /// Holt den namen des UV-Trägers
            /// </summary>
            public StringBuilder Name { get; private set; }

            /// <summary>
            /// Holt die minimale Länge der Mitgliedsnummer
            /// </summary>
            public string MinLength { get; private set; }

            /// <summary>
            /// Holt die maximale Länge der Mitgliedsnummer
            /// </summary>
            public string MaxLength { get; private set; }

            /// <summary>
            /// Holt die gültigen Zeichen der Mitgliedsnummer
            /// </summary>
            public string ValidChars { get; private set; }

            private string ParseMaxLength(string text)
            {
                var result = ParseLength(text);
                MaxLength = result.Item1;
                return result.Item2;
            }

            private string ParseMinLength(string text)
            {
                var result = ParseLength(text);
                MinLength = result.Item1;
                return result.Item2;
            }

            private string ParseValidChars(string text)
            {
                if (text.EndsWith(KeinePruefung, StringComparison.OrdinalIgnoreCase))
                {
                    ValidChars = string.Empty;
                    return text.Substring(0, text.Length - KeinePruefung.Length).TrimEnd();
                }

                var pos = text.LastIndexOf(' ');
                while (text[pos - 1] == ',')
                    pos = text.LastIndexOf(' ', pos - 1);

                ValidChars = text.Substring(pos + 1);
                return text.Substring(0, pos).TrimEnd();
            }

            private Tuple<string, string> ParseLength(string text)
            {
                if (text.EndsWith(KeinePruefung, StringComparison.OrdinalIgnoreCase))
                    return Tuple.Create(KeinePruefung, text.Substring(0, text.Length - KeinePruefung.Length).TrimEnd());

                var numChars = "0123456789";
                var pos = text.Length - 1;
                while (pos >= 0 && numChars.IndexOf(text[pos]) != -1)
                    --pos;

                return Tuple.Create(text.Substring(pos + 1), text.Substring(0, pos + 1).TrimEnd());
            }
        }
    }
}
