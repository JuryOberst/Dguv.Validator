# Dguv.Validator

[![Build-Status](https://img.shields.io/teamcity/https/build.service-dataline.de:8081/s/OpenSource_DguvValidator.svg?label=TeamCity)](https://build.service-dataline.de:8081/viewType.html?buildTypeId=OpenSource_DguvValidator&guest=1)

Eine Prüffunktion für die Mitgliedsnummern der deutsche Unfallversicherungsträger.

# Lizenz

Die Bibliothek steht unter der [MIT-Lizenz](LICENSE.md)
und wurde bereitgestellt von:

[![DATALINE](http://www.dataline.de/images/Logo_kleiner.png)](http://www.dataline.de)

# Grundlegender Aufbau

## Kern-Bibliothek

Die Bibliothek besteht aus den folgenden Bereichen:

* Prüffunktion für die Mitgliedsnummer eines einzelnen Unfallversicherungsträgers
* Laden von Prüffunktionen für die Mitgliedsnummern
* Validierung von Mitgliedsnummern von Unfallversicherungsträgern

## Web-Bibliothek

Diese Bibliothek enthält Funktionen zur Erstellung von Prüffunktionen
basierend auf Informationen aus dem Internet:

* [Anlage 20 der gemeinsamen Rundschreiben](http://www.gkv-datenaustausch.de/arbeitgeber/deuev/gemeinsame_rundschreiben/gemeinsame_rundschreiben.jsp)
* [Mitgliedsnummern von der DGUV](http://www.dguv.de/de/mediencenter/hintergrund/meldeverfahren/mitgliedsnr/index.jsp) (veraltet, die Liste der Mitgliedsnummern wurde von der Web-Seite entfernt)

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

Wenn die Klasse `GkvAnlage20CheckProvider` verwendet wird, dann werden die Prüfungen anhand
einer PDF-Datei der Anlage 20 der [gemeinsamen Rundschreiben](http://www.gkv-datenaustausch.de/arbeitgeber/deuev/gemeinsame_rundschreiben/gemeinsame_rundschreiben.jsp) erstellt.
