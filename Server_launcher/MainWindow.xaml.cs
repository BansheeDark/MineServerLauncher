using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;


namespace Server_launcher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {       
        public MainWindow()
        {
            InitializeComponent();
        }

        void OpenFileBat()
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = ".bat files (*.bat)|*.bat*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            Properties.Settings.Default.filepath = openFileDialog1.FileName;
                            Properties.Settings.Default.directoryName = Path.GetDirectoryName(Properties.Settings.Default.filepath);
                            tx1.Text = Properties.Settings.Default.filepath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void OpenFileGame()
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Exe files (*.exe)|*.exe*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            Properties.Settings.Default.gamepath = openFileDialog1.FileName;
                            tx3.Text = Properties.Settings.Default.gamepath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void OpenFileSettings()
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Properties files (*.properties)|*.properties*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            Properties.Settings.Default.settingspath = openFileDialog1.FileName;
                            tx4.Text = Properties.Settings.Default.settingspath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        void OpenDirectoryServer()
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new System.Windows.Forms.FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                try
                {                  
                    Properties.Settings.Default.directoryName = folderBrowser.SelectedPath;
                    tx2.Text = Properties.Settings.Default.directoryName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileBat();
            if (Properties.Settings.Default.filepath != "")
            {
                Properties.Settings.Default.directoryName = Path.GetDirectoryName(Properties.Settings.Default.filepath);
                tx2.Text = Properties.Settings.Default.directoryName;
                Properties.Settings.Default.Save();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {           
            if (Properties.Settings.Default.directoryName != "")
            {
                
                    Process.Start(new ProcessStartInfo("explorer.exe", Properties.Settings.Default.directoryName));
                
            }
                
            else                   
                MessageBox.Show("Чтобы открыть папку, необходимо задать её путь.");            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.settingspath != "")
            {
                tx4.Text = Properties.Settings.Default.settingspath;
                Process.Start(new ProcessStartInfo("notepad.exe", Properties.Settings.Default.settingspath));
            }
            else if (Properties.Settings.Default.directoryName != "")
            {
                tx4.Text = Properties.Settings.Default.directoryName + "\\server.properties";
                Process.Start(new ProcessStartInfo("notepad.exe", Properties.Settings.Default.directoryName + "\\server.properties"));
            }
            else
                MessageBox.Show("Чтобы открыть файл, необходимо задать его путь.");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.filepath != "")
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(Properties.Settings.Default.filepath);
                startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
                Process.Start(startInfo);
            }

            else
                MessageBox.Show("Чтобы запустить сервер, необходимо задать его путь.");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.gamepath != "")
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(Properties.Settings.Default.gamepath);
                startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
                Process.Start(startInfo);
            }

            else
                MessageBox.Show("Чтобы запустить игру, необходимо задать путь.");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            OpenFileBat();
        }

        private void tx1_Loaded(object sender, RoutedEventArgs e)
        {
            tx1.Text = Properties.Settings.Default.filepath;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            OpenDirectoryServer();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            OpenFileGame();
           
        }

        private void tx2_Loaded(object sender, RoutedEventArgs e)
        {
            tx2.Text = Properties.Settings.Default.directoryName;
        }

        private void tx3_Loaded(object sender, RoutedEventArgs e)
        {
            tx3.Text = Properties.Settings.Default.gamepath;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            OpenFileSettings();
        }

        private void tx4_Loaded(object sender, RoutedEventArgs e)
        {
            if(Properties.Settings.Default.settingspath !="")
            tx4.Text = Properties.Settings.Default.settingspath;
            else if(Properties.Settings.Default.directoryName!="")
            tx4.Text = Properties.Settings.Default.directoryName + "\\server.properties";
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
