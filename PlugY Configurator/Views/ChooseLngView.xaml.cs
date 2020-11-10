using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlugY_Configurator.View
{
    /// <summary>
    /// Логика взаимодействия для ChooseLngView.xaml
    /// </summary>
    public partial class ChooseLngView
    {
        Dictionary<string, NameLng> mainLng = new Dictionary<string, NameLng>()
        {
            { "en", new NameLng
                {
                    Title = "Select Language",
                    Label = "Select the language to use program.",
                    OK = "OK",
                    Cancel = "Cancel"
                }
            },
            { "ru", new NameLng
                {
                    Title = "Выберите язык",
                    Label = "Выберите язык, который будет использован программой.",
                    OK = "OK",
                    Cancel = "Отмена"
                }
            },
            { "uk", new NameLng // Ukrainian
                {
                    Title = "Оберіть мову",
                    Label = "Оберіть мову, яка буде використовуватися програмою.",
                    OK = "Так",
                    Cancel = "Скасувати"
                }
            }
        };
        private struct NameLng
        {
            public string Title { get; set; }
            public string Label { get; set; }
            public string OK { get; set; }
            public string Cancel { get; set; }
        }

        public ChooseLngView()
        {
            InitializeComponent();

            LangCmbBox.ItemsSource = App.GuiLanguagesList;

            if (!mainLng.TryGetValue(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, out NameLng lng))
                lng = mainLng["en"];

            Title = lng.Title;
            SelectLanguageLabel.Text = lng.Label;
            ButtonOK.Content = lng.OK;
            ButtonCancel.Content = lng.Cancel;
        }

        private void bttnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            //Environment.Exit(0);
        }

        private void bttnOk_Click(object sender, RoutedEventArgs e)
        {
            App.SetLocalGUI(LangCmbBox.SelectedIndex);
            this.Hide();
        }

        private void FormLangChange_CmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LangCmbBox.SelectedIndex != -1)
                ButtonOK.IsEnabled = true;
        }
    }
}
