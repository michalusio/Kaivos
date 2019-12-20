using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assets.Scripts
{
    public static class SaveUtilities
    {
        public static string MapFileName = "map.dat";
        public static string StatFileName = "stats.json";

        private static readonly Regex validSaveNameRegex = new Regex(@"(-|\w|\s|\.|\(|\))+", RegexOptions.Compiled);
        public static bool IsValidName(string saveName)
        {
            return validSaveNameRegex.IsMatch(saveName);
        }

        public static void EnsureSaveFolderPath()
        {
            Directory.CreateDirectory(GetSaveFolderPath());
        }

        public static bool IsValidSave(string savePath)
        {
            var filesInFolder = Directory.GetFiles(savePath);
            return filesInFolder.Length == 2 && filesInFolder.Any(file => Path.GetFileName(file) == MapFileName) && filesInFolder.Any(file => Path.GetFileName(file) == StatFileName);
        }

        public static string GetSaveFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"My Games{Path.DirectorySeparatorChar}Kaivos{Path.DirectorySeparatorChar}");
        }
    }
}
