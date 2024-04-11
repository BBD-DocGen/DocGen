using DocGen.Classes;
using DocGen.Views.Pages;
using DocGen.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Amazon.S3.Model;

namespace DocGen.ViewModels
{
    internal class ViewDocumentsViewModel: BaseViewModel
    {
        private ObservableCollection<GeneratedDocs> _generatedDocs;

        public ObservableCollection<GeneratedDocs> GeneratedDocs 
        {
            get => _generatedDocs;
            set
            {
                _generatedDocs = value;
                OnPropertyChanged();
            }
        }

        public ICommand BackCommand { get; }
        public ICommand DownloadCommand { get; }

        public ViewDocumentsViewModel() 
        {
            BackCommand = new RelayCommand((param) => ExecuteBack());
            DownloadCommand = new RelayCommand((param) => ExecuteDownload());
            GetData();
        }

        private void ExecuteDownload()
        {
            MessageBox.Show("Lol");
        }

        private void ExecuteBack()
        {
            MainWindow.Instance.Main.Content = new GenerateDocsPage();
        }

        public async void GetData()
        {
            GeneratedDocs = await Provider.getGeneratedDocs();
        }
    }
}
