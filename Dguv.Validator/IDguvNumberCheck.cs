// <copyright file="IDguvNumberCheck.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Dguv.Validator
{
    /// <summary>
    /// Schnittstelle für ein Prüfverfahren für einen Unfallversicherungsträger
    /// </summary>
    public interface IDguvNumberCheck
    {
        /// <summary>
        /// Holt die Betriebsnummer des Unfallversicherungsträgers
        /// </summary>
        string BbnrUv { get; }

        /// <summary>
        /// Holt den Namen des Unfallversicherungsträgers
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Prüft, ob die Mitgliedsnummer für den Unfallversicherungsträger gültig ist und liefert im
        /// Falle eines Fehlers die Fehlermeldung zurück.
        /// </summary>
        /// <param name="memberId">Die zu prüfenden Mitgliedsnummer</param>
        /// <returns>Status der Validierung</returns>
        IStatus Validate(string memberId);
    }
}
