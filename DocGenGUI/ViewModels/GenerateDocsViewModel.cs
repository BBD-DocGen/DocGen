using DocGen.Classes;
using DocGen.Models;
using OpenAI_API.Models;
using System.Windows;
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

        public string FileSummary
        {
            get => _fileContents.FileSummary;
            set
            {
                _fileContents.FileSummary = value;
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
            if (FileContents == null) 
            {
                MessageBox.Show("Please select a file");
                return;
            }

            await S3.uploadFile(
                FileContents.FileName == null ? "NO_NAME" : FileContents.FileName,
                FileContents.FileSummary == null ? "Nothing to summarize" : FileContents.FileSummary
            );
        }

        public async void ExecuteSelectFile()
        {
            GetFileViaDialog file = new GetFileViaDialog();
            FileContents = file.getFileContents();

            if (FileContents != null)
            {
                Content content = await Provider.uploadDocuAndGetGeneratedDoc(new Document
                {
                    fileName = FileContents.FileName, 
                    content = FileContents.FileContents
                });

                FileSummary = content.content ?? "";
            }
        }
    }
}
