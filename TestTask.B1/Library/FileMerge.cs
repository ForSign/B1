using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Library
{
    internal class FileMerge
    {
        public void MergeFiles(string pathDir, string outFilePath)
        {
            string[] files = Directory.GetFiles(pathDir);
            Trace.WriteLine($"Total files queued for merge: {files.Length}");
            Trace.TraceWarning("Merge will overwrite file at it's destination if exists");

            //Directory.CreateDirectory(outFilePath);
            using (var fs = File.Create(outFilePath))
            {
                foreach (var file in files)
                {
                    using (var inputStream = File.OpenRead(file))
                    {
                        inputStream.CopyTo(fs);
                    }
                    Trace.WriteLine($"{file} merged to it's destination filepath.");
                }
            }
        }

        public void PurgeFiles(string[] filterArray, string pathDir)
        {
            string[] files = Directory.GetFiles(pathDir);
            int total = 0;
            Trace.WriteLine($"Total files queued for purged: {files.Length}");

            foreach (var file in files)
            {
                string temp = Path.GetTempFileName();
                int counter = 0;

                using (var sr = new StreamReader(file))
                using (var sw = new StreamWriter(temp))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!filterArray.Any(line.Contains))
                            sw.WriteLine(line);
                        else
                            counter++;
                    }

                    total += counter;
                }

                File.Delete(file);
                File.Move(temp, file);
                Trace.WriteLine($"{file} has been purged. Removed lines count: {counter}");
            }
            Trace.WriteLine($"Total lines Removed: {total}");
        }
    }
}
