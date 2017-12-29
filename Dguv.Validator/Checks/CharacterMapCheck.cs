﻿// <copyright file="CharacterMapCheck.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

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
            : this(bbnrUv, name, null, null, null)
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
        public CharacterMapCheck(string bbnrUv, string name, int? minLength, int? maxLength, string validCharacters)
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
        /// Holt einen Wert, der angibt, ob eine Überprüfung erforderlich ist
        /// </summary>
        public bool IsValidationRequired => MinLength != null || MaxLength != null || (ValidCharacters != null && ValidCharacters.Count != 0);

        /// <inheritdoc />
        public IStatus Validate(string memberId)
        {
            if (!IsValidationRequired)
                return new CharacterMapStatus(true, Resources.StatusOK);
            if (string.IsNullOrEmpty(memberId))
                return new CharacterMapStatus(false, Resources.StatusMemberIdMissing);
            if (MinLength != null && memberId.Length < MinLength)
                return new CharacterMapStatus(false, string.Format(Resources.StatusMemberIdTooShort, MinLength));
            if (MaxLength != null && memberId.Length > MaxLength)
                return new CharacterMapStatus(false, string.Format(Resources.StatusMemberIdTooLong, MaxLength));
            if (ValidCharacters == null || ValidCharacters.Count == 0)
                return new CharacterMapStatus(true, Resources.StatusOK);
            if (!memberId.ToCharArray().All(x => ValidCharacters.Contains(x)))
                return new CharacterMapStatus(false, string.Format(Resources.StatusMemberIdInvalidCharacter, _validCharacters));
            return new CharacterMapStatus(true, Resources.StatusOK);
        }

        private class CharacterMapStatus : IStatus
        {
            private readonly string _statusMessage;

            public CharacterMapStatus(bool isSuccessful, string statusMessage)
            {
                _statusMessage = statusMessage;
                IsSuccessful = isSuccessful;
            }

            public bool IsSuccessful { get; }

            public string GetStatusText() => _statusMessage;
        }
    }
}
