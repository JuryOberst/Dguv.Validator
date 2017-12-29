// <copyright file="IDguvValidator.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Dguv.Validator
{
    /// <summary>
    /// Schnittstelle für eine Prüfung der Mitgliedsnummern eines Unfallversicherungsträgers
    /// </summary>
    public interface IDguvValidator
    {
        /// <summary>
        /// Überprüft, ob die Kombination aus UV-Betriebsnummer und Mitgliedsnummer gültig ist und
        /// gibt im Falle eines Fehlers eine Fehlermeldung zurück.
        /// </summary>
        /// <param name="bbnrUv">Die Betriebsnummer eines Unfallversicherungsträgers</param>
        /// <param name="memberId">Die Mitgliedsnummer eines Unfallversicherungsträgers</param>
        /// <returns>Die Fehlermeldung oder null, wenn kein Fehler aufgetreten ist.</returns>
        IStatus Validate(string bbnrUv, string memberId);
    }
}
