using System;
using System.Collections.Generic;
using System.Linq;

using Dguv.Validator.Properties;
using System.Text.RegularExpressions;

namespace Dguv.Validator.Checks
{
    /// <summary>
    /// Standard-Überprüfungsverfahren für Unfallversicherungsträger
    /// </summary>
    public class CharacterMapCheckFormat : IDguvNumberCheck
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="CharacterMapCheck"/> Klasse.
        /// </summary>
        /// <param name="bbnrUv">Die Betriebsnummer des Unfallversicherungsträgers</param>
        /// <param name="name">Der Name des Unfallversicherungsträgers</param>
        public CharacterMapCheckFormat(string bbnrUv, string name)
            : this(bbnrUv, name, null, null)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="CharacterMapCheck"/> Klasse.
        /// </summary>
        /// <param name="bbnrUv">Die Betriebsnummer des Unfallversicherungsträgers</param>
        /// <param name="name">Der Name des Unfallversicherungsträgers</param>
        /// <param name="patterns">Die RegEx-Muster den die zu prüfende Mitgliedsnummer entsprechen sollte (mindestens einem der Muster)</param>
        /// <param name="validator">Komponente zum Erstellen und Prüfen der Prüfziffer</param>
        public CharacterMapCheckFormat(string bbnrUv, string name, string[] patterns, ICheckNumberValidator[] validator)
        {
            if (string.IsNullOrWhiteSpace(bbnrUv))
                throw new ArgumentOutOfRangeException(nameof(bbnrUv), Resources.CheckBbnrUvError);
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name), Resources.CheckBbnrUvNameError);
            //if (patterns != null && patterns.Length == 0)
            //    throw new ArgumentOutOfRangeException(nameof(patterns) /* TODO: Resources mit der entsprechnenden Fehlermeldung erweitern, sobald Mark die MultilungualResources angepasst hat */);

            BbnrUv = bbnrUv;
            Name = name;
            Patterns = patterns;
            CheckNumberValidator = validator;
        }

        /// <summary>
        /// Holt die Betriebsnummer des Unfallversicherungsträgers
        /// </summary>
        public string BbnrUv { get; }

        /// <summary>
        /// Holt den Namen des Unfallversicherungsträgers
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Holt die Prüffunktion für die Prüfziffer
        /// </summary>
        public ICheckNumberValidator[] CheckNumberValidator { get; }

        /// <summary>
        /// Holt die Pattern für eine Regular Expression
        /// </summary>
        public string[] Patterns { get; }

        /// <summary>
        /// Holt einen Wert, der angibt, ob eine Überprüfung erforderlich ist
        /// </summary>
        public bool IsValidationRequired => Patterns.Length > 0 || CheckNumberValidator != null;

        /// <summary>
        /// Prüft, ob die Mitgliedsnummer für den Unfallversicherungsträger gültig ist.
        /// </summary>
        /// <param name="memberId">Die zu prüfenden Mitgliedsnummer</param>
        /// <returns>true, wenn die Mitgliedsnummer gültig ist</returns>
        public bool IsValid(string memberId)
        {
            return GetStatus(memberId) == null;
        }

        /// <summary>
        /// Prüft, ob die Mitgliedsnummer für den Unfallversicherungsträger gültig ist und liefert im
        /// Falle eines Fehlers die Fehlermeldung zurück.
        /// </summary>
        /// <param name="memberId">Die zu prüfenden Mitgliedsnummer</param>
        /// <returns>null, wenn die Mitgliedsnummer gültig ist, ansonsten die Fehlermeldung</returns>
        public string GetStatus(string memberId)
        {
            if (!IsValidationRequired)
                return null;
            if (string.IsNullOrEmpty(memberId))
                return Resources.StatusMemberIdMissing;
            if (Patterns != null && !checkWithPatterns(memberId))
                return "Fehler im  Aufbau der Mitgliesnummer" /* TODO: Resources.StatusMemberIdInvalidStructure */;
            if (CheckNumberValidator != null && !checkCheckNumber(memberId))
                return "Die Prüfziffer scheint nicht korrekt zu sein." /* TODO: Resources.StatusMemberIdInvalidChecknumber */ ;
            return null;
        }

        /// <summary>
        /// Prüft, ob die Mitgliedsnummer den korrekten Aufbau hat
        /// </summary>
        /// <param name="memberId">Die zu prüfenden Mitgliedsnummer</param>
        /// <returns><code>TRUE</code>, wenn mindestens eines der Muster passt</returns>
        private bool checkWithPatterns(string memberId)
        {
            // Wenn keine Formate vorhanden sind, dann können/müssen die Eingaben nicht geprüft werden
            if (Patterns.Length == 0)
                return true;
            // Gehe durch die Liste der Muster und prüfe ob die Eingaben auf eines der Muster zutreffen
            foreach(var pat in Patterns)
            {
                if (Regex.IsMatch(memberId, pat))
                    // Wenn die Eingaben auf mindestens eines der Muster treffen, dann kann die Prüfung 
                    // erfolgreich abgeschlossen werden, weitere Muster müssen nicht mehr geprüft werden
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Prüft, ob die Mitgliedsnummer den korrekten Aufbau hat
        /// </summary>
        /// <param name="memberId">Die zu prüfenden Mitgliedsnummer</param>
        /// <returns><code>TRUE</code>, wenn mindestens eines der Muster passt</returns>
        private bool checkCheckNumber(string memberId)
        {
            // Wenn keine Formate vorhanden sind, dann können/müssen die Eingaben nicht geprüft werden
            if (CheckNumberValidator.Length == 0)
                return true;
            // Gehe durch die Liste der Muster und prüfe ob die Eingaben auf eines der Muster zutreffen
            foreach (var check in CheckNumberValidator)
            {
                if (check.Validate(memberId))
                    // Wenn die Eingaben auf mindestens eines der Muster treffen, dann kann die Prüfung 
                    // erfolgreich abgeschlossen werden, weitere Muster müssen nicht mehr geprüft werden
                    return true;
            }
            return false;
        }
    }
}
