// <copyright file="StaticCheckProvider.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Dguv.Validator.Checks;

namespace Dguv.Validator.Providers
{
    /// <summary>
    /// Statische Liste von Prüfungen für Unfallversicherungsträger
    /// </summary>
    public class StaticCheckProvider : IDguvCheckProvider
    {
        [SuppressMessage("StyleCopPlus.StyleCopPlusRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        private static readonly IList<IDguvNumberCheck> _checks = new List<IDguvNumberCheck>
            {
                new CharacterMapCheck("01064065", "Unfallkasse Sachsen", 6, 6, "0123456789"),
                new CharacterMapCheck("01627953", "Hanseatische Feuerwehr-Unfallkasse Nord", null, null, null),
                new CharacterMapCheck("01681222", "Unfallkasse Mecklenburg-Vorpommern", 7, 12, "0123456789P/-"),
                new CharacterMapCheck("02379637", "Unfallkasse und Feuerwehrunfallkasse Brandenburg", null, null, null),
                new CharacterMapCheck("03701377", "Unfallkasse Sachsen-Anhalt", null, null, null),
                new CharacterMapCheck("07235792", "Unfallkasse Thüringen", 10, 13, "0123456789/."),
                new CharacterMapCheck("08270878", "SVLFG, LBG, Standort Hoppegarten", null, null, null),
                new CharacterMapCheck("09322747", "Feuerwehr-Unfallkasse Mitte", null, null, null),
                new CharacterMapCheck("13174962", "SVLFG, LBG, Standort Kiel", null, null, null),
                new CharacterMapCheck("13385729", "Hanseatische Feuerwehr-Unfallkasse Nord", null, null, null),
                new CharacterMapCheck("14066582", "Berufsgenossenschaft der Bauwirtschaft", 7, 20, "0123456789/-BEFNMXZ ."),
                new CharacterMapCheck("15087927", "Berufsgenossenschaft der Bauwirtschaft – Hamburg", null, null, null),
                new CharacterMapCheck("15141364", "Berufsgenossenschaft Verkehrswirtschaft Post-Logistik Telekommunikation Sparte Fahrzeughaltungen (ehemals Berufsgenossenschaft für Fahrzeughaltungen)", 9, 9, "0123456789"),
                new CharacterMapCheck("15186676", "Berufsgenossenschaft für Gesundheitsdienst und Wohlfahrtspflege", 10, 10, "0123456789SABCDEFGHIJKLM"),
                new CharacterMapCheck("15197214", "Verwaltungs-Berufsgenossenschaft (ehemals Berufsgenossenschaft der Straßen-, U-Bahnen und Eisenbahnen)", 9, 9, "0123456789"),
                new CharacterMapCheck("15250094", "Verwaltungs-Berufsgenossenschaft", 10, 10, "0123456789"),
                new CharacterMapCheck("16716004", "Unfallkasse Nord", 5, 11, "0123456789."),
                new CharacterMapCheck("18477668", "Kommunale Unfallversicherung Bayern (ehemals Unfallkasse München)", 5, 9, "0123456789"),
                new CharacterMapCheck("18484827", "Berufsgenossenschaft Rohstoffe und chemische Industrie Branche Papierherstellung und Ausrüstung (ehemals Papiermacher-Berufsgenossenschaft)", 7, 9, "0123456789"),
                new CharacterMapCheck("18484877", "Berufsgenossenschaft Rohstoffe und chemische Industrie Branche Zucker (ehemals Zucker-Berufsgenossenschaft)", 7, 9, "0123456789"),
                new CharacterMapCheck("18626026", "Landesunfallkasse Niedersachsen", 14, 14, "0123456789."),
                new CharacterMapCheck("18645029", "Hanseatische Feuerwehr-Unfallkasse Nord", null, null, null),
                new CharacterMapCheck("20345417", "Unfallkasse Freie Hansestadt Bremen", null, null, null),
                new CharacterMapCheck("21204943", "Braunschweigischer Gemeindeunfallversicherungsverband", null, null, null),
                new CharacterMapCheck("26125562", "Gemeinde-Unfallversicherungsverband Oldenburg", null, null, null),
                new CharacterMapCheck("28143238", "Unfallversicherung Bund und Bahn – Bereich Bund (ehemals Unfallkasse des Bundes)", null, null, null),
                new CharacterMapCheck("29029801", "Berufsgenossenschaft Rohstoffe und chemische Industrie Branche Baustoffe – Steine – Erden (ehemals Steinbruchs-Berufsgenossenschaft)", 7, 11, "0123456789."),
                new CharacterMapCheck("29036720", "Berufsgenossenschaft der Bauwirtschaft – Hannover", null, null, null),
                new CharacterMapCheck("29059513", "Berufsgenossenschaft Holz und Metall (ehemals Berufsgenossenschaft Metall Nord Süd – Bereich Nord)", 9, 9, "0123456789"),
                new CharacterMapCheck("29086457", "Gemeinde-Unfallversicherungsverband Hannover", 14, 14, "0123456789."),
                new CharacterMapCheck("29139336", "SVLFG, LBG, Standort Hannover", null, null, null),
                new CharacterMapCheck("29214533", "Feuerwehr-Unfallkasse Niedersachsen", null, null, null),
                new CharacterMapCheck("31608112", "Berufsgenossenschaft Rohstoffe und chemische Industrie Branche Bergbau (ehemals Bergbau-Berufsgenossenschaft)", 7, 11, "0123456789-"),
                new CharacterMapCheck("32064004", "Berufsgenossenschaft Handel und Warenlogistik", 10, 10, "0123456789-"),
                new CharacterMapCheck("34217193", "Berufsgenossenschaft Holz und Metall (ehemals Maschinenbau- und Metall-Berufsgenossenschaft)", 9, 9, "0123456789"),
                new CharacterMapCheck("34239086", "Unfallkasse Nordrhein-Westfalen", null, null, null),
                new CharacterMapCheck("34364283", "Berufsgenossenschaft Holz und Metall (ehemals Hütten und Walzwerks-Berufsgenossenschaft)", 9, 9, "0123456789"),
                new CharacterMapCheck("34364294", "Berufsgenossenschaft Energie Textil Elektro Medienerzeugnisse Branchenverwaltung Energie und Wasserwirtschaft (ehemals Berufsgenossenschaft der Gas-, Fernwärme- und Wasserwirtschaft)", 6, 8, "0123456789/"),
                new CharacterMapCheck("37916971", "Berufsgenossenschaft Energie Textil Elektro Medienerzeugnisse (ehemals Berufsgenossenschaft Elektro Textil Feinmechanik)", 8, 16, "0123456789 -"),
                new CharacterMapCheck("39892693", "SVLFG, LBG, Standort Münster", null, null, null),
                new CharacterMapCheck("42884688", "Berufsgenossenschaft der Bauwirtschaft – Wuppertal", null, null, null),
                new CharacterMapCheck("44861264", "Unfallkasse Hessen", 10, 13, "0123456789/."),
                new CharacterMapCheck("44888436", "Berufsgenossenschaft der Bauwirtschaft – Frankfurt", null, null, null),
                new CharacterMapCheck("47009510", "SVLFG, LBG, Standort Kassel (ehemals Gartenbau-BG)", null, null, null),
                new CharacterMapCheck("47042806", "SVLFG, LBG, Standort Darmstadt", null, null, null),
                new CharacterMapCheck("48626018", "Berufsgenossenschaft Energie Textil Elektro Medienerzeugnisse Branchenverwaltung Druck und Papierverarbeitung (ehemals Berufsgenossenschaft Druck und Papierverarbeitung)", 10, 12, "0123456789 -"),
                new CharacterMapCheck("49005902", "Unfallversicherung Bund und Bahn – Bereich Bahn (ehemals Eisenbahn-Unfallkasse)", 4, 10, "0123456789"),
                new CharacterMapCheck("52717470", "Berufsgenossenschaft Rohstoffe und chemische Industrie Branche Lederindustrie (ehemals Lederindustrie-Berufsgenossenschaft)", 7, 9, "0123456789"),
                new CharacterMapCheck("52738475", "Berufsgenossenschaft Nahrungsmittel und Gastgewerbe (ehemals Fleischerei-Berufsgenossenschaft)", 7, 8, "0123456789.,"),
                new CharacterMapCheck("52742028", "Berufsgenossenschaft Holz und Metall (ehemals Berufsgenossenschaft Metall Nord Süd – Bereich Süd)", 9, 9, "0123456789"),
                new CharacterMapCheck("53149588", "Unfallkasse Rheinland-Pfalz", 10, 11, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"),
                new CharacterMapCheck("55423519", "Unfallkasse Saarland", 3, 3, "0123456789"),
                new CharacterMapCheck("61635458", "Berufsgenossenschaft Rohstoffe und chemische Industrie Branche chemische Industrie (ehemals Berufsgenossenschaft der chemischen Industrie)", 7, 9, "0123456789/"),
                new CharacterMapCheck("62279404", "Berufsgenossenschaft der Bauwirtschaft – Karlsruhe", null, null, null),
                new CharacterMapCheck("63800761", "Berufsgenossenschaft Nahrungsmittel und Gastgewerbe", 11, 20, "0123456789M .-"),
                new CharacterMapCheck("63886548", "Berufsgenossenschaft Handel und Warenlogistik", 8, 8, "0123456789"),
                new CharacterMapCheck("66337061", "Berufsgenossenschaft Verkehrswirtschaft Post-Logistik Telekommunikation Sparte Post und Telekom (ehemals Unfallkasse Post und Telekom)", 4, 4, "0123456789"),
                new CharacterMapCheck("67334480", "Unfallkasse Baden-Württemberg", 10, 17, "0123456789 "),
                new CharacterMapCheck("67350937", "Berufsgenossenschaft der Bauwirtschaft – Böblingen", null, null, null),
                new CharacterMapCheck("67545123", "SVLFG, LBG, Standort Stuttgart", null, null, null),
                new CharacterMapCheck("72305544", "SVLFG, LBG, Standort Bayreuth", null, null, null),
                new CharacterMapCheck("75932959", "Verwaltungs-Berufsgenossenschaft (ehemals Berufsgenossenschaft der keramischen und Glas-Industrie)", null, null, null),
                new CharacterMapCheck("87108525", "SVLFG, LBG, Standort Landshut", null, null, null),
                new CharacterMapCheck("87661138", "Berufsgenossenschaft der Bauwirtschaft – München Hochbau", null, null, null),
                new CharacterMapCheck("87661183", "Berufsgenossenschaft der Bauwirtschaft – München Tiefbau", null, null, null),
                new CharacterMapCheck("87661207", "Kommunale Unfallversicherung Bayern (ehemals Bayerischer Gemeindeunfallversicherungsverband)", 6, 6, "0123456789"),
                new CharacterMapCheck("87741942", "Berufsgenossenschaft Holz und Metall (ehemals Holz- Berufsgenossenschaft)", 9, 11, "0123456789 "),
                new CharacterMapCheck("88270171", "Bayerische Landesunfallkasse", 6, 6, "0123456789"),
                new CharacterMapCheck("90276713", "Unfallkasse Berlin", 9, 9, "0123456789"),
                new CharacterMapCheck("98705576", "Feuerwehr-Unfallkasse Mitte", null, null, null),
                new CharacterMapCheck("99011352", "Berufsgenossenschaft Verkehrswirtschaft Post-Logistik Telekommunikation Sparte Seeschifffahrt (ehemals See-Berufsgenossenschaft)", 8, 8, "0123456789"),
            };

        /// <summary>
        /// Holt eine Liste von Prüfungen für Unfallversicherungsträger
        /// </summary>
        public IEnumerable<IDguvNumberCheck> Checks => _checks;

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
