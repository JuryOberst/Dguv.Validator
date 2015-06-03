using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dguv.Validator.Checks;

namespace Dguv.Validator.Providers
{
    /// <summary>
    /// Statische Liste von Prüfungen für Unfallversicherungsträger
    /// </summary>
    public class StaticCheckProvider : IDguvCheckProvider
    {
        private static readonly IList<IDguvNumberCheck> _checks = new List<IDguvNumberCheck>
            {
                new CharacterMapCheck("01064065", "Unfallkasse Sachsen", 6, 6, "0123456789"),
                new CharacterMapCheck("01681222", "Unfallkasse Mecklenburg-Vorpommern", 7, 12, "0123456789P-"),
                new CharacterMapCheck("02379637", "Unfallkasse Brandenburg", 5, 5, "0123456789"),
                new CharacterMapCheck("03701377", "Unfallkasse Sachsen-Anhalt", -1, -1, string.Empty),
                new CharacterMapCheck("07235792", "Unfallkasse Thüringen", 10, 13, "0123456789/."),
                new CharacterMapCheck("14066582", "Hauptverwaltung Berlin", 7, 17, "0123456789/-BEFNMXZ ."),
                new CharacterMapCheck("15087927", "Bezirksverwaltung Hamburg (gültig bis einschl. 2011)", 10, 17, "0123456789M ."),
                new CharacterMapCheck("15141364", "Bereich Fahrzeughaltungen (ehemals Berufsgenossenschaft für Fahrzeughaltungen)", 9, 9, "0123456789"),
                new CharacterMapCheck("15186676", "Berufsgenossenschaft für Gesundheitsdienst und Wohlfahrtspflege", 10, 10, "0123456789SABCDEFGHIJKLM"),
                new CharacterMapCheck("15197214", "BG der Straßen, U-Bahnen, Eisenbahnen (gültig bis einschl. 2009)", 9, 9, "0123456789"),
                new CharacterMapCheck("15250094", "Verwaltungs-Berufsgenossenschaft", 10, 10, "0123456789"),
                new CharacterMapCheck("16716004", "Unfallkasse Nord", 5, 11, "0123456789."),
                new CharacterMapCheck("18477668", "Kommunale Unfallversicherung Bayern (ehem. Unfallkasse München)", 5, 9, "0123456789"),
                new CharacterMapCheck("18484827", "Branche Papierherstellung und Ausrüstung (ehemals Papiermacher-Berufsgenossenschaft)", 9, 9, "0123456789"),
                new CharacterMapCheck("18484877", "Branche Zucker (ehemals Zucker-Berufsgenossenschaft)", 9, 9, "0123456789"),
                new CharacterMapCheck("18626026", "Landesunfallkasse Niedersachsen", 14, 14, "0123456789."),
                new CharacterMapCheck("20345417", "Unfallkasse Freie Hansestadt Bremen", -1, -1, string.Empty),
                new CharacterMapCheck("21204943", "Braunschweigischer GUV", -1, -1, string.Empty),
                new CharacterMapCheck("26125562", "Gemeinde-Unfallversicherungsverband Oldenburg", -1, -1, string.Empty),
                new CharacterMapCheck("28143238", "Unfallkasse des Bundes", -1, -1, string.Empty),
                new CharacterMapCheck("29029801", "Branche Baustoffe - Steine - Erden (ehemals Steinbruchs-Berufsgenossenschaft)", 9, 11, "0123456789."),
                new CharacterMapCheck("29036720", "Bezirksverwaltung Hannover (gültig bis einschl. 2011)", 10, 17, "0123456789M ."),
                new CharacterMapCheck("29059513", "Berufsgenossenschaft Metall Nord Süd Bereich Nord (gültig bis einschl. 2009)", 9, 9, "0123456789A"),
                new CharacterMapCheck("29086457", "Gemeinde-Unfallversicherungsverband Hannover", 14, 14, "0123456789."),
                new CharacterMapCheck("29214533", "FUK Niedersachsen", -1, -1, string.Empty),
                new CharacterMapCheck("31608112", "Branche Bergbau (ehemals Bergbau-Berufsgenossenschaft)", 11, 11, "0123456789-"),
                new CharacterMapCheck("32064004", "Sparte Einzelhandel", 10, 10, "0123456789-"),
                new CharacterMapCheck("34217193", "Maschinenbau- und Metall-Berufsgenossenschaft (gültig bis einschl. 2012)", 9, 9, "0123456789"),
                new CharacterMapCheck("34239086", "Unfallkasse Nordrhein-Westfalen", -1, -1, string.Empty),
                new CharacterMapCheck("34364283", "Hütten und Walzwerks-Berufsgenossenschaft (gültig bis einschl. 2012)", 9, 9, "0123456789"),
                new CharacterMapCheck("34364294", "Branchenverwaltung Energie und Wasserwirtschaft (ehemals Berufsgenossenschaft der Gas-, Fernwärme- und Wasserwirtschaft)", 6, 8, "0123456789/"),
                new CharacterMapCheck("37916971", "ehemals Berufsgenossenschaft Elektro Textil Feinmechanik", 8, 16, "0123456789 -"),
                new CharacterMapCheck("42884688", "Bezirksverwaltung Wuppertal (gültig bis einschl. 2011)", 9, 17, "0123456789/-BEFNXZM ."),
                new CharacterMapCheck("44861264", "Unfallkasse Hessen", 10, 13, "0123456789/."),
                new CharacterMapCheck("44888436", "Bezirksverwaltung Frankfurt (gültig bis einschl. 2011)", 10, 17, "0123456789M "),
                new CharacterMapCheck("48626018", "Branchenverwaltung Druck und Papierverarbeitung (ehemals Berufsgenossenschaft Druck und Papierverarbeitung)", 10, 12, "0123456789 -"),
                new CharacterMapCheck("49005902", "Eisenbahn-Unfallkasse", 4, 4, "0123456789"),
                new CharacterMapCheck("52717470", "Branche Lederindustrie (ehemals Lederindustrie-Berufsgenossenschaft)", 9, 9, "0123456789"),
                new CharacterMapCheck("52738475", "BGN Bereich Fleischereiwirtschaft", 7, 8, "0123456789.,"),
                new CharacterMapCheck("52742028", "Berufsgenossenschaft Holz und Metall", 9, 9, "0123456789"),
                new CharacterMapCheck("53149588", "Unfallkasse Rheinland-Pfalz", 10, 11, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"),
                new CharacterMapCheck("55423519", "Unfallkasse Saarland", 3, 3, "0123456789"),
                new CharacterMapCheck("61635458", "Branche Chemische Industrie (ehemals Berufsgenossenschaft der chemischen Industrie)", 7, 9, "0123456789/"),
                new CharacterMapCheck("62279404", "Bezirksverwaltung Karlsruhe (gültig bis einschl. 2011)", 10, 17, "0123456789M/- ."),
                new CharacterMapCheck("63800761", "Berufsgenossenschaft Nahrungsmittel und Gastgewerbe", 11, 17, "0123456789M ."),
                new CharacterMapCheck("63886548", "Sparte Großhandel und Lagerei (gültig bis einschl. 2013)", 8, 8, "0123456789"),
                new CharacterMapCheck("66337061", "Unfallkasse Post und Telekom", 4, 4, "0123456789"),
                new CharacterMapCheck("67334480", "Unfallkasse Baden-Württemberg", 10, 17, "0123456789 "),
                new CharacterMapCheck("67350937", "Bezirksverwaltung Böblingen (gültig bis einschl. 2011)", 11, 17, "0123456789M ."),
                new CharacterMapCheck("87661138", "Bezirksverwaltung München Hochbau (gültig bis einschl. 2011)", 7, 17, "0123456789M ."),
                new CharacterMapCheck("87661183", "Bezirksverwaltung München Tiefbau (gültig bis einschl. 2011)", 7, 17, "0123456789M .KL-"),
                new CharacterMapCheck("87661207", "Kommunale Unfallversicherung Bayern (ehem. Unfallkasse München)", 6, 6, "0123456789"),
                new CharacterMapCheck("87741942", "Holz-Berufsgenossenschaft (gültig bis einschl. 2012)", 9, 11, "0123456789 "),
                new CharacterMapCheck("88270171", "Bayerische Landesunfallkasse", 6, 6, "0123456789"),
                new CharacterMapCheck("90276713", "Unfallkasse Berlin", 9, 9, "0123456789"),
                new CharacterMapCheck("98705576", "FUK Mitte", -1, -1, string.Empty),
                new CharacterMapCheck("99011352", "Bereich Seeschifffahrt (ehemals See-Berufsgenossenschaft)", 8, 8, "0123456789"),
            };

        /// <summary>
        /// Holt eine Liste von Prüfungen für Unfallversicherungsträger
        /// </summary>
        public IEnumerable<IDguvNumberCheck> Checks
        {
            get { return _checks; }
        }

        /// <summary>
        /// Lädt die Prüfungen von Unfallversicherungsträgern.
        /// </summary>
        /// <returns>Ein <see cref="Task"/>, in dem das Laden der Prüfungen
        /// von Unfallversicherungsträgern ausgeführt wird.</returns>
        public Task<IEnumerable<IDguvNumberCheck>> LoadChecks()
        {
            var result = new TaskCompletionSource<IEnumerable<IDguvNumberCheck>>();
            result.SetResult(_checks);
            return result.Task;
        }
    }
}
