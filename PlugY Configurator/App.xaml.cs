using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PlugY_Configurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly Models.Settings _mainSettings = new Models.Settings();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (DetectLightTheme())
                ControlzEx.Theming.ThemeManager.Current.ChangeTheme(System.Windows.Application.Current, "Light.Crimson");
            else ControlzEx.Theming.ThemeManager.Current.ChangeTheme(System.Windows.Application.Current, "Dark.Crimson");


            #region Смена локализации
            bool newLocal = false;

            // Сначала читаем язык из файла настроек
            string currentUICulture = _mainSettings.Get().language;
            if (!string.IsNullOrEmpty(currentUICulture))
                newLocal = SetLocalGUI(currentUICulture);
            else
            {
                // Если локализация ОС совпадает с одной из существующих локализаций программы, тогда применяем
                currentUICulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                newLocal = SetLocalGUI(currentUICulture);
            }

            // Если автоматически выяснить необходимый язык не удалось, тогда спрашиваем пользователя
            if (!newLocal)
            {
                View.ChooseLngView lngView = new View.ChooseLngView();
                lngView.ShowDialog();
                StartMainWindow();
                lngView.Close();
            }
            #endregion
            else StartMainWindow();

            void StartMainWindow()
            {
                var mainWindow = new MainWindow();
                this.MainWindow = mainWindow;
                mainWindow.Show();
                mainWindow.Activate();
            }

        }

        #region Смена локализации
        public static ObservableCollection<CultureInfo> GuiLanguagesList { get; set; } = new ObservableCollection<CultureInfo> { new CultureInfo("ru"), new CultureInfo("de"), new CultureInfo("en") };
        public static int GuiLanguagesNum;

        bool SetLocalGUI(string nameCulture)
        {
            for (int i = 0; i < GuiLanguagesList.Count; i++)
            {
                if (GuiLanguagesList[i].TwoLetterISOLanguageName == nameCulture)
                {
                    GuiLanguagesNum = i;
                    SetLocalGUI(GuiLanguagesList[i]);
                    return true;
                }
            }
            return false;
        }

        public static void SetLocalGUI(int numCulture)
        {
            GuiLanguagesNum = numCulture;
            SetLocalGUI(GuiLanguagesList[numCulture]);
        }

        private static void SetLocalGUI(CultureInfo cuinCulture)
        {
            WpfLocalization.TranslationSource.Instance.CurrentCulture = cuinCulture; // меняем локализацию xaml
            Thread.CurrentThread.CurrentUICulture = cuinCulture; // меняем локализацию в коде
            //Thread.CurrentThread.CurrentCulture = cuinCulture;
            //_mainSettings.Save(cuinCulture.TwoLetterISOLanguageName); // сохраняем выбранную локализацию
        }

        public static void SaveJson(string language = "", string installationPath = "")
        {
            _mainSettings.Save(language, installationPath);
        }
        #endregion

        public bool DetectLightTheme()
        {
            using (RegistryKey regHKCU = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", false))
                if (regHKCU != null)
                {
                    var v = regHKCU.GetValue("AppsUseLightTheme");
                    if (v != null)
                        return Convert.ToBoolean(v);
                }
            return true;
        }
    }
}
