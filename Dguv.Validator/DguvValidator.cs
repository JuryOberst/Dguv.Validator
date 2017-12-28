// <copyright file="DguvValidator.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

using Dguv.Validator.Properties;
using Dguv.Validator.Providers;

namespace Dguv.Validator
{
    /// <summary>
    /// Standard-Implementation von <see cref="IDguvValidator"/>, die anhand von übergebenen
    /// Prüfungen für Unfallversicherungsträger die Mitgliedsnummern überprüft.
    /// </summary>
    public class DguvValidator : IDguvValidator
    {
        private readonly IDictionary<string, IDguvNumberCheck> _checks;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="DguvValidator"/> Klasse.
        /// </summary>
        /// <remarks>
        /// Es werden die Prüfungen aus <see cref="StaticCheckProvider"/> verwendet.
        /// </remarks>
        public DguvValidator()
            : this(new StaticCheckProvider().Checks)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="DguvValidator"/> Klasse.
        /// </summary>
        /// <param name="checks">Die Prüfungen für die Unfallversicherungsträger.</param>
        public DguvValidator(IEnumerable<IDguvNumberCheck> checks)
        {
            _checks = checks.ToDictionary(x => x.BbnrUv);
        }

        /// <summary>
        /// Überprüft, ob die Kombination aus UV-Betriebsnummer und Mitgliedsnummer gültig ist und
        /// gibt im Falle eines Fehlers eine Fehlermeldung zurück.
        /// </summary>
        /// <param name="bbnrUv">Die Betriebsnummer eines Unfallversicherungsträgers</param>
        /// <param name="memberId">Die Mitgliedsnummer eines Unfallversicherungsträgers</param>
        /// <returns>Die Fehlermeldung oder null, wenn kein Fehler aufgetreten ist.</returns>
        public IStatus Validate(string bbnrUv, string memberId)
        {
            if (_checks.TryGetValue(bbnrUv, out var check))
                return check.Validate(memberId);
            return new DguvValidatorStatus();
        }

        private class DguvValidatorStatus : IStatus
        {
            public bool IsSuccessful => false;

            public string GetStatusText() => Resources.StatusInvalidBbnrUv;
        }
    }
}
