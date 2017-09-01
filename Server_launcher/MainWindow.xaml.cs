using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
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

        private void Button_Click(object sender, RoutedEventArgs e)
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
                            MessageBox.Show("Путь .bat - файла: " + Properties.Settings.Default.filepath + "\nПапка сервера: "+ Properties.Settings.Default.directoryName, "Выбрано.");
                            Properties.Settings.Default.Save();
                        }
                    }
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
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
            if (Properties.Settings.Default.filepath != "")
            {

                Process.Start(new ProcessStartInfo("notepad.exe", Properties.Settings.Default.directoryName + "/server.properties"));

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
    }
}
