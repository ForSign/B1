using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestTask.B1.Library
{
    internal static class FileWorker
    {
        internal static string[]? OpenFile(bool Multiselect, string Filter = "Txt files (.txt)|*.txt")
        {
            var dlg = new OpenFileDialog();

            dlg.DefaultExt = ".txt";
            dlg.Filter = Filter;
            dlg.Multiselect = Multiselect;

            Nullable<bool> dialogResult = dlg.ShowDialog();

            if (dialogResult != true)
            {
                MessageBox.Show("You need to select file to open");
                return null;
            }

            if (Multiselect)
                return dlg.FileNames;
            return new string[] { dlg.FileName };
        }

        internal static string? SaveFile()
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.DefaultExt = ".txt";
            dlg.Filter = "Txt files (.txt)|*.txt";

            Nullable<bool> dialogResult = dlg.ShowDialog();

            if (dialogResult != true)
            {
                MessageBox.Show("You need to select save file location");
                return null;
            }

            return dlg.FileName;
        }

        internal static void MergeFiles(string[] files, string outFilePath)
        {
            Trace.WriteLine($"Total files queued for merge: {files.Length}");
            Trace.TraceWarning("Merge will overwrite file at it's destination if exists");

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

        internal static void PurgeFiles(string[] filterArray, string[] files)
        {
            int total = 0;
            Trace.WriteLine($"Total files queued for purged: {files.Length}");

            foreach (var file in files)
            {
                string temp = Path.GetTempFileName();
                int counter = 0;

                using (var sr = new StreamReader(file))
                using (var sw = new StreamWriter(temp))
                {
                    string? line;

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
