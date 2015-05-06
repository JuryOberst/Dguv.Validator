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
        /// wirft eine Exception aus, wenn ein Fehler auftritt.
        /// </summary>
        /// <param name="bbnrUv">Die Betriebsnummer eines Unfallversicherungsträgers</param>
        /// <param name="memberId">Die Mitgliedsnummer eines Unfallversicherungsträgers</param>
        void Validate(string bbnrUv, string memberId);

        /// <summary>
        /// Überprüft, ob die Kombination aus UV-Betriebsnummer und Mitgliedsnummer gültig ist und
        /// gibt im Falle eines Fehlers eine Fehlermeldung zurück.
        /// </summary>
        /// <param name="bbnrUv">Die Betriebsnummer eines Unfallversicherungsträgers</param>
        /// <param name="memberId">Die Mitgliedsnummer eines Unfallversicherungsträgers</param>
        /// <returns>Die Fehlermeldung oder null, wenn kein Fehler aufgetreten ist.</returns>
        string GetStatus(string bbnrUv, string memberId);

        /// <summary>
        /// Überprüft, ob die Kombination aus UV-Betriebsnummer und Mitgliedsnummer gültig ist und
        /// gibt im Falle eines Fehlers den Wert false zurück.
        /// </summary>
        /// <param name="bbnrUv">Die Betriebsnummer eines Unfallversicherungsträgers</param>
        /// <param name="memberId">Die Mitgliedsnummer eines Unfallversicherungsträgers</param>
        /// <returns>true, wenn die Kombination aus <paramref name="bbnrUv"/> und <paramref name="memberId"/> gültig ist.</returns>
        bool IsValid(string bbnrUv, string memberId);
    }
}
