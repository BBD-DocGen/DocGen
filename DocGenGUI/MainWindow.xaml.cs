using DocGen.Views.Pages;
using System;
using System.IO;
using System.Windows;

namespace DocGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            Main.Content = new LoginPage();
        }
    }
}
