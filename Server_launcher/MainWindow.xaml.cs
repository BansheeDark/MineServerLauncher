using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Server_launcher.Properties;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Server_launcher
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MineLaunch.Title = MineLaunch.Title +" v."+ System.Reflection.Assembly.GetExecutingAssembly().GetName().Version+"(Alpha)";
            App.LanguageChanged += LanguageChanged;
            CultureInfo currLang = App.Language;
            StartGame(false);
            //Заполняем меню смены языка:
            MenuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {

                ComboBoxItem menuBoxItem = new ComboBoxItem();
                menuBoxItem.Tag = lang;
                menuBoxItem.Content = lang.DisplayName;
                menuBoxItem.Foreground = Brushes.Black;
                menuBoxItem.Selected += ChangeLanguageClick;
                menuBoxItem.IsSelected = lang.Equals(currLang);
                MenuLanguage.Items.Add(menuBoxItem);
            }
        }

        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;

            //Отмечаем нужный пункт смены языка как выбранный язык
            foreach (ComboBoxItem i in MenuLanguage.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;
                i.IsSelected = ci != null && ci.Equals(currLang);
            }
        }

        private void ChangeLanguageClick(Object sender, EventArgs e)
        {
            if (sender is ComboBoxItem mi)
            {
                CultureInfo lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
            }

        }

        private void OpenFileBat()
        {
            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = ".bat files (*.bat)|*.bat*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == true)
                try
                {
                    Settings.Default.filepath = openFileDialog1.FileName;
                    Settings.Default.directoryName = Path.GetDirectoryName(Settings.Default.filepath);
                    tx1.Text = Settings.Default.filepath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileBat();
            if (Settings.Default.filepath != "")
            {
                Settings.Default.directoryName = Path.GetDirectoryName(Settings.Default.filepath);
                tx2.Text = Settings.Default.directoryName;
                Settings.Default.Save();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.directoryName != "")
                Process.Start(new ProcessStartInfo("explorer.exe", Settings.Default.directoryName));

            else
            {
                MessageBox.Show("Чтобы открыть папку, необходимо задать её путь.");
                OpenDirectoryServer();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.settingspath != "")
            {
                tx4.Text = Settings.Default.settingspath;
                Process.Start(new ProcessStartInfo("notepad.exe", Settings.Default.settingspath));
            }
            else if (Settings.Default.directoryName != "")
            {
                tx4.Text = Settings.Default.directoryName + "\\server.properties";
                Process.Start(new ProcessStartInfo("notepad.exe",
                    Settings.Default.directoryName + "\\server.properties"));
            }
            else
            {
                MessageBox.Show("Чтобы открыть файл, необходимо задать его путь.");
                OpenFileSettings();
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.filepath != "")
            {
                var startInfo = new ProcessStartInfo(Settings.Default.filepath);
                startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
                Process.Start(startInfo);
            }

            else
            {
                MessageBox.Show("Чтобы запустить сервер, необходимо задать его путь.");
                OpenFileBat();
            }
        }

        private void ProcessStartGame()
        {
            if (Settings.Default.gamepath != "")
            {
                var startInfo = new ProcessStartInfo(Settings.Default.gamepath);

                    startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
                    tx3.Text = Settings.Default.gamepath;
                    Process.Start(startInfo);       
            }
            else
            {
                MessageBox.Show("Файл запуска игры не найден. Для запуска игры, необходимо указать путь в настройках.");
                OpenFileGame();
            }
        }

        private void StartGame(bool check)
        {
            var gamepathdef1 = @"%AppData%\Roaming\.minecraft\launcher.jar";
            var gamepathdef2 = @"C:\Program Files (x86)\Minecraft\MinecraftLauncher.exe";
            if (File.Exists(gamepathdef1))
            {
                Settings.Default.gamepath = gamepathdef1;
                if(check)
                ProcessStartGame();
            }
            else
            {
                if (File.Exists(gamepathdef2))
                {
                    Settings.Default.gamepath = gamepathdef2;
                    if (check)
                        ProcessStartGame();
                }
                else
                {
                    if (check)
                        ProcessStartGame();
                }
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            StartGame(true);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            OpenFileBat();
        }

        private void tx1_Loaded(object sender, RoutedEventArgs e)
        {
            tx1.Text = Settings.Default.filepath;
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
        }



        #region DirectoryServer

        private void OpenDirectoryServer()
        {
            var folderBrowser = new System.Windows.Forms.FolderBrowserDialog();

            var result = folderBrowser.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
                { 
                if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                    try
                    {
                        Settings.Default.directoryName = folderBrowser.SelectedPath;
                        tx2.Text = Settings.Default.directoryName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            OpenDirectoryServer();
        }

        private void tx2_Loaded(object sender, RoutedEventArgs e)
        {
            tx2.Text = Settings.Default.directoryName;
        }

        #endregion

        #region FileGame

        private void OpenFileGame()
        {
            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "Exe files (*.exe)|*.exe*",
                FilterIndex = 2,
                RestoreDirectory = true
            };      
            if (openFileDialog1.ShowDialog() == true)
                try
                {                  
                    Settings.Default.gamepath = openFileDialog1.FileName;
                    tx3.Text = Settings.Default.gamepath;                      
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
        }
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            OpenFileGame();
        }
        private void tx3_Loaded(object sender, RoutedEventArgs e)
        {
            tx3.Text = Settings.Default.gamepath;
        }
        private void tx3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.Default.gamepath = tx3.Text;
        }

        #endregion

        #region Settings

        private void OpenFileSettings()
        {
            Stream myStream = null;
            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "Properties files (*.properties)|*.properties*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == true)
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                        using (myStream)
                        {
                            Settings.Default.settingspath = openFileDialog1.FileName;
                            tx4.Text = Settings.Default.settingspath;
                        }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            OpenFileSettings();
        }

        private void tx4_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.settingspath != "")
                tx4.Text = Settings.Default.settingspath;
            else if (Settings.Default.directoryName != "")
                tx4.Text = Settings.Default.directoryName + "\\server.properties";
        }

        #endregion

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;                            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Information: "+ex,"Eror", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            
        }
    }
}