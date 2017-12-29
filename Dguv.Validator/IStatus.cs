// <copyright file="IStatus.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

namespace Dguv.Validator
{
    /// <summary>
    /// Interface für die Rückgabe der Mitgliedsnummern-Prüfung
    /// </summary>
    public interface IStatus
    {
        /// <summary>
        /// Holt einen Wert, der angibt, ob die Mitgliedsnummer gültig ist.
        /// </summary>
        bool IsSuccessful { get; }

        /// <summary>
        /// Ruft den Meldungstext zu einem Statuscode ab
        /// </summary>
        /// <returns>Der Meldungstext</returns>
        string GetStatusText();
    }
}
