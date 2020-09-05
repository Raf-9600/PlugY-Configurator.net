using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace PlugY_Configurator.Models
{
    class MainModel
    {
        public bool UpdateFind()
        {
/*
#if RELEASE
            UpdateStruct updateJson = new UpdateStruct(null, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            string updateSerialize = System.Text.Json.JsonSerializer.Serialize(updateJson, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "update.json"), updateSerialize);
#endif*/

            string updateFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Raf-9600", "PlugY Configurator", "update.json");
            string updateUrl = @"https://raw.githubusercontent.com/Raf-9600/PlugY-Configurator/master/PlugY%20Configurator/update.json";

            var dw = DoWork(true);
            if (dw != null) return dw ?? false;

            bool down = DownloadNewFile(updateUrl, updateFile);
            if (!down) return false;

            var result = DoWork(false);
            
            bool? DoWork(bool checkDate)
            {
                if (File.Exists(updateFile))
                {
                    try
                    {
                        string updateFileStr = File.ReadAllText(updateFile);
                        UpdateStruct updateJson = JsonSerializer.Deserialize<UpdateStruct>(updateFileStr);
                        {
                            var versionExe = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                            var versionJson = new Version(updateJson.Ver);

                            if (versionJson > versionExe) return true;

                            if (checkDate)
                            {
                                var dateToday = DateTime.Today;
                                var dateJson = DateTime.Parse(updateJson.Date);
                                var dateJsonWeek = dateJson.AddDays(7);

                                if (dateJsonWeek > dateToday) return false;
                            }
                            else
                            {
                                updateJson.Date = DateTime.Today.ToString();

                                string updateSerialize = JsonSerializer.Serialize(updateJson, new JsonSerializerOptions { WriteIndented = true });
                                File.WriteAllText(updateFile, updateSerialize);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    { }
                }
                return null;
            }

            return result ?? false;
        }

        [Serializable]
        public struct UpdateStruct
        {
            public string Date { get; set; }
            public string Ver { get; set; }

            public UpdateStruct(string date, string ver)
            {
                Date = date;
                Ver = ver;
            }
        }

        private bool DownloadNewFile(string WebPath, string DestPath)
        {
            string dp = Path.GetDirectoryName(DestPath);
            string destPathTemp = Path.Combine(dp, Path.GetRandomFileName());
            Directory.CreateDirectory(dp);
            File.Delete(destPathTemp);
            try
            {
                using System.Net.WebClient client = new System.Net.WebClient();
                client.DownloadFile(WebPath, destPathTemp);
            }
            catch (Exception)
            { }

            if (File.Exists(destPathTemp))
            {
                File.Delete(DestPath);
                File.Move(destPathTemp, DestPath);
                return true;
            }
            return false;
        }

        public string FindPlugyIni(string[] filesArray)
        {
            foreach (var fle in filesArray)
            {
                string fleString = Path.GetFileName(fle);

                if (fleString.Equals("PlugY.ini", StringComparison.OrdinalIgnoreCase))
                    return fle;
            }

            return string.Empty;
        }


        public string FindWorkDir(string nameFile)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), nameFile);

            if (File.Exists(fullPath))
                return Directory.GetCurrentDirectory();


            string setpPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string fullPath1 = Path.Combine(setpPath, nameFile);

            if (File.Exists(fullPath1))
                return setpPath;

            return string.Empty;
        }

        public bool DetectLightTheme()
        {
            using (RegistryKey regHKCU = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", false))
                if (regHKCU != null)
                    return Convert.ToBoolean(regHKCU.GetValue("AppsUseLightTheme"));

            return true;
        }

        private string pathInstalledDiablo2;

        public string FindInstalledDiablo2()
        {
            if (!string.IsNullOrEmpty(pathInstalledDiablo2))
                return pathInstalledDiablo2;

            RegistryKey registryHKCU = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
            if (registryHKCU != null)
            {
                RegistryKey regD2cu = registryHKCU.OpenSubKey(@"Software\Blizzard Entertainment\Diablo II");
                if (regD2cu != null)
                {
                    var result = regD2cu.GetValue("InstallPath");
                    if (result != null)
                        pathInstalledDiablo2 = (string)result;
                }
                regD2cu.Close();
            }
            registryHKCU.Close();

            string d2dataPath = Path.Combine(pathInstalledDiablo2, "d2data.mpq");

            if (File.Exists(d2dataPath))
                return pathInstalledDiablo2;

            return string.Empty;
        }

        public string DlgFindFile(string fileName, string filter, string defaultExt = null, string initialDirectory = null)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            if (!string.IsNullOrEmpty(initialDirectory))
                dlg.InitialDirectory = initialDirectory;

            if (!string.IsNullOrEmpty(defaultExt))
                dlg.DefaultExt = defaultExt;

            dlg.FileName = fileName;
            dlg.Filter = filter;
            dlg.FilterIndex = 0;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
                return dlg.FileName;

            return string.Empty;
        }

        public string ActAddRemove(string mainStr, string newStr)
        {
            mainStr = mainStr.Replace("-act 1", "");
            mainStr = mainStr.Replace("-act 2", "");
            mainStr = mainStr.Replace("-act 3", "");
            mainStr = mainStr.Replace("-act 4", "");
            mainStr = mainStr.Replace("-act 5", "");

            if (newStr == "-act 1")
                return mainStr;

            return mainStr + " " + newStr;
        }

        public string AddParam(string mainStr, string newStr)
        {
            if (mainStr.Contains(newStr))
                return mainStr;
            else return mainStr + " " + newStr;
        }

        private bool? _glideWindowed;
        public void GlideWindowed(bool? enab)
        {
            if (enab == _glideWindowed)
                return;
            _glideWindowed = enab;

            using RegistryKey registryHKCU = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
            if (registryHKCU != null)
            {
                RegistryKey regGlideCU = registryHKCU.CreateSubKey(@"SOFTWARE\GLIDE3toOpenGL");
                regGlideCU?.SetValue("windowed", Convert.ToInt32(enab));
            }
        }


    }
}
