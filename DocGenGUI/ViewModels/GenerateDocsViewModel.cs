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
            GetFileViaDialog file = new GetFileViaDialog();

            FileContents = file.getFileContents();
        }
    }
}
