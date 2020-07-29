using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace PlugY_Configurator.Models
{
    class Settings
    {
        static private string settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Raf-9600", "PlugY Configurator", "Settings.json");

        [Serializable]
        public struct MainSettings
        {
            public string Lng { get; set; }
            public string PathPlugyIni { get; set; }

            public MainSettings(string lng, string pathPlugyIni)
            {
                Lng = lng;
                PathPlugyIni = pathPlugyIni;
            }
        }

        private static string startSettingsStr;
        public static MainSettings? Get()
        {
            if (File.Exists(settingsFile))
                try
                {
                    startSettingsStr = File.ReadAllText(settingsFile);
                    return JsonSerializer.Deserialize<MainSettings>(startSettingsStr);
                }
                catch (Exception)
                { return null; }
            return null;
        }

        public static void Save(MainSettings result)
        {
            string endSettingsStr = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });

            if (startSettingsStr == endSettingsStr)
                return;

            string p = Path.GetDirectoryName(settingsFile);
            Directory.CreateDirectory(p);


            File.WriteAllText(settingsFile, endSettingsStr);
        }

    }
}
