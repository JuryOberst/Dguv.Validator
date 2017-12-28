namespace Dguv.Validator
{
    /// <summary>
    /// Interface für die Rückgabe der Mitgliedsnummern-Prüfung
    /// </summary>
    public interface IStatus
    {
        /// <summary>
        /// Holt den Statuscode
        /// </summary>
        int StatusCode { get; }

        /// <summary>
        /// Ruft den Meldungstext zu einem Statuscode ab
        /// </summary>
        /// <returns>Der Meldungstext</returns>
        string GetStatusText();
    }
}