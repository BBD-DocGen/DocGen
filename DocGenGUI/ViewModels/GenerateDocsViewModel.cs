using DocGen.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DocGen.ViewModels
{
    internal class GenerateDocsViewModel : BaseViewModel
    {
        private string _fileContents;
        public string FileContents
        {
            get => _fileContents;
            set
            {
                _fileContents = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectFileCommand{ get; }

        public GenerateDocsViewModel()
        {
            FileContents = "Docs Lol";

            SelectFileCommand = new RelayCommand((param) => ExecuteSelectFile());
        }

        public void ExecuteSelectFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "cs files (*.cs)|*.cs|All files (*.*)|*.*";
            fileDialog.ShowDialog();

            //Read the contents of the file into a stream
            Stream fileStream = fileDialog.OpenFile();

            using (StreamReader reader = new StreamReader(fileStream))
            {
                StringBuilder sb = new StringBuilder();
                String line = null;
                String pattern = "^\\s*(public|private)*\\s*(static)*\\s*(\\w+ \\w+)\\s?\\(.*\\).*$";
                while ((line = reader.ReadLine()) != null) 
                {
                    if (!Regex.Match(line, pattern).Success) continue;

                    sb.Append(line.Trim());
                    sb.Append('\n');
                }
                FileContents = sb.ToString();
            }
        }

        public async Task<string> generateDocs(string methodHeader)
        {
            StringBuilder sb = new StringBuilder();



            return sb.ToString();
        }
    }
}
