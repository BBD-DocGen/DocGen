using DocGen.Classes;
using DocGen.Models;
using DocGen.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace DocGen.Views.Pages
{
    /// <summary>
    /// Interaction logic for ViewDocumentsPage.xaml
    /// </summary>
    public partial class ViewDocumentsPage : Page
    {
        ViewDocumentsViewModel viewModel;
        public ViewDocumentsPage()
        {
            InitializeComponent();
            this.viewModel = new ViewDocumentsViewModel();
            this.DataContext = viewModel;
        }

        public async void btn_view(object sender, RoutedEventArgs e)
        {
            GeneratedDocs data = (GeneratedDocs)((Button)e.Source).DataContext;
            string name = data.GenDocName;
            string url = data.GenDocURL;

            string fileContent = await Provider.downloadFile(url);

            MessageBox.Show(fileContent, name);
        }

        public async void btn_download(object sender, RoutedEventArgs e)
        {
            GeneratedDocs data = (GeneratedDocs)((Button)e.Source).DataContext;
            string name = data.GenDocName;
            string url = data.GenDocURL;

            string fileContent = await Provider.downloadFile(url);

            string downloadsPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";

            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(downloadsPath, $"{name}")))
            {
                outputFile.WriteLine(fileContent);
            }

            MessageBox.Show("File has downloaded");
        }
    }
}
