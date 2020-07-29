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
                {
                    return Convert.ToBoolean(regHKCU.GetValue("AppsUseLightTheme"));
                }
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


    }
}
