using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dguv.Validator;
using System.IO;
using System.Linq;


namespace Dguv.Validator.Format.Providers
{
    public class DguvTextCheckProvider : IDguvCheckProvider
    {
        private Dictionary<string, ICheckNumberValidator[]> checkNumberValidators = new Dictionary<string, ICheckNumberValidator[]>()
        {
           { "15087927", new ICheckNumberValidator[] { new Checks.Check1(), new Checks.Check15() } },
           { "29036720", new ICheckNumberValidator[] { new Checks.Check1(), new Checks.Check15() } },
           { "44888436", new ICheckNumberValidator[] { new Checks.Check1(), new Checks.Check15() } },
           { "15141364", new ICheckNumberValidator[] { new Checks.Check2() } },
           { "15186676", new ICheckNumberValidator[] { new Checks.Check3() } },
           { "15250094", new ICheckNumberValidator[] { new Checks.Check4() } },
           { "29029801", new ICheckNumberValidator[] { new Checks.Check6() } },
           { "34364294", new ICheckNumberValidator[] { new Checks.Check8() } },
           { "37916971", new ICheckNumberValidator[] { new Checks.Check9() } },
           { "14066582", new ICheckNumberValidator[] { new Checks.Check10(), new Checks.Check15() } },
           { "42884688", new ICheckNumberValidator[] { new Checks.Check10(), new Checks.Check15() } },
           { "62279404", new ICheckNumberValidator[] { new Checks.Check12(), new Checks.Check15() } },
           { "63800761", new ICheckNumberValidator[] { new Checks.Check13() } },
           { "63886548", new ICheckNumberValidator[] { new Checks.Check14() } },
           { "67350937", new ICheckNumberValidator[] { new Checks.Check15() } },
           { "87661138", new ICheckNumberValidator[] { new Checks.Check15(), new Checks.Check16() } },
           { "87661183", new ICheckNumberValidator[] { new Checks.Check15(), new Checks.Check17() } }
        };
        

        public async Task<IEnumerable<IDguvNumberCheck>> LoadChecks()
        {
            List<IDguvNumberCheck> checks = new List<IDguvNumberCheck>();
            //Eventuel sollte man die Textdatei auf einen Server legen und diese von dort laden.
            using (FileStream fs = File.Open("Data/uv171001_v4.txt", FileMode.Open, FileAccess.Read))
            {
                using(BufferedStream bs = new BufferedStream(fs))
                {
                    using (var streamReader = new StreamReader(bs))
                    {
                        string line = string.Empty, bnr, name, format;

                        while (!streamReader.EndOfStream)
                        {
                            bnr = string.Empty;
                            name = string.Empty;
                            format = string.Empty;
                            line = streamReader.ReadLine();
                            if (line.Length >= 28)
                                bnr = line.Substring(27, Math.Min(15, line.Length - 27)).Trim();
                            if (line.Length >= 57)
                                name = line.Substring(56, Math.Min(35, line.Length - 56)).TrimStart().TrimEnd();
                            if (line.Length >= 343)
                                format = line.Substring(342, Math.Min(100, line.Length - 342)).TrimStart().TrimEnd();
                            string[] patterns = FormatParser.parseFormat(format);

                            if(bnr != string.Empty && name != string.Empty)
                            {
                                ICheckNumberValidator[] checkers = null;
                                checkNumberValidators.TryGetValue(bnr, out checkers);
                                checks.Add(new Dguv.Validator.Checks.CharacterMapCheckFormat(bnr, name, patterns, checkers));
                            }
                                
                        }
                    }
                }
            }
            return checks;
        }
    }
}
