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
            SetEnvironmentVariable();
            InitializeComponent();
            Instance = this;
            Main.Content = new LoginPage();
        }

        private void SetEnvironmentVariable()
        {
            try
            {
                string path = @"..\..\..\env";

                foreach (string file in File.ReadAllLines(path))
                {
                    string [] keyValuePair = file.Split('=');
                    Environment.SetEnvironmentVariable(keyValuePair[0], keyValuePair[1]);
                }
            } 
            catch (FileNotFoundException ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }
    }
}
