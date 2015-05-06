using System;
using System.Collections.Generic;
using System.Linq;

using Dguv.Validator.Properties;

namespace Dguv.Validator.Checks
{
    /// <summary>
    /// Standard-Überprüfungsverfahren für Unfallversicherungsträger
    /// </summary>
    public class CharacterMapCheck : IDguvNumberCheck
    {
        private readonly string _validCharacters;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="CharacterMapCheck"/> Klasse.
        /// </summary>
        /// <param name="bbnrUv">Die Betriebsnummer des Unfallversicherungsträgers</param>
        /// <param name="name">Der Name des Unfallversicherungsträgers</param>
        public CharacterMapCheck(string bbnrUv, string name)
            : this(bbnrUv, name, -1, -1, null)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="CharacterMapCheck"/> Klasse.
        /// </summary>
        /// <param name="bbnrUv">Die Betriebsnummer des Unfallversicherungsträgers</param>
        /// <param name="name">Der Name des Unfallversicherungsträgers</param>
        /// <param name="minLength">Die minimale Länge der Mitgliedsnummer (oder -1, wenn nicht geprüft werden soll)</param>
        /// <param name="maxLength">Die maximale Länge der Mitgliedsnummer (oder -1, wenn nicht geprüft werden soll)</param>
        /// <param name="validCharacters">Die Zeichen, die eine Mitgliedsnummer haben darf (oder null/Empty, wenn nicht geprüft werden soll)</param>
        public CharacterMapCheck(string bbnrUv, string name, int minLength, int maxLength, string validCharacters)
        {
            if (minLength < -1)
                throw new ArgumentOutOfRangeException("minLength", Resources.CheckMinLengthError);
            if (maxLength < -1 || maxLength < minLength || maxLength == 0)
                throw new ArgumentOutOfRangeException("maxLength", Resources.CheckMaxLengthError);
            if (string.IsNullOrWhiteSpace(bbnrUv))
                throw new ArgumentOutOfRangeException("bbnrUv", Resources.CheckBbnrUvError);
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException("name", Resources.CheckBbnrUvNameError);

            BbnrUv = bbnrUv;
            Name = name;
            MinLength = minLength;
            MaxLength = maxLength;
            _validCharacters = validCharacters;
            ValidCharacters = new HashSet<char>((validCharacters ?? string.Empty).ToCharArray());
        }

        /// <summary>
        /// Holt die Betriebsnummer des Unfallversicherungsträgers
        /// </summary>
        public string BbnrUv { get; private set; }

        /// <summary>
        /// Holt den Namen des Unfallversicherungsträgers
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Holt die minimale Länge der Mitgliedsnummer
        /// </summary>
        public int MinLength { get; private set; }

        /// <summary>
        /// Holt die maximale Länge der Mitgliedsnummer
        /// </summary>
        public int MaxLength { get; private set; }

        /// <summary>
        /// Holt ein Set mit allen gültigen Zeichen einer Mitgliedsnummer
        /// </summary>
        public ISet<char> ValidCharacters { get; private set; }

        /// <summary>
        /// Holt einen Wert, der angibt, ob eine Überprüfung erforderlich ist
        /// </summary>
        public bool IsValidationRequired
        {
            get { return MinLength != -1 || MaxLength != -1 || (ValidCharacters != null && ValidCharacters.Count != 0); }
        }

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
            if (MinLength != -1 && memberId.Length < MinLength)
                return string.Format(Resources.StatusMemberIdTooShort, MinLength);
            if (MaxLength != -1 && memberId.Length > MaxLength)
                return string.Format(Resources.StatusMemberIdTooLong, MaxLength);
            if (ValidCharacters == null || ValidCharacters.Count == 0)
                return null;
            if (!memberId.ToCharArray().All(x => ValidCharacters.Contains(x)))
                return string.Format(Resources.StatusMemberIdInvalidCharacter, _validCharacters);
            return null;
        }
    }
}
