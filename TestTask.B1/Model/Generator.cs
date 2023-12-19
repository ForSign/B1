using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestTask.B1.Model
{
    internal class Generator
    {
        private DateTime start = new DateTime(2018, 1, 1);
        private Random gen = new Random();
        private int range;

        const string char_ENG = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const string char_RUS = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЬЫЭЮЯабвгдеёжзийклмнопрстуфхцчшщъьыэюя";

        const int maxInt = 100_000_000;
        const int maxDouble = 20;

        /// <summary>
        /// Return random date withing range limit (5 years according to task)
        /// </summary>
        /// <returns></returns>
        public DateTime RandomDay()
        {
            range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        /// <summary>
        /// Return random charset with linq
        /// </summary>
        /// <returns></returns>
        public string RandomString(string char_set, int length)
        {
            return new string(Enumerable.Repeat(char_set, length)
                .Select(s => s[gen.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Return random int
        /// </summary>
        /// <returns></returns>
        public int RandomInt()
        {
            return gen.Next(0, maxInt + 1);
        }

        /// <summary>
        /// Return random double
        /// </summary>
        /// <returns></returns>
        public double RandomDouble()
        {
            return gen.NextDouble() * maxDouble;
        }

        /// <summary>
        /// Get task1 random set
        /// </summary>
        /// <returns></returns>
        public string RandomSet()
        {
            string randomSet = $"" +
                $"{RandomDay().ToString("dd.MM.yyyy")}||" +
                $"{RandomString(char_ENG, 10)}||" +
                $"{RandomString(char_RUS, 10)}||" +
                $"{RandomInt()}||" +
                $"{RandomDouble()}";

            return randomSet;
        }

        /// <summary>
        /// Generate files with randomizer
        /// </summary>
        /// <param name="randomSet">Randomizer</param>
        /// <param name="fileCount">Count of files</param>
        /// <param name="lineCount">Count of lines in a file</param>
        /// <param name="pathDir">Save directory</param>
        public void GenerateFiles(Func<string> randomSet, int fileCount, int lineCount, string pathDir)
        {
            Directory.CreateDirectory(pathDir);
            for (int i = 0; i < fileCount; i++)
            {
                using (var fs = File.OpenWrite($"{pathDir}\\{i}.txt"))
                using (var w = new StreamWriter(fs))
                {
                    for (var j = 0; j < lineCount; j++)
                    {
                        var line = $"{randomSet()}";
                        w.WriteLine(line);
                    }
                }
            }
        }
    }
}
