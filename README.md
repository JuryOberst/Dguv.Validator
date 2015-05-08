# Dguv.Validator

[![Build-Status](https://img.shields.io/teamcity/https/build.service-dataline.de:8081/s/OpenSource_DguvValidator.svg?label=TeamCity)](https://build.service-dataline.de:8081/viewType.html?buildTypeId=OpenSource_DguvValidator&guest=1)

Eine Prüffunktion für die Mitgliedsnummern der deutsche Unfallversicherungsträger.

# Lizenz

Die Bibliothek steht unter der [MIT-Lizenz](LICENSE.md)
und wurde bereitgestellt von:

[![DATALINE](http://www.dataline.de/images/Logo_kleiner.png)](http://www.dataline.de)

# Grundlegender Aufbau

Die Bibliothek besteht aus den folgenden Bereichen:

* Prüffunktion für die Mitgliedsnummer eines einzelnen Unfallversicherungsträgers
* Laden von Prüffunktionen für die Mitgliedsnummern
* Validierung von Mitgliedsnummern von Unfallversicherungsträgern

# Beispiel

Hier ein kurzes Beispiel zur Prüfung von Mitgliedsnummern, wobei die Nummern **nicht** aus dem Internet geladen werden:

```csharp
// Betriebsnummer eines Unfallversicherungsträgers
var bbnrUv = "15250094";
// Mitgliedsnummer eines Unfallversicherungsträgers
var memberId = "1234567890";

var validator = new DguvValidator();
var status = validator.GetStatus(bbnrUv, memberId);
if (status != null) {
    // Fehler! Meldung steht in status.
} else {
    // Alles OK
}
```

# Besonderheit

Über die Klasse ```WebCheckProvider``` können die Prüfungen aus der aktuellen Tabelle
von [dieser Web-Seite](http://www.dguv.de/de/mediencenter/hintergrund/meldeverfahren/mitgliedsnr/index.jsp) erstellt werden.
