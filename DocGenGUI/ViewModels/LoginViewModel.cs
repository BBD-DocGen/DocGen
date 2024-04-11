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
        private bool _loading;
        private string _loginBtnText;

        public bool Loading 
        {
            get => _loading;
            set 
            {
                _loading = value;
                OnPropertyChanged();
            } 
        }

        public string LogInBtnText 
        {
            get => _loginBtnText;
            set
            {
                _loginBtnText = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel() 
        {
            Loading = true;
            LogInBtnText = "Login";

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
            Loading = false;
            LogInBtnText = "Loading...........";

            var audience = "https://docgen.com";
            var extraParameters = new Dictionary<string, string>();
            extraParameters.Add("audience", audience);

            var loginResult = await client.LoginAsync(extraParameters: extraParameters);


            if (loginResult.IsError)
            {
                MessageBox.Show("Could not log in");
                Loading = true;
                LogInBtnText = "Login";
                return;
            }

            Provider.setHeader(loginResult.AccessToken);

            bool isloggedin = await Provider.login();

            if (!isloggedin)
            {
                MessageBox.Show("Could not log in");
                Loading = true;
                LogInBtnText = "Login";
                return;
            }

            MainWindow.Instance.Main.Content = new GenerateDocsPage();
        }
    }
}
