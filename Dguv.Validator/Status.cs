using System;
using Dguv.Validator.Properties;

namespace Dguv.Validator
{
    /// <summary>
    /// Der Status der nach dem Prüfen der Mitgliedsnummer zurückgegeben wird
    /// </summary>
    internal class Status : IStatus
    {
        private readonly int _statuscode;

        private readonly int? _maxLength;

        private readonly int? _minLength;

        private readonly string _validChars;

        /// <summary>
        /// Konstruktor zum Initialisieren des Status
        /// </summary>
        /// <param name="statuscode">Der Statuscode der angezegt werden soll</param>
        public Status(int statuscode)
            : this(statuscode, null, null, null) => _statuscode = statuscode;

        /// <summary>
        /// Erweiterter Konstruktor zum Initialisieren des Status
        /// </summary>
        /// <param name="statuscode">Der Statuscode der angezeigt werden soll</param>
        /// <param name="maxLength">Die maximal zugelassene Länge der Mitgliedsnummer</param>
        /// <param name="minLength">Die minimal zugelassene Länge der Mitgliedsnummer</param>
        /// <param name="validChars">Die zugelassenen Zeichen innehalb der Mitgliedsnummer</param>
        public Status(int statuscode, int? maxLength, int? minLength, string validChars)
        {
            _statuscode = statuscode;
            _maxLength = maxLength;
            _minLength = minLength;
            _validChars = validChars;
        }

        /// <summary>
        /// Holt den Statuscode
        /// </summary>
        public int StatusCode => _statuscode;

        /// <summary>
        /// Ruft den Meldungstext zu einem Statuscode ab
        /// </summary>
        /// <returns>Der Meldungstext</returns>
        public string GetStatusText()
        {
            switch (_statuscode)
            {
                case 0:
                    return Resources.StatusOK;
                case 1:
                    return string.Format(Resources.StatusMemberIdInvalidCharacter, _validChars);
                case 2:
                    return string.Format(Resources.StatusMemberIdTooShort, _minLength);
                case 3:
                    return string.Format(Resources.StatusMemberIdTooLong, _maxLength);
                case 4:
                    return Resources.StatusMemberIdInvalidStructure;
                case 5:
                    return Resources.StatusMemberIdInvalidChecknumber;
                case 6:
                    return Resources.StatusMemberIdMissing;
                case 7:
                    return Resources.StatusInvalidBbnrUv;
                default:
                    throw new NotSupportedException($"Stauscode {_statuscode} wird nicht unterstützt");
            }
        }
    }
}
