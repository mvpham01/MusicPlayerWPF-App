using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WpfAppNagivator
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            LoadFileLink();
        }

        private void LoadFileLink()
        {
            string filePath = @"C:\Users\84338\AppData\Local\PvmMussicApp\config.txt";
            string LinkFile = File.ReadAllText(filePath);
            LinkFileText.Text = LinkFile;
        }
        private void ChangeLinkFile(object sender, RoutedEventArgs e)
        {
            using (var folderDialog = new CommonOpenFileDialog())
            {
                folderDialog.IsFolderPicker = true; 

                if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string selectedFolder = folderDialog.FileName;
                    String LinkFile = selectedFolder;
                    string configFilePath = @"C:\Users\84338\AppData\Local\PvmMussicApp\config.txt";
                    File.WriteAllText(configFilePath, string.Empty);
                    string newContent = LinkFile;
                    File.WriteAllText(configFilePath, newContent, Encoding.UTF8);
                }
            }
        }
    }
}
