// <copyright file="ICheckNumberValidator.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Dguv.Validator
{
    public interface ICheckNumberValidator
    {
        /// <summary>
        /// Funktion zum Berechnen einer Prüfziffer anhand der übegebenen Mitgliedsnummer
        /// </summary>
        /// <returns>Die errechnete Prüfziffer</returns>
        object Calculate(string membershipNumber);

        /// <summary>
        /// Funktion zum vergleichen der BG-Mitgliedsnummern
        /// </summary>
        /// <returns>Liefert <code>True</code> wenn die errechnete und aus der Mitgliedsnummer extrahirte Prüfziffer übereinstimmen</returns>
        bool Validate(string membershipNumber);
    }
}
