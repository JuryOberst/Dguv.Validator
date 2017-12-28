// <copyright file="IDguvChecksumHandler.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

namespace Dguv.Validator.Format
{
    public interface IDguvChecksumHandler
    {
        /// <summary>
        /// Holt die ID des Prüfverfahrens
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Funktion zum Berechnen einer Prüfziffer anhand der übegebenen Mitgliedsnummer
        /// </summary>
        /// <returns>Die errechnete Prüfziffer</returns>
        string[] Calculate(string membershipNumber);
    }
}
