// <copyright file="DguvValidationException.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// </copyright>

using System;

namespace Dguv.Validator
{
    /// <summary>
    /// Eine Exception, die im Falle eines Validierungsfehlers ausgeworfen wird.
    /// </summary>
    public class DguvValidationException : Exception
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="DguvValidationException"/> Klasse.
        /// </summary>
        public DguvValidationException()
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="DguvValidationException"/> Klasse.
        /// </summary>
        /// <param name="message">Die Fehlermeldung</param>
        public DguvValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="DguvValidationException"/> Klasse.
        /// </summary>
        /// <param name="message">Die Fehlermeldung</param>
        /// <param name="inner">Die Exception, die zum Auslösen dieser Exception geführt hat.</param>
        public DguvValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
