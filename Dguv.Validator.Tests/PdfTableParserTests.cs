using System.Diagnostics.CodeAnalysis;

using Dguv.Validator.Checks;

using Xunit;

namespace Dguv.Validator.Tests
{
    [SuppressMessage("StyleCopPlus.StyleCopPlusRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
    public class PdfTableParserTests
    {
        [Fact]
        public void TestTableSimple()
        {
            const string pageContent = "Anlage 20 \nGültige Betriebsnummer des zuständigen Unfallversicherungsträgers (BBNR-UV) und das in Abhängigkeit der BBNR-UV \njeweils gültige Format der Mitgliedsnummer (MNR) \n \n \nBBNR-UV Name des Unfallversicherungsträgers minimale maximale  gültiger Zeichenvorrat  \nLänge der Länge der der MNR \nMNR MNR \n01064065 Unfallkasse Sachsen 6 6 0-9 \nStand: 05.06.2014 Anlage 20 Seite 1 von 4 Version 2.54 ";
            var parser = new PdfTableParser();
            parser.Parse(pageContent);
            var checks = parser.GetChecks();
            Assert.Equal(1, checks.Count);
            var item = checks[0];
            var check = Assert.IsType<CharacterMapCheck>(item);
            Assert.Equal("01064065", check.BbnrUv);
            Assert.Equal("Unfallkasse Sachsen", check.Name);
            Assert.Equal(6, check.MinLength);
            Assert.Equal(6, check.MaxLength);
            Assert.Equal(10, check.ValidCharacters.Count);
            Assert.True(check.ValidCharacters.Contains('0'));
            Assert.True(check.ValidCharacters.Contains('1'));
            Assert.True(check.ValidCharacters.Contains('2'));
            Assert.True(check.ValidCharacters.Contains('3'));
            Assert.True(check.ValidCharacters.Contains('4'));
            Assert.True(check.ValidCharacters.Contains('5'));
            Assert.True(check.ValidCharacters.Contains('6'));
            Assert.True(check.ValidCharacters.Contains('7'));
            Assert.True(check.ValidCharacters.Contains('8'));
            Assert.True(check.ValidCharacters.Contains('9'));
        }

        [Fact]
        public void TestNoCheck()
        {
            const string pageContent = "Anlage 20 \nGültige Betriebsnummer des zuständigen Unfallversicherungsträgers (BBNR-UV) und das in Abhängigkeit der BBNR-UV \njeweils gültige Format der Mitgliedsnummer (MNR) \n \n \nBBNR-UV Name des Unfallversicherungsträgers minimale maximale  gültiger Zeichenvorrat  \nLänge der Länge der der MNR \nMNR MNR \n01627953 Hanseatische Feuerwehr-Unfallkasse Nord keine Prüfung keine Prüfung keine Prüfung  \nStand: 05.06.2014 Anlage 20 Seite 1 von 4 Version 2.54 ";
            var parser = new PdfTableParser();
            parser.Parse(pageContent);
            var checks = parser.GetChecks();
            Assert.Equal(1, checks.Count);
            var item = checks[0];
            var check = Assert.IsType<CharacterMapCheck>(item);
            Assert.Equal("01627953", check.BbnrUv);
            Assert.Equal("Hanseatische Feuerwehr-Unfallkasse Nord", check.Name);
            Assert.Equal(-1, check.MinLength);
            Assert.Equal(-1, check.MaxLength);
            Assert.Equal(0, check.ValidCharacters.Count);
        }

        [Fact]
        public void TestMultiLine()
        {
            const string pageContent = "Anlage 20 \nGültige Betriebsnummer des zuständigen Unfallversicherungsträgers (BBNR-UV) und das in Abhängigkeit der BBNR-UV \njeweils gültige Format der Mitgliedsnummer (MNR) \n \n \nBBNR-UV Name des Unfallversicherungsträgers minimale maximale  gültiger Zeichenvorrat  \nLänge der Länge der der MNR \nMNR MNR \n18477668 Kommunale Unfallversicherung Bayern (ehemals Unfallkasse 5 9 0-9, \nMünchen) \nStand: 05.06.2014 Anlage 20 Seite 1 von 4 Version 2.54 ";
            var parser = new PdfTableParser();
            parser.Parse(pageContent);
            var checks = parser.GetChecks();
            Assert.Equal(1, checks.Count);
            var item = checks[0];
            var check = Assert.IsType<CharacterMapCheck>(item);
            Assert.Equal("18477668", check.BbnrUv);
            Assert.Equal("Kommunale Unfallversicherung Bayern (ehemals Unfallkasse München)", check.Name);
            Assert.Equal(5, check.MinLength);
            Assert.Equal(9, check.MaxLength);
        }

        [Fact]
        public void TestMultiPageMultiLine()
        {
            const string pageContent1 = "Anlage 20 \nBBNR-UV Name des Unfallversicherungsträgers minimale maximale  gültiger Zeichenvorrat  \nLänge der Länge der der MNR \nMNR MNR \n61635458 Berufsgenossenschaft Rohstoffe und chemische Industrie  7 9 0-9, / \nBranche chemische Industrie \nStand: 05.06.2014 Anlage 20 Seite 3 von 4 Version 2.54 ";
            const string pageContent2 = "Anlage 20 \nBBNR-UV Name des Unfallversicherungsträgers minimale maximale  gültiger Zeichenvorrat  \nLänge der Länge der der MNR \nMNR MNR \n(ehemals Berufsgenossenschaft der chemischen Industrie) \n62279404 Berufsgenossenschaft der Bauwirtschaft – Karlsruhe 10 17 0-9, M, /, -, Blank, Punkt \n99011352 Berufsgenossenschaft für Transport und Verkehrswirtschaft  8 8 0-9 \nBereich Seeschifffahrt  \n(ehemals See-Berufsgenossenschaft) \n \nStand: 05.06.2014 Anlage 20 Seite 4 von 4 Version 2.54 ";
            var parser = new PdfTableParser();
            parser.Parse(pageContent1);
            parser.Parse(pageContent2);
            var checks = parser.GetChecks();
            Assert.Equal(3, checks.Count);

            {
                var item = checks[0];
                var check = Assert.IsType<CharacterMapCheck>(item);
                Assert.Equal("61635458", check.BbnrUv);
                Assert.Equal("Berufsgenossenschaft Rohstoffe und chemische Industrie Branche chemische Industrie (ehemals Berufsgenossenschaft der chemischen Industrie)", check.Name);
                Assert.Equal(7, check.MinLength);
                Assert.Equal(9, check.MaxLength);
            }

            {
                var item = checks[1];
                var check = Assert.IsType<CharacterMapCheck>(item);
                Assert.Equal("62279404", check.BbnrUv);
                Assert.Equal("Berufsgenossenschaft der Bauwirtschaft – Karlsruhe", check.Name);
                Assert.Equal(10, check.MinLength);
                Assert.Equal(17, check.MaxLength);
                Assert.Equal(15, check.ValidCharacters.Count);
            }

            {
                var item = checks[2];
                var check = Assert.IsType<CharacterMapCheck>(item);
                Assert.Equal("99011352", check.BbnrUv);
                Assert.Equal("Berufsgenossenschaft für Transport und Verkehrswirtschaft Bereich Seeschifffahrt (ehemals See-Berufsgenossenschaft)", check.Name);
                Assert.Equal(8, check.MinLength);
                Assert.Equal(8, check.MaxLength);
            }
        }
    }
}
