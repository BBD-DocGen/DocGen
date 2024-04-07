using DocGen.Classes;
using DocGen.Models;
using System.Windows.Input;

namespace DocGen.ViewModels
{
    internal class GenerateDocsViewModel : BaseViewModel
    {
        private FileModel _fileContents;
        public FileModel FileContents
        {
            get => _fileContents;
            set
            {
                _fileContents = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectFileCommand{ get; }
        public ICommand SaveFileCommand { get; }


        public GenerateDocsViewModel()
        {
            SelectFileCommand = new RelayCommand((param) => ExecuteSelectFile());
            SaveFileCommand = new RelayCommand((param) => ExecuteSaveFile());
        }

        public async void ExecuteSaveFile()
        {
            await S3.uploadFile("AnotherTestFile", @"C:\Users\bbdnet2862\Desktop\List.txt");
        }

        public void ExecuteSelectFile()
        {
            GetFileViaDialog file = new GetFileViaDialog();
            FileContents = file.getFileContents();

            if (FileContents != null)
            {
                // ChatGPT the file
            }
        }
    }
}
