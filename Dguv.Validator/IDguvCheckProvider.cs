using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dguv.Validator
{
    /// <summary>
    /// Schnittstelle für Anbieter von Prüfungen für Unfallversicherungsträger.
    /// </summary>
    public interface IDguvCheckProvider
    {
        /// <summary>
        /// Lädt die Prüfungen von Unfallversicherungsträgern.
        /// </summary>
        /// <returns>Ein <see cref="Task"/>, in dem das Laden der Prüfungen
        /// von Unfallversicherungsträgern ausgeführt wird.</returns>
        Task<IEnumerable<IDguvNumberCheck>> LoadChecks();
    }
}
