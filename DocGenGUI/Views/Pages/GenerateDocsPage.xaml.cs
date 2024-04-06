using DocGen.ViewModels;
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

namespace DocGen.Views.Pages
{
    /// <summary>
    /// Interaction logic for GenerateDocsPage.xaml
    /// </summary>
    public partial class GenerateDocsPage : Page
    {
        GenerateDocsViewModel viewModel;

        public GenerateDocsPage()
        {
            InitializeComponent();
            this.viewModel = new GenerateDocsViewModel();
            this.DataContext = viewModel;
        }
    }
}
