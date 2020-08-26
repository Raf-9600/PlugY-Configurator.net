using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using Path = System.IO.Path;
using System.Windows.Shell;
using System.Drawing;
using PlugY_Configurator.ViewModels;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Controls;
using ControlzEx.Theming;
using System.Collections.ObjectModel;
using PlugY_Configurator.Resources.Translation;
using System.Diagnostics;

namespace PlugY_Configurator.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        readonly Models.MainModel _model = new Models.MainModel();
        private Models.IniWork _ini = new Models.IniWork();
        

        private string _plugyFullPath;
        public string PlugyFullPath
        {
            get { return _plugyFullPath; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                _plugyFullPath = value;
                

                _ini.NotSet = true;
                _ini.Initialization(value);
                try
                {
                    // LAUNCHING
                    Param = _ini.GetVal("LAUNCHING", "Param", string.Empty);
                    Library = _ini.GetVal("LAUNCHING", "Library", string.Empty);

                    // GENERAL
                    ActivePlugin = _ini.GetVal("GENERAL", "ActivePlugin", false);
                    DisableBattleNet = _ini.GetVal("GENERAL", "DisableBattleNet", false);
                    ActiveLogFile = _ini.GetVal("GENERAL", "ActiveLogFile", false);
                    DllToLoad = _ini.GetVal("GENERAL", "DllToLoad", string.Empty);
                    DllToLoad2 = _ini.GetVal("GENERAL", "DllToLoad2", string.Empty);
                    ActiveCommands = _ini.GetVal("GENERAL", "ActiveCommands", false);
                    ActiveCheckMemory = _ini.GetVal("GENERAL", "ActiveCheckMemory", false);
                    ActiveAllOthersFeatures = _ini.GetVal("GENERAL", "ActiveAllOthersFeatures", false);

                    // WINDOWED
                    ActiveWindowed = _ini.GetVal("WINDOWED", "ActiveWindowed", false);
                    RemoveBorder = _ini.GetVal("WINDOWED", "RemoveBorder", false);
                    WindowOnTop = _ini.GetVal("WINDOWED", "WindowOnTop", false);
                    Maximized = _ini.GetVal("WINDOWED", "Maximized", false);
                    SetWindowPos = _ini.GetVal("WINDOWED", "SetWindowPos", false);
                    WindowedX = _ini.GetVal("WINDOWED", "X", 0);
                    WindowedY = _ini.GetVal("WINDOWED", "Y", 0);
                    WindowedWidth = _ini.GetVal("WINDOWED", "Width", 0);
                    WindowedHeight = _ini.GetVal("WINDOWED", "Height", 0);
                    LockMouseOnStartup = _ini.GetVal("WINDOWED", "LockMouseOnStartup", false);

                    // LANGUAGE
                    ActiveChangeLanguage = _ini.GetVal("LANGUAGE", "ActiveChangeLanguage", false);
                    ActiveLanguageManagement = _ini.GetVal("LANGUAGE", "ActiveLanguageManagement", false);

                    var selectedLanguage_ini = _ini.GetVal("LANGUAGE", "SelectedLanguage", string.Empty);
                    SelectedLanguage = Array.IndexOf(_languageListWrite, selectedLanguage_ini);

                    var defaultLanguage_ini = _ini.GetVal("LANGUAGE", "DefaultLanguage", string.Empty);
                    DefaultLanguage = Array.IndexOf(_languageListWrite, defaultLanguage_ini);

                    AvlblLngs_ENG = _ini.GetAvailableLanguages("ENG");
                    AvlblLngs_ESP = _ini.GetAvailableLanguages("ESP");
                    AvlblLngs_DEU = _ini.GetAvailableLanguages("DEU");
                    AvlblLngs_FRA = _ini.GetAvailableLanguages("FRA");
                    AvlblLngs_POR = _ini.GetAvailableLanguages("POR");
                    AvlblLngs_ITA = _ini.GetAvailableLanguages("ITA");
                    AvlblLngs_JPN = _ini.GetAvailableLanguages("JPN");
                    AvlblLngs_KOR = _ini.GetAvailableLanguages("KOR");
                    AvlblLngs_SIN = _ini.GetAvailableLanguages("SIN");
                    AvlblLngs_CHI = _ini.GetAvailableLanguages("CHI");
                    AvlblLngs_POL = _ini.GetAvailableLanguages("POL");
                    AvlblLngs_RUS = _ini.GetAvailableLanguages("RUS");


                    // SAVEPATH
                    ActiveSavePathChange = _ini.GetVal("SAVEPATH", "ActiveSavePathChange", false);
                    SavePath = _ini.GetVal("SAVEPATH", "SavePath", string.Empty);

                    // MAIN SCREEN
                    ActiveVersionTextChange = _ini.GetVal("MAIN SCREEN", "ActiveVersionTextChange", false);
                    VersionText = _ini.GetVal("MAIN SCREEN", "VersionText", string.Empty);
                    ColorOfVersionText = _ini.GetVal("MAIN SCREEN", "ColorOfVersionText", 0);
                    ActivePrintPlugYVersion = _ini.GetVal("MAIN SCREEN", "ActivePrintPlugYVersion", false);
                    ColorOfPlugYVersion = _ini.GetVal("MAIN SCREEN", "ColorOfPlugYVersion", 0);

                    // STASH
                    ActiveBigStash = _ini.GetVal("STASH", "ActiveBigStash", false);
                    ActiveMultiPageStash = _ini.GetVal("STASH", "ActiveMultiPageStash", false);
                    NbPagesPerIndex = _ini.GetVal("STASH", "NbPagesPerIndex", 0);
                    NbPagesPerIndex2 = _ini.GetVal("STASH", "NbPagesPerIndex2", 0);
                    MaxPersonnalPages = _ini.GetVal("STASH", "MaxPersonnalPages", 0);
                    ActiveSharedStash = _ini.GetVal("STASH", "ActiveSharedStash", false);
                    SeparateHardcoreStash = _ini.GetVal("STASH", "SeparateHardcoreStash", false);
                    OpenSharedStashOnLoading = _ini.GetVal("STASH", "OpenSharedStashOnLoading", false);
                    SharedStashFilename = _ini.GetVal("STASH", "SharedStashFilename", string.Empty);
                    DisplaySharedSetItemNameInGreen = _ini.GetVal("STASH", "DisplaySharedSetItemNameInGreen", false);
                    MaxSharedPages = _ini.GetVal("STASH", "MaxSharedPages", 0);
                    ActiveSharedGold = _ini.GetVal("STASH", "ActiveSharedGold", false);
                    PosXPreviousBtn = _ini.GetVal("STASH", "PosXPreviousBtn", -1);
                    PosYPreviousBtn = _ini.GetVal("STASH", "PosYPreviousBtn", -1);
                    PosXNextBtn = _ini.GetVal("STASH", "PosXNextBtn", -1);
                    PosYNextBtn = _ini.GetVal("STASH", "PosYNextBtn", -1);
                    PosXSharedBtn = _ini.GetVal("STASH", "PosXSharedBtn", -1);
                    PosYSharedBtn = _ini.GetVal("STASH", "PosYSharedBtn", -1);
                    PosXPreviousIndexBtn = _ini.GetVal("STASH", "PosXPreviousIndexBtn", -1);
                    PosYPreviousIndexBtn = _ini.GetVal("STASH", "PosYPreviousIndexBtn", -1);
                    PosXNextIndexBtn = _ini.GetVal("STASH", "PosXNextIndexBtn", -1);
                    PosYNextIndexBtn = _ini.GetVal("STASH", "PosYNextIndexBtn", -1);
                    PosXPutGoldBtn = _ini.GetVal("STASH", "PosXPutGoldBtn", -1);
                    PosYPutGoldBtn = _ini.GetVal("STASH", "PosYPutGoldBtn", -1);
                    PosXTakeGoldBtn = _ini.GetVal("STASH", "PosXTakeGoldBtn", -1);
                    PosYTakeGoldBtn = _ini.GetVal("STASH", "PosYTakeGoldBtn", -1);

                    // STATS POINTS
                    ActiveStatsUnassignment = _ini.GetVal("STATS POINTS", "ActiveStatsUnassignment", false);
                    var keyUsed_ini = _ini.GetVal("STATS POINTS", "KeyUsed", 18);
                    if (keyUsed_ini == 17) KeyUsed = 0;
                    else if (keyUsed_ini == 18) KeyUsed = 1;

                    ActiveShiftClickLimit = _ini.GetVal("STATS POINTS", "ActiveShiftClickLimit", false);
                    LimitValueToShiftClick = _ini.GetVal("STATS POINTS", "LimitValueToShiftClick", 5);

                    // STAT ON LEVEL UP
                    ActiveStatPerLevelUp = _ini.GetVal("STAT ON LEVEL UP", "ActiveStatPerLevelUp", false);
                    StatPerLevelUp = _ini.GetVal("STAT ON LEVEL UP", "StatPerLevelUp", 0);

                    // SKILLS POINTS
                    ActiveSkillsUnassignment = _ini.GetVal("SKILLS POINTS", "ActiveSkillsUnassignment", false);
                    ActiveSkillsUnassignmentOneForOne = _ini.GetVal("SKILLS POINTS", "ActiveSkillsUnassignmentOneByOne", false);
                    PosXUnassignSkillBtn = _ini.GetVal("SKILLS POINTS", "PosXUnassignSkillBtn", -1);
                    PosYUnassignSkillBtn = _ini.GetVal("SKILLS POINTS", "PosYUnassignSkillBtn", -1);

                    // SKILL ON LEVEL UP
                    ActiveSkillPerLevelUp = _ini.GetVal("SKILL ON LEVEL UP", "ActiveSkillPerLevelUp", false);
                    SkillPerLevelUp = _ini.GetVal("SKILL ON LEVEL UP", "SkillPerLevelUp", 1);

                    // WORLD EVENT
                    ActiveWorldEvent = _ini.GetVal("WORLD EVENT", "ActiveWorldEvent", false);
                    ShowCounterInAllDifficulty = _ini.GetVal("WORLD EVENT", "ShowCounterInAllDifficulty", false);
                    ItemsToSell = _ini.GetVal("WORLD EVENT", "ItemsToSell", string.Empty);
                    MonsterID = _ini.GetVal("WORLD EVENT", "MonsterID", 333);
                    OwnSOJSoldChargeFor = _ini.GetVal("WORLD EVENT", "OwnSOJSoldChargeFor", 100);
                    InititalSOJSoldMin = _ini.GetVal("WORLD EVENT", "InititalSOJSoldMin", 200);
                    InititalSOJSoldMax = _ini.GetVal("WORLD EVENT", "InititalSOJSoldMax", 3000);
                    TriggerAtEachSOJSoldMin = _ini.GetVal("WORLD EVENT", "TriggerAtEachSOJSoldMin", 75);
                    TriggerAtEachSOJSoldMax = _ini.GetVal("WORLD EVENT", "TriggerAtEachSOJSoldMax", 125);
                    ActiveAutoSell = _ini.GetVal("WORLD EVENT", "ActiveAutoSell", false);
                    TimeBeforeAutoSellMin = _ini.GetVal("WORLD EVENT", "TimeBeforeAutoSellMin", 0);
                    TimeBeforeAutoSellMax = _ini.GetVal("WORLD EVENT", "TimeBeforeAutoSellMax", 1200);

                    // UBER QUEST
                    ActiveUberQuest = _ini.GetVal("UBER QUEST", "ActiveUberQuest", false);

                    // INTERFACE
                    ActiveNewStatsInterface = _ini.GetVal("INTERFACE", "ActiveNewStatsInterface", false);
                    SelectMainPageOnOpenning = _ini.GetVal("INTERFACE", "SelectMainPageOnOpenning", false);
                    PrintButtonsBackgroundOnMainStatsPage = _ini.GetVal("INTERFACE", "PrintButtonsBackgroundOnMainStatsPage", false);

                    // EXTRA
                    ActiveLaunchAnyNumberOfLOD = _ini.GetVal("EXTRA", "ActiveLaunchAnyNumberOfLOD", false);
                    AlwaysRegenMapInSP = _ini.GetVal("EXTRA", "AlwaysRegenMapInSP", false);
                    NBPlayersByDefault = _ini.GetVal("EXTRA", "NBPlayersByDefault", 0);
                    ActiveDisplayItemLevel = _ini.GetVal("EXTRA", "ActiveDisplayItemLevel", false);
                    AlwaysDisplayLifeAndManaValues = _ini.GetVal("EXTRA", "AlwaysDisplayLifeAndManaValues", 0);
                    EnabledTXTFilesWhenMSExcelOpenIt = _ini.GetVal("EXTRA", "EnabledTXTFilesWhenMSExcelOpenIt", false);
                    ActiveDisplayBaseStatsValue = _ini.GetVal("EXTRA", "ActiveDisplayBaseStatsValue", false);
                    ActiveLadderRunewords = _ini.GetVal("EXTRA", "ActiveLadderRunewords", false);
                    ActiveCowPortalWhenCowKingWasKilled = _ini.GetVal("EXTRA", "ActiveCowPortalWhenCowKingWasKilled", false);
                    ActiveDoNotCloseNihlathakPortal = _ini.GetVal("EXTRA", "ActiveDoNotCloseNihlathakPortal", false);
                }
                catch (Exception)
                { }

                _ini.NotSet = false;

                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            // Если в Винде тёмная тема, то включаем её и в программе
            if (!_model.DetectLightTheme())
                ThemeManager.Current.ChangeTheme(System.Windows.Application.Current, "Dark.Crimson");

            DpiScale dpi = VisualTreeHelper.GetDpi(new System.Windows.Controls.Control());
            double screenRealWidth = SystemParameters.PrimaryScreenWidth * dpi.DpiScaleX;

            //if (currentUICulture == "ru") MainWindowWidth = 1330;
            if (MainWindowWidth > screenRealWidth)
                MainWindowWidth = screenRealWidth - 20;


            string workFile = string.Empty;

            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
                workFile = _model.FindPlugyIni(args);



            string currentUICulture;
            var sett = Models.Settings.Get();
            if (sett != null)
            {
                currentUICulture = sett.Value.Lng;

                if (string.IsNullOrEmpty(workFile))
                    workFile = sett.Value.PathPlugyIni;
            }
            else
            {
                currentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            }


            for (int i = 0; i < Sttngs_Languages.Count; i++)
            {
                if (Sttngs_Languages[i].TwoLetterISOLanguageName == currentUICulture)
                {
                    Sttngs_Languages_Index = i;
                    break;
                }
            }

            if (Sttngs_Languages_Index == -1)
            {
                currentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
                for (int i = 0; i < Sttngs_Languages.Count; i++)
                {
                    if (Sttngs_Languages[i].TwoLetterISOLanguageName == currentUICulture)
                    {
                        Sttngs_Languages_Index = i;
                        break;
                    }
                }
            }

            if (!File.Exists(workFile))
            {
                workFile = _model.FindWorkDir("PlugY.ini");
                workFile = Path.Combine(workFile, "PlugY.ini");
            }

            if (File.Exists(workFile))
                PlugyFullPath = workFile;
            else
            {
                findPIni:
                MessageBoxResult msgBx = MessageBox.Show(lang.PlugYiniNotFound_Question, lang.PlugYiniNotFound_Heading, MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (msgBx == MessageBoxResult.OK)
                {
                    PlugYRefresh_Click.Execute(null);
                    if (string.IsNullOrEmpty(PlugyFullPath))
                        goto findPIni;
                }
                else
                    Environment.Exit(0);
            }
        }

        public ICommand WindowLoaded
        {
            get
            {
                return new RelayCommand<RoutedEventArgs>(async (args) =>
                {
                    // Выравниваем окно по центру
                    Rect workArea = System.Windows.SystemParameters.WorkArea;
                    Application.Current.MainWindow.Left = (workArea.Width - Application.Current.MainWindow.ActualWidth) / 2 + workArea.Left;
                    Application.Current.MainWindow.Top = (workArea.Height - Application.Current.MainWindow.ActualHeight) / 2 + workArea.Top;
                });
            }
        }

        public ICommand WindowContentRendered
        {
            get
            {
                return new RelayCommand<EventArgs>(async (args) =>
                {
                    System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                    dispatcherTimer.Tick += async (sender, args) =>
                    {
                        await Task.Run(() =>
                        {
                            if (_model.UpdateFind())
                                NewVer_Visab = Visibility.Visible;
                        });


                        dispatcherTimer.Stop();
                    };

                    dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                    dispatcherTimer.Start();
                });
            }
        }

        public ICommand WindowClosing
        {
            get
            {
                return new RelayCommand<CancelEventArgs>((args) =>
                {
                    Models.Settings.Save(new Models.Settings.MainSettings(Sttngs_Languages[Sttngs_Languages_Index].TwoLetterISOLanguageName, PlugyFullPath));
                });
            }
        }

        public ICommand EventDrop
        {
            get
            {
                return new RelayCommand<DragEventArgs>( (args) =>
                {
                    string[] files = (string[])args.Data.GetData(DataFormats.FileDrop);

                    PlugyFullPath = _model.FindPlugyIni(files);

                });
            }
        }

        #region MainSettings

        private ICommand _btnStart_Click;
        public ICommand BtnStart_Click
        {
            get
            {
                return _btnStart_Click ?? (_btnStart_Click = new RelayCommand(async () =>
                {


                }));
            }
        }

        public ObservableCollection<CultureInfo> Sttngs_Languages { get; set; } = new ObservableCollection<CultureInfo> { new CultureInfo("ru"),  new CultureInfo("de") };


        private int _sttngs_Languages_Index = -1;
        public int Sttngs_Languages_Index
        {
            get { return _sttngs_Languages_Index; }
            set
            {
                if (value != _sttngs_Languages_Index)
                {
                    if (value != -1)
                        WpfLocalization.TranslationSource.Instance.CurrentCulture = Sttngs_Languages[value];

                    _sttngs_Languages_Index = value;
                    OnPropertyChanged();
                }
            }
        }

        public double _mainWindowWidth = 1330;
        public double MainWindowWidth
        {
            get { return _mainWindowWidth; }
            set
            {
                _mainWindowWidth = value;
                OnPropertyChanged();

                ContentMainWindowWidth = value + 20;
            }
        }

        public double _contentMainWindowWidth = 1330;
        public double ContentMainWindowWidth
        {
            get { return _contentMainWindowWidth; }
            set
            {
                _contentMainWindowWidth = value;
                OnPropertyChanged();
            }
        }

        private ICommand _newVer_Click;
        public ICommand NewVer_Click
        {
            get
            {
                return _newVer_Click ?? (_newVer_Click = new RelayCommand(() =>
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = @"https://github.com/Raf-9600/PlugY-Configurator/releases",
                        UseShellExecute = true
                    };
                    Process.Start(psi);

                }));
            }
        }

        public Visibility _newVer_Visab = Visibility.Collapsed;
        public Visibility NewVer_Visab
        {
            get { return _newVer_Visab; }
            set
            {
                _newVer_Visab = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private ICommand _plugYRefresh_Click;
        public ICommand PlugYRefresh_Click
        {
            get
            {
                return _plugYRefresh_Click ?? (_plugYRefresh_Click = new RelayCommand(() =>
                {
                    string pIni = _model.DlgFindFile("PlugY.ini", $"PlugY.ini|PlugY.ini|{lang.DlgFolderPlugyIni_AllIni}|*.ini|{lang.DlgFolderPlugyIni_AllFiles}|*.*", ".ini", _model.FindInstalledDiablo2());

                    if(!string.IsNullOrEmpty(pIni))
                        PlugyFullPath = pIni;

                }));
            }
        }

        #region LaunchParam_Flyout
        private bool _param_Flyout_Open;
        public bool Param_Flyout_Open
        {
            get { return _param_Flyout_Open; }
            set 
            {
                if (value)
                {
                    Act1 = Param.Contains("-act 1");
                    Act2 = Param.Contains("-act 2");
                    Act3 = Param.Contains("-act 3");
                    Act4 = Param.Contains("-act 4");
                    Act5 = Param.Contains("-act 5");

                    GameParam_WindowMode = Param.Contains("-w");
                    GameParam_nofixaspect = Param.Contains("-nofixaspect");
                    GameParam_Direct = Param.Contains("-direct");
                    GameParam_Txt = Param.Contains("-txt");
                    GameParam_Ns = Param.Contains("-ns");
                    GameParam_sndbkg = Param.Contains("-sndbkg");
                    GameParam_skiptobnet = Param.Contains("-skiptobnet");
                    GameParam_nosave = Param.Contains("-nosave");
                    GameParam_3dfx = Param.Contains("-3dfx");
                }

                _param_Flyout_Open = value;
                OnPropertyChanged();
            }
        }

        private bool _act1;
        public bool Act1
        {
            get { return _act1; }
            set 
            {
                _act1 = value;

                if (value)
                    Param = _model.ActAddRemove(Param, "-act 1");
                OnPropertyChanged();
            }
        }

        private bool _act2;
        public bool Act2
        {
            get { return _act2; }
            set 
            { 
                _act2 = value;

                if (value)
                    Param = _model.ActAddRemove(Param, "-act 2");
                OnPropertyChanged();
            }
        }

        private bool _act3;
        public bool Act3
        {
            get { return _act3; }
            set 
            { 
                _act3 = value;

                if (value)
                    Param = _model.ActAddRemove(Param, "-act 3");
                OnPropertyChanged();
            }
        }

        private bool _act4;
        public bool Act4
        {
            get { return _act4; }
            set 
            { 
                _act4 = value;

                if (value)
                    Param = _model.ActAddRemove(Param, "-act 4");
                OnPropertyChanged();
            }
        }

        private bool _act5;
        public bool Act5
        {
            get { return _act5; }
            set 
            { 
                _act5 = value;

                if (value)
                    Param = _model.ActAddRemove(Param, "-act 5");
                OnPropertyChanged();
            }
        }

        private bool _gameParam_WindowMode;
        public bool GameParam_WindowMode
        {
            get { return _gameParam_WindowMode; }
            set
            {
                _gameParam_WindowMode = value;

                if (value)
                    Param = _model.AddParam(Param, "-w");
                else Param = Param.Replace("-w", "");

                OnPropertyChanged();
            }
        }

        private bool _gameParam_nofixaspect;
        public bool GameParam_nofixaspect
        {
            get { return _gameParam_nofixaspect; }
            set
            {
                _gameParam_nofixaspect = value;

                if (value)
                    Param = _model.AddParam(Param, "-nofixaspect");
                else Param = Param.Replace("-nofixaspect", "");

                OnPropertyChanged();
            }
        }

        private bool _gameParam_Direct;
        public bool GameParam_Direct
        {
            get { return _gameParam_Direct; }
            set
            {
                _gameParam_Direct = value;

                if (value)
                    Param = _model.AddParam(Param, "-direct");
                else Param = Param.Replace("-direct", "");

                OnPropertyChanged();
            }
        }

        private bool _gameParam_Txt;
        public bool GameParam_Txt
        {
            get { return _gameParam_Txt; }
            set
            {
                _gameParam_Txt = value;

                if (value)
                    Param = _model.AddParam(Param, "-txt");
                else Param = Param.Replace("-txt", "");

                OnPropertyChanged();
            }
        }

        private bool _gameParam_Ns;
        public bool GameParam_Ns
        {
            get { return _gameParam_Ns; }
            set
            {
                _gameParam_Ns = value;

                if (value)
                    Param = _model.AddParam(Param, "-ns");
                else Param = Param.Replace("-ns", "");

                OnPropertyChanged();
            }
        }

        private bool _gameParam_sndbkg;
        public bool GameParam_sndbkg
        {
            get { return _gameParam_sndbkg; }
            set
            {
                _gameParam_sndbkg = value;

                if (value)
                    Param = _model.AddParam(Param, "-sndbkg");
                else Param = Param.Replace("-sndbkg", "");

                OnPropertyChanged();
            }
        }

        private bool _gameParam_skiptobnet;
        public bool GameParam_skiptobnet
        {
            get { return _gameParam_skiptobnet; }
            set
            {
                _gameParam_skiptobnet = value;

                if (value)
                    Param = _model.AddParam(Param, "-skiptobnet");
                else Param = Param.Replace("-skiptobnet", "");

                OnPropertyChanged();
            }
        }

        private bool _gameParam_nosave;
        public bool GameParam_nosave
        {
            get { return _gameParam_nosave; }
            set
            {
                _gameParam_nosave = value;

                if (value)
                    Param = _model.AddParam(Param, "-nosave");
                else Param = Param.Replace("-nosave", "");

                OnPropertyChanged();
            }
        }

        private bool _gameParam_3dfx;
        public bool GameParam_3dfx
        {
            get { return _gameParam_3dfx; }
            set
            {
                _gameParam_3dfx = value;

                if (value)
                    Param = _model.AddParam(Param, "-3dfx");
                else Param = Param.Replace("-3dfx", "");

                OnPropertyChanged();
            }
        }
        #endregion

        #region [LAUNCHING]
        private ICommand _param_Open;
        public ICommand Param_Open
        {
            get
            {
                return _param_Open ?? (_param_Open = new RelayCommand(() =>
                {
                    Param_Flyout_Open = true;
                }));
            }
        }

        private string _param = "";
        public string Param
        {
            get { return _param; }
            set
            {
                value = value.ToLower().Trim();
                value = value.Replace("  ", " ");

                if (value != _param)
                {
                    _param = value;
                    OnPropertyChanged();
                }
                _ini.SetVal("LAUNCHING", "Param", value);
            }
        }

        private string _library = "";
        public string Library
        {
            get { return _library; }
            set
            {
                if (value != _library)
                {
                    _library = value;
                    OnPropertyChanged();
                }
                _ini.SetVal("LAUNCHING", "Library", value);
            }
        }
        #endregion

        #region [GENERAL]
        private bool _activePlugin;
        public bool ActivePlugin
        {
            get { return _activePlugin; }
            set
            {
                _ini.SetVal("GENERAL", "ActivePlugin", value);

                _activePlugin = value;
                OnPropertyChanged();
            }
        }

        private bool _disableBattleNet;
        public bool DisableBattleNet
        {
            get { return _disableBattleNet; }
            set
            {
                _ini.SetVal("GENERAL", "DisableBattleNet", value);

                _disableBattleNet = value;
                OnPropertyChanged();
            }
        }

        private bool _activeLogFile;
        public bool ActiveLogFile
        {
            get { return _activeLogFile; }
            set
            {
                _ini.SetVal("GENERAL", "ActiveLogFile", value);

                _activeLogFile = value;
                OnPropertyChanged();
            }
        }

        private string _dllToLoad;
        public string DllToLoad
        {
            get { return _dllToLoad; }
            set
            {
                if (value != _dllToLoad)
                {
                    _dllToLoad = value;
                    OnPropertyChanged();
                }
                _ini.SetVal("GENERAL", "DllToLoad", value);
            }
        }

        private string _dllToLoad2;
        public string DllToLoad2
        {
            get { return _dllToLoad2; }
            set
            {
                if (value != _dllToLoad2)
                {
                    _dllToLoad2 = value;
                    OnPropertyChanged();
                }
                _ini.SetVal("GENERAL", "DllToLoad2", value);
            }
        }

        private bool _activeCommands;
        public bool ActiveCommands
        {
            get { return _activeCommands; }
            set
            {
                _ini.SetVal("GENERAL", "ActiveCommands", value);

                _activeCommands = value;
                OnPropertyChanged();
            }
        }

        private bool _activeCheckMemory;
        public bool ActiveCheckMemory
        {
            get { return _activeCheckMemory; }
            set
            {
                _ini.SetVal("GENERAL", "ActiveCheckMemory", value);

                _activeCheckMemory = value;
                OnPropertyChanged();
            }
        }

        private bool _activeAllOthersFeatures;
        public bool ActiveAllOthersFeatures
        {
            get { return _activeAllOthersFeatures; }
            set
            {
                _ini.SetVal("GENERAL", "ActiveAllOthersFeatures", value);

                _activeAllOthersFeatures = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [WINDOWED]
        private bool _activeWindowed;
        public bool ActiveWindowed
        {
            get { return _activeWindowed; }
            set
            {
                _ini.SetVal("WINDOWED", "ActiveWindowed", value);

                _activeWindowed = value;
                OnPropertyChanged();
            }
        }

        private bool _removeBorder;
        public bool RemoveBorder
        {
            get { return _removeBorder; }
            set
            {
                _ini.SetVal("WINDOWED", "RemoveBorder", value);

                _removeBorder = value;
                OnPropertyChanged();
            }
        }

        private bool _windowOnTop;
        public bool WindowOnTop
        {
            get { return _windowOnTop; }
            set
            {
                _ini.SetVal("WINDOWED", "WindowOnTop", value);

                _windowOnTop = value;
                OnPropertyChanged();
            }
        }

        private bool _maximized;
        public bool Maximized
        {
            get { return _maximized; }
            set
            {
                _ini.SetVal("WINDOWED", "Maximized", value);

                _maximized = value;
                OnPropertyChanged();
            }
        }

        private bool _setWindowPos;
        public bool SetWindowPos
        {
            get { return _setWindowPos; }
            set
            {
                _ini.SetVal("WINDOWED", "SetWindowPos", value);

                _setWindowPos = value;
                OnPropertyChanged();
            }
        }

        private int _windowedX;
        public int WindowedX
        {
            get { return _windowedX; }
            set
            {
                _ini.SetVal("WINDOWED", "X", value);

                _windowedX = value;
                OnPropertyChanged();
            }
        }

        private int _windowedY;
        public int WindowedY
        {
            get { return _windowedY; }
            set
            {
                _ini.SetVal("WINDOWED", "Y", value);

                _windowedY = value;
                OnPropertyChanged();
            }
        }

        private int _windowedWidth;
        public int WindowedWidth
        {
            get { return _windowedWidth; }
            set
            {
                _ini.SetVal("WINDOWED", "Width", value);

                _windowedWidth = value;
                OnPropertyChanged();
            }
        }

        private int _windowedHeight;
        public int WindowedHeight
        {
            get { return _windowedHeight; }
            set
            {
                _ini.SetVal("WINDOWED", "Height", value);

                _windowedHeight = value;
                OnPropertyChanged();
            }
        }

        private bool _lockMouseOnStartup;
        public bool LockMouseOnStartup
        {
            get { return _lockMouseOnStartup; }
            set
            {
                _ini.SetVal("WINDOWED", "LockMouseOnStartup", value);

                _lockMouseOnStartup = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [LANGUAGE]
        private bool _activeChangeLanguage;
        public bool ActiveChangeLanguage
        {
            get { return _activeChangeLanguage; }
            set
            {
                _ini.SetVal("LANGUAGE", "ActiveChangeLanguage", value);

                _activeChangeLanguage = value;
                OnPropertyChanged();
            }
        }
        //public ObservableCollection<string> LanguageList { get; set; } = new ObservableCollection<string> { lang.LocalizationENG, lang.LocalizationESP, lang.LocalizationDEU, lang.LocalizationFRA, lang.LocalizationPOR, lang.LocalizationITA, lang.LocalizationJPN, lang.LocalizationKOR, lang.LocalizationSIN, lang.LocalizationCHI, lang.LocalizationPOL, lang.LocalizationRUS };
        private readonly string[] _languageListWrite = new string[] { "ENG", "ESP", "DEU", "FRA", "POR", "ITA", "JPN", "KOR", "SIN", "CHI", "POL", "RUS" };

        private int _selectedLanguage;
        public int SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                var result = _languageListWrite[value];
                _ini.SetVal("LANGUAGE", "SelectedLanguage", result);

                _selectedLanguage = value;
                OnPropertyChanged();
            }
        }

        private bool _activeLanguageManagement;
        public bool ActiveLanguageManagement
        {
            get { return _activeLanguageManagement; }
            set
            {
                _ini.SetVal("LANGUAGE", "ActiveLanguageManagement", value);

                _activeLanguageManagement = value;
                OnPropertyChanged();
            }
        }

        private int _defaultLanguage;
        public int DefaultLanguage
        {
            get { return _defaultLanguage; }
            set
            {
                var result = _languageListWrite[value];
                _ini.SetVal("LANGUAGE", "DefaultLanguage", result);

                _defaultLanguage = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_ENG;
        public bool AvlblLngs_ENG
        {
            get { return _avlblLngs_ENG; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("ENG");
                else _ini.DelAvailableLanguages("ENG");

                _avlblLngs_ENG = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_ESP;
        public bool AvlblLngs_ESP
        {
            get { return _avlblLngs_ESP; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("ESP");
                else _ini.DelAvailableLanguages("ESP");

                _avlblLngs_ESP = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_DEU;
        public bool AvlblLngs_DEU
        {
            get { return _avlblLngs_DEU; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("DEU");
                else _ini.DelAvailableLanguages("DEU");

                _avlblLngs_DEU = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_FRA;
        public bool AvlblLngs_FRA
        {
            get { return _avlblLngs_FRA; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("FRA");
                else _ini.DelAvailableLanguages("FRA");

                _avlblLngs_FRA = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_POR;
        public bool AvlblLngs_POR
        {
            get { return _avlblLngs_POR; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("POR");
                else _ini.DelAvailableLanguages("POR");

                _avlblLngs_POR = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_ITA;
        public bool AvlblLngs_ITA
        {
            get { return _avlblLngs_ITA; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("ITA");
                else _ini.DelAvailableLanguages("ITA");

                _avlblLngs_ITA = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_JPN;
        public bool AvlblLngs_JPN
        {
            get { return _avlblLngs_JPN; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("JPN");
                else _ini.DelAvailableLanguages("JPN");

                _avlblLngs_JPN = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_KOR;
        public bool AvlblLngs_KOR
        {
            get { return _avlblLngs_KOR; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("KOR");
                else _ini.DelAvailableLanguages("KOR");

                _avlblLngs_KOR = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_SIN;
        public bool AvlblLngs_SIN
        {
            get { return _avlblLngs_SIN; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("SIN");
                else _ini.DelAvailableLanguages("SIN");

                _avlblLngs_SIN = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_CHI;
        public bool AvlblLngs_CHI
        {
            get { return _avlblLngs_CHI; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("CHI");
                else _ini.DelAvailableLanguages("CHI");

                _avlblLngs_CHI = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_POL;
        public bool AvlblLngs_POL
        {
            get { return _avlblLngs_POL; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("POL");
                else _ini.DelAvailableLanguages("POL");

                _avlblLngs_POL = value;
                OnPropertyChanged();
            }
        }

        private bool _avlblLngs_RUS;
        public bool AvlblLngs_RUS
        {
            get { return _avlblLngs_RUS; }
            set
            {
                if (value)
                    _ini.SetAvailableLanguages("RUS");
                else _ini.DelAvailableLanguages("RUS");

                _avlblLngs_RUS = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [SAVEPATH]
        private bool _activeSavePathChange;
        public bool ActiveSavePathChange
        {
            get { return _activeSavePathChange; }
            set
            {
                _ini.SetVal("SAVEPATH", "ActiveSavePathChange", value);

                _activeSavePathChange = value;
                OnPropertyChanged();
            }
        }

        private string _savePath;
        public string SavePath
        {
            get { return _savePath; }
            set
            {
                _ini.SetVal("SAVEPATH", "SavePath", value);

                _savePath = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [MAIN_SCREEN]
        private bool _activeVersionTextChange;
        public bool ActiveVersionTextChange
        {
            get { return _activeVersionTextChange; }
            set
            {
                _ini.SetVal("MAIN SCREEN", "ActiveVersionTextChange", value);

                _activeVersionTextChange = value;
                OnPropertyChanged();
            }
        }

        private string _versionText;
        public string VersionText
        {
            get { return _versionText; }
            set
            {
                _ini.SetVal("MAIN SCREEN", "VersionText", value);

                _versionText = value;
                OnPropertyChanged();
            }
        }

        private int _сolorOfVersionText;
        public int ColorOfVersionText
        {
            get { return _сolorOfVersionText; }
            set
            {
                _ini.SetVal("MAIN SCREEN", "ColorOfVersionText", value);

                _сolorOfVersionText = value;
                OnPropertyChanged();
            }
        }

        private bool _activePrintPlugYVersion;
        public bool ActivePrintPlugYVersion
        {
            get { return _activePrintPlugYVersion; }
            set
            {
                _ini.SetVal("MAIN SCREEN", "ActivePrintPlugYVersion", value);

                _activePrintPlugYVersion = value;
                OnPropertyChanged();
            }
        }

        private int _colorOfPlugYVersion;
        public int ColorOfPlugYVersion
        {
            get { return _colorOfPlugYVersion; }
            set
            {
                _ini.SetVal("MAIN SCREEN", "ColorOfPlugYVersion", value);

                _colorOfPlugYVersion = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [STASH]
        private bool _activeBigStash;
        public bool ActiveBigStash
        {
            get { return _activeBigStash; }
            set
            {
                _ini.SetVal("STASH", "ActiveBigStash", value);

                _activeBigStash = value;
                OnPropertyChanged();
            }
        }

        private bool _activeMultiPageStash;
        public bool ActiveMultiPageStash
        {
            get { return _activeMultiPageStash; }
            set
            {
                _ini.SetVal("STASH", "ActiveMultiPageStash", value);

                _activeMultiPageStash = value;
                OnPropertyChanged();
            }
        }

        private int _nbPagesPerIndex;
        public int NbPagesPerIndex
        {
            get { return _nbPagesPerIndex; }
            set
            {
                _ini.SetVal("STASH", "NbPagesPerIndex", value);

                _nbPagesPerIndex = value;
                OnPropertyChanged();
            }
        }

        private int _nbPagesPerIndex2;
        public int NbPagesPerIndex2
        {
            get { return _nbPagesPerIndex2; }
            set
            {
                _ini.SetVal("STASH", "NbPagesPerIndex2", value);

                _nbPagesPerIndex2 = value;
                OnPropertyChanged();
            }
        }

        private int _maxPersonnalPages;
        public int MaxPersonnalPages
        {
            get { return _maxPersonnalPages; }
            set
            {
                _ini.SetVal("STASH", "MaxPersonnalPages", value);

                _maxPersonnalPages = value;
                OnPropertyChanged();
            }
        }

        private bool _activeSharedStash;
        public bool ActiveSharedStash
        {
            get { return _activeSharedStash; }
            set
            {
                _ini.SetVal("STASH", "ActiveSharedStash", value);

                _activeSharedStash = value;
                OnPropertyChanged();
            }
        }

        private string _sharedStashFilename;
        public string SharedStashFilename
        {
            get { return _sharedStashFilename; }
            set
            {
                _ini.SetVal("STASH", "SharedStashFilename", value);

                _sharedStashFilename = value;
                OnPropertyChanged();
            }
        }


        private bool _displaySharedSetItemNameInGreen;
        public bool DisplaySharedSetItemNameInGreen
        {
            get { return _displaySharedSetItemNameInGreen; }
            set
            {
                _ini.SetVal("STASH", "DisplaySharedSetItemNameInGreen", value);

                _displaySharedSetItemNameInGreen = value;
                OnPropertyChanged();
            }
        }

        private int _maxSharedPages;
        public int MaxSharedPages
        {
            get { return _maxSharedPages; }
            set
            {
                _ini.SetVal("STASH", "MaxSharedPages", value);

                _maxSharedPages = value;
                OnPropertyChanged();
            }
        }

        private bool _activeSharedGold;
        public bool ActiveSharedGold
        {
            get { return _activeSharedGold; }
            set
            {
                _ini.SetVal("STASH", "ActiveSharedGold", value);

                _activeSharedGold = value;
                OnPropertyChanged();
            }
        }

        private bool _separateHardcoreStash;
        public bool SeparateHardcoreStash
        {
            get { return _separateHardcoreStash; }
            set
            {
                _ini.SetVal("STASH", "SeparateHardcoreStash", value);

                _separateHardcoreStash = value;
                OnPropertyChanged();
            }
        }

        private bool _openSharedStashOnLoading;
        public bool OpenSharedStashOnLoading
        {
            get { return _openSharedStashOnLoading; }
            set
            {
                _ini.SetVal("STASH", "OpenSharedStashOnLoading", value);

                _openSharedStashOnLoading = value;
                OnPropertyChanged();
            }
        }

        private int _posXPreviousBtn;
        public int PosXPreviousBtn
        {
            get { return _posXPreviousBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXPreviousBtn", value);

                _posXPreviousBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posYPreviousBtn;
        public int PosYPreviousBtn
        {
            get { return _posYPreviousBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYPreviousBtn", value);

                _posYPreviousBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posXNextBtn;
        public int PosXNextBtn
        {
            get { return _posXNextBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXNextBtn", value);

                _posXNextBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posYNextBtn;
        public int PosYNextBtn
        {
            get { return _posYNextBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYNextBtn", value);

                _posYNextBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posXSharedBtn;
        public int PosXSharedBtn
        {
            get { return _posXSharedBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXSharedBtn", value);

                _posXSharedBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posYSharedBtn;
        public int PosYSharedBtn
        {
            get { return _posYSharedBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYSharedBtn", value);

                _posYSharedBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posXPreviousIndexBtn;
        public int PosXPreviousIndexBtn
        {
            get { return _posXPreviousIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXPreviousIndexBtn", value);

                _posXPreviousIndexBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posYPreviousIndexBtn;
        public int PosYPreviousIndexBtn
        {
            get { return _posYPreviousIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYPreviousIndexBtn", value);

                _posYPreviousIndexBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posXNextIndexBtn;
        public int PosXNextIndexBtn
        {
            get { return _posXNextIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXNextIndexBtn", value);

                _posXNextIndexBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posYNextIndexBtn;
        public int PosYNextIndexBtn
        {
            get { return _posYNextIndexBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYNextIndexBtn", value);

                _posYNextIndexBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posXPutGoldBtn;
        public int PosXPutGoldBtn
        {
            get { return _posXPutGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXPutGoldBtn", value);

                _posXPutGoldBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posYPutGoldBtn;
        public int PosYPutGoldBtn
        {
            get { return _posYPutGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYPutGoldBtn", value);

                _posYPutGoldBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posXTakeGoldBtn;
        public int PosXTakeGoldBtn
        {
            get { return _posXTakeGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosXTakeGoldBtn", value);

                _posXTakeGoldBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posYTakeGoldBtn;
        public int PosYTakeGoldBtn
        {
            get { return _posYTakeGoldBtn; }
            set
            {
                _ini.SetVal("STASH", "PosYTakeGoldBtn", value);

                _posYTakeGoldBtn = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [STATS POINTS]
        private bool _activeStatsUnassignment;
        public bool ActiveStatsUnassignment
        {
            get { return _activeStatsUnassignment; }
            set
            {
                _ini.SetVal("STATS POINTS", "ActiveStatsUnassignment", value);

                _activeStatsUnassignment = value;
                OnPropertyChanged();
            }
        }

        private int _keyUsed;
        public int KeyUsed
        {
            get { return _keyUsed; }
            set
            {
                int result = 18;
                if (value == 0) result = 17;
                else if (value == 1) result = 18;

                _ini.SetVal("STATS POINTS", "KeyUsed", result);

                _keyUsed = value;
                OnPropertyChanged();
            }
        }

        private bool _activeShiftClickLimit;
        public bool ActiveShiftClickLimit
        {
            get { return _activeShiftClickLimit; }
            set
            {
                _ini.SetVal("STATS POINTS", "ActiveShiftClickLimit", value);

                _activeShiftClickLimit = value;
                OnPropertyChanged();
            }
        }

        private int _limitValueToShiftClick;
        public int LimitValueToShiftClick
        {
            get { return _limitValueToShiftClick; }
            set
            {
                _ini.SetVal("STATS POINTS", "LimitValueToShiftClick", value);

                _limitValueToShiftClick = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [STAT ON LEVEL UP]
        private bool _activeStatPerLevelUp;
        public bool ActiveStatPerLevelUp
        {
            get { return _activeStatPerLevelUp; }
            set
            {
                _ini.SetVal("STAT ON LEVEL UP", "ActiveStatPerLevelUp", value);

                _activeStatPerLevelUp = value;
                OnPropertyChanged();
            }
        }

        private int _statPerLevelUp;
        public int StatPerLevelUp
        {
            get { return _statPerLevelUp; }
            set
            {
                _ini.SetVal("STAT ON LEVEL UP", "StatPerLevelUp", value);

                _statPerLevelUp = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [SKILLS POINTS]

        private bool _activeSkillsUnassignment;
        public bool ActiveSkillsUnassignment
        {
            get { return _activeSkillsUnassignment; }
            set
            {
                _ini.SetVal("SKILLS POINTS", "ActiveSkillsUnassignment", value);

                _activeSkillsUnassignment = value;
                OnPropertyChanged();
            }
        }

        private bool _activeSkillsUnassignmentOneForOne;
        public bool ActiveSkillsUnassignmentOneForOne
        {
            get { return _activeSkillsUnassignmentOneForOne; }
            set
            {
                _ini.SetVal("SKILLS POINTS", "ActiveSkillsUnassignmentOneByOne", value);

                _activeSkillsUnassignmentOneForOne = value;
                OnPropertyChanged();
            }
        }

        private int _posXUnassignSkillBtn;
        public int PosXUnassignSkillBtn
        {
            get { return _posXUnassignSkillBtn; }
            set
            {
                _ini.SetVal("SKILLS POINTS", "PosXUnassignSkillBtn", value);

                _posXUnassignSkillBtn = value;
                OnPropertyChanged();
            }
        }

        private int _posYUnassignSkillBtn;
        public int PosYUnassignSkillBtn
        {
            get { return _posYUnassignSkillBtn; }
            set
            {
                _ini.SetVal("SKILLS POINTS", "PosYUnassignSkillBtn", value);

                _posYUnassignSkillBtn = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [SKILL ON LEVEL UP]
        private bool _activeSkillPerLevelUp;
        public bool ActiveSkillPerLevelUp
        {
            get { return _activeSkillPerLevelUp; }
            set
            {
                _ini.SetVal("SKILL ON LEVEL UP", "ActiveSkillPerLevelUp", value);

                _activeSkillPerLevelUp = value;
                OnPropertyChanged();
            }
        }

        private int _skillPerLevelUp;
        public int SkillPerLevelUp
        {
            get { return _skillPerLevelUp; }
            set
            {
                _ini.SetVal("SKILL ON LEVEL UP", "SkillPerLevelUp", value);

                _skillPerLevelUp = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [WORLD EVENT]
        private bool _activeWorldEvent;
        public bool ActiveWorldEvent
        {
            get { return _activeWorldEvent; }
            set
            {
                _ini.SetVal("WORLD EVENT", "ActiveWorldEvent", value);

                _activeWorldEvent = value;
                OnPropertyChanged();
            }
        }

        private bool _showCounterInAllDifficulty;
        public bool ShowCounterInAllDifficulty
        {
            get { return _showCounterInAllDifficulty; }
            set
            {
                _ini.SetVal("WORLD EVENT", "ShowCounterInAllDifficulty", value);

                _showCounterInAllDifficulty = value;
                OnPropertyChanged();
            }
        }

        private string _itemsToSell;
        public string ItemsToSell
        {
            get { return _itemsToSell; }
            set
            {
                _ini.SetVal("WORLD EVENT", "ItemsToSell", value);

                _itemsToSell = value;
                OnPropertyChanged();
            }
        }


        private int _monsterID;
        public int MonsterID
        {
            get { return _monsterID; }
            set
            {
                _ini.SetVal("WORLD EVENT", "MonsterID", value);

                _monsterID = value;
                OnPropertyChanged();
            }
        }

        private int _ownSOJSoldChargeFor;
        public int OwnSOJSoldChargeFor
        {
            get { return _ownSOJSoldChargeFor; }
            set
            {
                _ini.SetVal("WORLD EVENT", "OwnSOJSoldChargeFor", value);

                _ownSOJSoldChargeFor = value;
                OnPropertyChanged();
            }
        }

        private int _inititalSOJSoldMin;
        public int InititalSOJSoldMin
        {
            get { return _inititalSOJSoldMin; }
            set
            {
                _ini.SetVal("WORLD EVENT", "InititalSOJSoldMin", value);

                _inititalSOJSoldMin = value;
                OnPropertyChanged();
            }
        }

        private int _inititalSOJSoldMax;
        public int InititalSOJSoldMax
        {
            get { return _inititalSOJSoldMax; }
            set
            {
                _ini.SetVal("WORLD EVENT", "InititalSOJSoldMax", value);

                _inititalSOJSoldMax = value;
                OnPropertyChanged();
            }
        }

        private int _triggerAtEachSOJSoldMin;
        public int TriggerAtEachSOJSoldMin
        {
            get { return _triggerAtEachSOJSoldMin; }
            set
            {
                _ini.SetVal("WORLD EVENT", "TriggerAtEachSOJSoldMin", value);

                _triggerAtEachSOJSoldMin = value;
                OnPropertyChanged();
            }
        }

        private int _triggerAtEachSOJSoldMax;
        public int TriggerAtEachSOJSoldMax
        {
            get { return _triggerAtEachSOJSoldMax; }
            set
            {
                _ini.SetVal("WORLD EVENT", "TriggerAtEachSOJSoldMax", value);

                _triggerAtEachSOJSoldMax = value;
                OnPropertyChanged();
            }
        }

        private bool _activeAutoSell;
        public bool ActiveAutoSell
        {
            get { return _activeAutoSell; }
            set
            {
                _ini.SetVal("WORLD EVENT", "ActiveAutoSell", value);

                _activeAutoSell = value;
                OnPropertyChanged();
            }
        }

        private int _timeBeforeAutoSellMin;
        public int TimeBeforeAutoSellMin
        {
            get { return _timeBeforeAutoSellMin; }
            set
            {
                _ini.SetVal("WORLD EVENT", "TimeBeforeAutoSellMin", value);

                _timeBeforeAutoSellMin = value;
                OnPropertyChanged();
            }
        }

        private int _timeBeforeAutoSellMax;
        public int TimeBeforeAutoSellMax
        {
            get { return _timeBeforeAutoSellMax; }
            set
            {
                _ini.SetVal("WORLD EVENT", "TimeBeforeAutoSellMax", value);

                _timeBeforeAutoSellMax = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [UBER QUEST]
        private bool _activeUberQuest;
        public bool ActiveUberQuest
        {
            get { return _activeUberQuest; }
            set
            {
                _ini.SetVal("UBER QUEST", "ActiveUberQuest", value);

                _activeUberQuest = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [INTERFACE]
        private bool _activeNewStatsInterface;
        public bool ActiveNewStatsInterface
        {
            get { return _activeNewStatsInterface; }
            set
            {
                _ini.SetVal("INTERFACE", "ActiveNewStatsInterface", value);

                _activeNewStatsInterface = value;
                OnPropertyChanged();
            }
        }

        private bool _selectMainPageOnOpenning;
        public bool SelectMainPageOnOpenning
        {
            get { return _selectMainPageOnOpenning; }
            set
            {
                _ini.SetVal("INTERFACE", "SelectMainPageOnOpenning", value);

                _selectMainPageOnOpenning = value;
                OnPropertyChanged();
            }
        }

        private bool _printButtonsBackgroundOnMainStatsPage;
        public bool PrintButtonsBackgroundOnMainStatsPage
        {
            get { return _printButtonsBackgroundOnMainStatsPage; }
            set
            {
                _ini.SetVal("INTERFACE", "PrintButtonsBackgroundOnMainStatsPage", value);

                _printButtonsBackgroundOnMainStatsPage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region [EXTRA]
        private bool _activeLaunchAnyNumberOfLOD;
        public bool ActiveLaunchAnyNumberOfLOD
        {
            get { return _activeLaunchAnyNumberOfLOD; }
            set
            {
                _ini.SetVal("EXTRA", "ActiveLaunchAnyNumberOfLOD", value);

                _activeLaunchAnyNumberOfLOD = value;
                OnPropertyChanged();
            }
        }

        private bool _alwaysRegenMapInSP;
        public bool AlwaysRegenMapInSP
        {
            get { return _alwaysRegenMapInSP; }
            set
            {
                _ini.SetVal("EXTRA", "AlwaysRegenMapInSP", value);

                _alwaysRegenMapInSP = value;
                OnPropertyChanged();
            }
        }

        private int _nBPlayersByDefault;
        public int NBPlayersByDefault
        {
            get { return _nBPlayersByDefault; }
            set
            {
                _ini.SetVal("EXTRA", "NBPlayersByDefault", value);

                _nBPlayersByDefault = value;
                OnPropertyChanged();
            }
        }

        private bool _activeDisplayItemLevel;
        public bool ActiveDisplayItemLevel
        {
            get { return _activeDisplayItemLevel; }
            set
            {
                _ini.SetVal("EXTRA", "ActiveDisplayItemLevel", value);

                _activeDisplayItemLevel = value;
                OnPropertyChanged();
            }
        }

        private int _alwaysDisplayLifeAndManaValues;
        public int AlwaysDisplayLifeAndManaValues
        {
            get { return _alwaysDisplayLifeAndManaValues; }
            set
            {
                _ini.SetVal("EXTRA", "AlwaysDisplayLifeAndManaValues", value);

                _alwaysDisplayLifeAndManaValues = value;
                OnPropertyChanged();
            }
        }

        private bool _enabledTXTFilesWhenMSExcelOpenIt;
        public bool EnabledTXTFilesWhenMSExcelOpenIt
        {
            get { return _enabledTXTFilesWhenMSExcelOpenIt; }
            set
            {
                _ini.SetVal("EXTRA", "EnabledTXTFilesWhenMSExcelOpenIt", value);

                _enabledTXTFilesWhenMSExcelOpenIt = value;
                OnPropertyChanged();
            }
        }

        private bool _activeDisplayBaseStatsValue;
        public bool ActiveDisplayBaseStatsValue
        {
            get { return _activeDisplayBaseStatsValue; }
            set
            {
                _ini.SetVal("EXTRA", "ActiveDisplayBaseStatsValue", value);

                _activeDisplayBaseStatsValue = value;
                OnPropertyChanged();
            }
        }

        private bool _activeLadderRunewords;
        public bool ActiveLadderRunewords
        {
            get { return _activeLadderRunewords; }
            set
            {
                _ini.SetVal("EXTRA", "ActiveLadderRunewords", value);

                _activeLadderRunewords = value;
                OnPropertyChanged();
            }
        }

        private bool _activeCowPortalWhenCowKingWasKilled;
        public bool ActiveCowPortalWhenCowKingWasKilled
        {
            get { return _activeCowPortalWhenCowKingWasKilled; }
            set
            {
                _ini.SetVal("EXTRA", "ActiveCowPortalWhenCowKingWasKilled", value);

                _activeCowPortalWhenCowKingWasKilled = value;
                OnPropertyChanged();
            }
        }

        private bool _activeDoNotCloseNihlathakPortal;
        public bool ActiveDoNotCloseNihlathakPortal
        {
            get { return _activeDoNotCloseNihlathakPortal; }
            set
            {
                _ini.SetVal("EXTRA", "ActiveDoNotCloseNihlathakPortal", value);

                _activeDoNotCloseNihlathakPortal = value;
                OnPropertyChanged();
            }
        }
        #endregion

    }
}
