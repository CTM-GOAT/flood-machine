using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FloodMachine
{
    public partial class MainWindow : Window
    {
        public string FolderPath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "My Title";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = "C:\\";

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = "C:\\";
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FolderPath = dlg.FileName;
                BtnChooseFolder.Content = FolderPath;
                BtnStartFlood.IsEnabled = true;
            }
        }

        private void BtnStartFlood_Click(object sender, RoutedEventArgs e)
        {
            int NumberOfFilesToCreate = Convert.ToInt32(FilesCount.Text);
            int NumberOfFilesToCreate2 = NumberOfFilesToCreate;
            string WD = BtnChooseFolder.Content.ToString();

            int Threads = Environment.ProcessorCount - 1;

            if (Threads < 1)
            { // prevent single-core computers from not being able to run this
                Threads = 1;
            }

            ParallelOptions parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Threads
            };

            try
            {
                Parallel.For(0, NumberOfFilesToCreate, xd => {
                    CreateFile(WD);
                    NumberOfFilesToCreate--;
                });
                MessageBox.Show("Created "+ NumberOfFilesToCreate2.ToString() + " files.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void CreateFile(string WD)
        {
            try
            {
                using (StreamWriter sw = File.CreateText(WD + "\\" + Guid.NewGuid().ToString()))
                {
                    int LineCount = 0;
                    while (LineCount < 10000)
                    {
                        sw.WriteLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
                        LineCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }

        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);

        }
    }
}
