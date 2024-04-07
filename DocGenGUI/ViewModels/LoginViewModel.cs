using Auth0.OidcClient;
using DocGen.Classes;
using DocGen.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace DocGen.ViewModels
{
    internal class LoginViewModel : BaseViewModel
    {
        private Auth0Client client;
        public ICommand LoginCommand { get; }

        public LoginViewModel() 
        {
            LoginCommand = new RelayCommand((param) => ExecuteLogin());
            Auth0ClientOptions clientOptions = new Auth0ClientOptions
            {
                Domain = "dev-2f8sdpf6pls655l7.us.auth0.com",
                ClientId = "RvnPGiHVhtjPJ76vzfaIM0WmidvjRwl7",
            };
            client = new Auth0Client(clientOptions);
        }

        public async void ExecuteLogin()
        {
            var loginResult = await client.LoginAsync();
            MessageBox.Show(loginResult.AccessToken);
            if (loginResult.IsError == false)
            {
                MainWindow.Instance.Main.Content = new GenerateDocsPage();
            }

        }
    }
}
