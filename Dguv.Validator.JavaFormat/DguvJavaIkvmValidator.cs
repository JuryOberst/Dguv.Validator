using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dguv.Validator.JavaFormat
{
    public class DguvJavaIkvmValidator : IDguvValidator
    {
        private readonly string _testAppPath;

        public DguvJavaIkvmValidator()
        {
            var asm = typeof(DguvJavaIkvmValidator).GetTypeInfo().Assembly;
            var asmDir = Path.GetDirectoryName(new Uri(asm.CodeBase).LocalPath);
            Debug.Assert(asmDir != null, nameof(asmDir) + " != null");
            _testAppPath = Path.Combine(asmDir, "lib", "pl_mgnr.exe");
        }

        public IStatus Validate(string bbnrUv, string memberId)
        {
            var pi = new ProcessStartInfo(_testAppPath, $"-p 2 -u \"{bbnrUv}\" -m \"{memberId}\"")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                StandardOutputEncoding = Encoding.Default,
            };

            string output;
            int exitCode;
            using (var process = Process.Start(pi))
            {
                Debug.Assert(process != null, nameof(process) + " != null");
                process.WaitForExit();

                output = process.StandardOutput.ReadToEnd();
                exitCode = process.ExitCode;
            }

            var lines = new List<string>();
            using (var outputReader = new StringReader(output))
            {
                string line;
                while ((line = outputReader.ReadLine()) != null)
                    lines.Add(line);
            }

            if (lines.Count != 2)
                return new UvJavaCheckStatus(exitCode, output);

            var values = lines.Last().Split(new[] { ';' }, 6);
            var errorMessage = values[5];

            return new UvJavaCheckStatus(exitCode, errorMessage);
        }
    }
}
