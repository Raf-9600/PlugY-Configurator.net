using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace PlugY_Configurator.Models
{
    public class Settings
    {
        private string settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Raf-9600", "PlugY Configurator", "Settings.json");

        [Serializable]
        public struct MainSettings
        {
            public string Language { get; set; }
            public string PlugyIni { get; set; }

            public MainSettings(string language, string plugyIni)
            {
                Language = language;
                PlugyIni = plugyIni;
            }
        }

        private MainSettings json = new MainSettings("", "");
        public (string language, string installationPath) Get()
        {
            if (string.IsNullOrEmpty(json.Language) && string.IsNullOrEmpty(json.PlugyIni))
                if (File.Exists(settingsFile))
                    try
                    {
                        json = JsonSerializer.Deserialize<MainSettings>(File.ReadAllText(settingsFile));

                        return (json.Language, json.PlugyIni);
                    }
                    catch (Exception)
                    { return (json.Language, json.PlugyIni); }

            string instPath = string.Empty;

            if (!string.IsNullOrEmpty(json.PlugyIni) && Directory.Exists(json.PlugyIni))
                instPath = json.PlugyIni;

            return (json.Language, instPath);
        }

        JsonSerializerOptions jsonOption = new JsonSerializerOptions { WriteIndented = true };
        public void Save(string language = "", string installationPath = "")
        {
            if (string.IsNullOrEmpty(installationPath))
                installationPath = json.PlugyIni;

            if (string.IsNullOrEmpty(language))
                language = json.Language;


            if ((installationPath == json.PlugyIni) && (language == json.Language))
                return;


            string p = Path.GetDirectoryName(settingsFile);
            Directory.CreateDirectory(p);

            json = new MainSettings(language, installationPath);
            byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(json, jsonOption);
            File.WriteAllBytes(settingsFile, jsonUtf8Bytes);
        }

    }
}
