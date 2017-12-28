// <copyright file="CharacterMapCheckFormat.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dguv.Validator.Properties;

namespace Dguv.Validator.Checks
{
    /// <summary>
    /// Standard-Überprüfungsverfahren für Unfallversicherungsträger
    /// </summary>
    public class CharacterMapCheckFormat : IDguvNumberCheck
    {
        private readonly string _validCharacters;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="CharacterMapCheck"/> Klasse.
        /// </summary>
        /// <param name="bbnrUv">Die Betriebsnummer des Unfallversicherungsträgers</param>
        /// <param name="name">Der Name des Unfallversicherungsträgers</param>
        public CharacterMapCheckFormat(string bbnrUv, string name)
            : this(bbnrUv, name, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="CharacterMapCheck"/> Klasse.
        /// </summary>
        /// <param name="bbnrUv">Die Betriebsnummer des Unfallversicherungsträgers</param>
        /// <param name="name">Der Name des Unfallversicherungsträgers</param>
        /// <param name="minLength">Die minimale Länge der Mitgliedsnummer (oder <code>null</code>, wenn nicht geprüft werden soll)</param>
        /// <param name="maxLength">Die maximale Länge der Mitgliedsnummer (oder <code>null</code>, wenn nicht geprüft werden soll)</param>
        /// <param name="validCharacters">Die Zeichen, die eine Mitgliedsnummer haben darf (oder null/Empty, wenn nicht geprüft werden soll)</param>
        /// <param name="patterns">Die RegEx-Muster den die zu prüfende Mitgliedsnummer entsprechen sollte (mindestens einem der Muster)</param>
        /// <param name="validators">Komponente zum Erstellen und Prüfen der Prüfziffer</param>
        public CharacterMapCheckFormat(string bbnrUv, string name, int? minLength, int? maxLength, string validCharacters, string[] patterns, ICheckNumberValidator[] validators)
        {
            if (minLength != null && minLength < 1)
                throw new ArgumentOutOfRangeException(nameof(minLength), Resources.CheckMinLengthError);
            if (maxLength != null && (maxLength < 1 || (minLength != null && maxLength < minLength)))
                throw new ArgumentOutOfRangeException(nameof(maxLength), Resources.CheckMaxLengthError);
            if (string.IsNullOrWhiteSpace(bbnrUv))
                throw new ArgumentOutOfRangeException(nameof(bbnrUv), Resources.CheckBbnrUvError);
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name), Resources.CheckBbnrUvNameError);

            BbnrUv = bbnrUv;
            Name = name;
            MinLength = minLength;
            MaxLength = maxLength;
            _validCharacters = validCharacters;
            ValidCharacters = new HashSet<char>((validCharacters ?? string.Empty).ToCharArray());
            Patterns = patterns;
            CheckNumberValidators = validators;
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
        /// Holt die minimale Länge der Mitgliedsnummer
        /// </summary>
        public int? MinLength { get; }

        /// <summary>
        /// Holt die maximale Länge der Mitgliedsnummer
        /// </summary>
        public int? MaxLength { get; }

        /// <summary>
        /// Holt ein Set mit allen gültigen Zeichen einer Mitgliedsnummer
        /// </summary>
        public ISet<char> ValidCharacters { get; }

        /// <summary>
        /// Holt die Prüffunktion für die Prüfziffer
        /// </summary>
        public ICheckNumberValidator[] CheckNumberValidators { get; }

        /// <summary>
        /// Holt die Pattern für eine Regular Expression
        /// </summary>
        public string[] Patterns { get; }

        /// <summary>
        /// Holt einen Wert, der angibt, ob eine Überprüfung erforderlich ist
        /// </summary>
        public bool IsValidationRequired => MinLength != null || MaxLength != null || (ValidCharacters != null && ValidCharacters.Count != 0) || Patterns.Length > 0 || CheckNumberValidators != null;

        /// <summary>
        /// Prüft, ob die Mitgliedsnummer für den Unfallversicherungsträger gültig ist.
        /// </summary>
        /// <param name="memberId">Die zu prüfenden Mitgliedsnummer</param>
        /// <returns>true, wenn die Mitgliedsnummer gültig ist</returns>
        public bool IsValid(string memberId)
        {
            return GetStatus(memberId).StatusCode == 0;
        }

        /// <summary>
        /// Prüft, ob die Mitgliedsnummer für den Unfallversicherungsträger gültig ist und liefert im
        /// Falle eines Fehlers die Fehlermeldung zurück.
        /// </summary>
        /// <param name="memberId">Die zu prüfenden Mitgliedsnummer</param>
        /// <returns>null, wenn die Mitgliedsnummer gültig ist, ansonsten die Fehlermeldung</returns>
        public IStatus GetStatus(string memberId)
        {
            if (!IsValidationRequired)
                return new Status(0);
            if (string.IsNullOrEmpty(memberId))
                return new Status(6);
            if (MinLength != null && memberId.Length < MinLength)
                return new Status(2, null, MinLength, null);
            if (MaxLength != null && memberId.Length > MaxLength)
                return new Status(3, MaxLength, null, null);
            if (ValidCharacters == null || ValidCharacters.Count == 0)
                return new Status(0);
            if (!memberId.ToCharArray().All(x => ValidCharacters.Contains(x)))
                return new Status(1, null, null,  _validCharacters);
            if (Patterns != null && !CheckWithPatterns(memberId))
                return new Status(4);
            if (CheckNumberValidators != null && !CheckCheckNumber(memberId))
                return new Status(5);
            return new Status(0);
        }

        /// <summary>
        /// Prüft, ob die Mitgliedsnummer den korrekten Aufbau hat
        /// </summary>
        /// <param name="memberId">Die zu prüfenden Mitgliedsnummer</param>
        /// <returns><code>TRUE</code>, wenn mindestens eines der Muster passt</returns>
        internal bool CheckWithPatterns(string memberId)
        {
            // Wenn keine Formate vorhanden sind, dann können/müssen die Eingaben nicht geprüft werden
            if (Patterns.Length == 0)
                return true;

            // Gehe durch die Liste der Muster und prüfe ob die Eingaben auf eines der Muster zutreffen
            foreach (var pat in Patterns)
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
        private bool CheckCheckNumber(string memberId)
        {
            // Wenn keine Formate vorhanden sind, dann können/müssen die Eingaben nicht geprüft werden
            if (CheckNumberValidators.Length == 0)
                return true;

            // Gehe durch die Liste der Muster und prüfe ob die Eingaben auf eines der Muster zutreffen
            foreach (var check in CheckNumberValidators)
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
