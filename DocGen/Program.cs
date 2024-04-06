using System;
using System.Diagnostics;
﻿using Auth0.OidcClient;
using RestSharp;
using Newtonsoft.Json;

namespace  Program
{
  class Program{
    private static Auth0Client client;

    static void Main(string[] args){
      // string target= "http://www.microsoft.com";
      // var info = new ProcessStartInfo {
      //     FileName = target,
      //     UseShellExecute = true
      // };
      // Process.Start(info);

      // Auth0ClientOptions clientOptions = new Auth0ClientOptions
      // {
      //     Domain = "dev-2f8sdpf6pls655l7.us.auth0.com",
      //     ClientId = "RvnPGiHVhtjPJ76vzfaIM0WmidvjRwl7",
      // };
      // client = new Auth0Client(clientOptions);

      // ExecuteLogin();


      var client = new RestClient("https://dev-2f8sdpf6pls655l7.us.auth0.com/oauth/device/code");
      var request = new RestRequest();
      request.AddHeader("content-type", "application/x-www-form-urlencoded");
      // request.AddParameter("application/json", "{\"client_id\":\"RvnPGiHVhtjPJ76vzfaIM0WmidvjRwl7\",\"client_secret\":\"XhZaHKW6ecG5MIDU9RIkx96V5fKXWVn9YTUuIME4F7XTNMpsJLEdn1CHXX6CQLmg\",\"audience\":\"https://docgen.com\",\"grant_type\":\"authorization_code\"}", ParameterType.RequestBody);
      request.AddParameter("application/x-www-form-urlencoded", "client_id=RvnPGiHVhtjPJ76vzfaIM0WmidvjRwl7&scope=openid", ParameterType.RequestBody);

      try
        {
            var response = client.ExecutePost(request);

            if (response != null)
            {
                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Content: {response.Content}");

                Console.WriteLine(response.Content);

                var responseObject = JsonConvert.DeserializeObject<dynamic>(response.Content);
                string target= responseObject.verification_uri;
                var info = new ProcessStartInfo {
                    FileName = target,
                    UseShellExecute = true
                };
                Process.Start(info);

                Console.WriteLine($"User Code: {responseObject.user_code}");
                Console.WriteLine("Enter 1 when you are successfully connected");
                var oAuthOption = Console.ReadLine();

                while (oAuthOption != "1"){
                  Console.WriteLine($"Your User Code is: {responseObject.user_code}");
                  Console.WriteLine("Enter 1 when you are successfully connected");
                  oAuthOption = Console.ReadLine();
                }
                  Console.WriteLine($"Device Code: {responseObject.device_code}");
                  Console.WriteLine($"Verification URI: {responseObject.verification_uri}");

                  client = new RestClient("https://dev-2f8sdpf6pls655l7.us.auth0.com/oauth/token");
                  request = new RestRequest();
                  request.AddHeader("content-type", "application/x-www-form-urlencoded");
                  request.AddParameter("application/x-www-form-urlencoded", $"grant_type=urn%3Aietf%3Aparams%3Aoauth%3Agrant-type%3Adevice_code&device_code={responseObject.device_code}&client_id=RvnPGiHVhtjPJ76vzfaIM0WmidvjRwl7", ParameterType.RequestBody);
                  response = client.ExecutePost(request);

                  Console.WriteLine($"Response Status: {response.StatusCode}");
                  Console.WriteLine($"Response Content: {response.Content}");

                  responseObject = JsonConvert.DeserializeObject<dynamic>(response.Content);

                  Console.WriteLine($"ID Token: {responseObject.id_token}");
            }
            else
            {
                Console.WriteLine("No response received.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

      // Console.WriteLine(response.Content);

      // var request = new RestRequest("products");
      //   request.AddBody(new
      //   {
      //       name = "RestSharp POST Request Example"
      //   });
      //   var response = client.ExecutePost(request);




      // bool appRunning = true;
      // bool signIn = false;

      // Console.WriteLine("********************************************************");
      // Console.WriteLine("********************************************************");
      // Console.WriteLine("************                               *************");
      // Console.WriteLine("************  Welcome to DocGen Generator! *************");
      // Console.WriteLine("************                               *************");
      // Console.WriteLine("********************************************************");
      // Console.WriteLine("********************************************************");
      // Console.WriteLine();
      // Console.WriteLine();
      // Console.WriteLine("-----------------------------------------------------------------------");

      // while (appRunning)
      // {
      //   Console.WriteLine("Please enter the number to select the option!");
      //   Console.WriteLine("1. Please enter 1 to sign in using your github account");
      //   Console.WriteLine("2. Exit");

      //   string choice = Console.ReadLine();

      //   switch (choice)
      //   {
      //       // Option 1 to sign in
      //       case "1":
      //           // If not signed in, then take to OAuth sign process
      //           if (signIn != true){
      //               Console.WriteLine("Signing in...");
      //               signIn = true;
      //               break;
      //           }

      //           // If signed in, then take to next step
      //           while (signIn){
      //               Console.WriteLine();
      //               Console.WriteLine();
      //               Console.WriteLine("-----------------------------------------------------------------------");
      //               Console.WriteLine("You are already signed in");
      //               bool signInRunning = true;
      //               while (signInRunning){
      //                 Console.WriteLine("1. Generate Document");
      //                 Console.WriteLine("2. Sign out");
      //                 Console.WriteLine("3. Exit");

      //                 choice = Console.ReadLine();
      //                 string sourceCodeFilePath = "";
      //                 string documentationFilePath = "";

      //                 switch (choice){
      //                   // Option 1 to generate document
      //                   case "1":
      //                     Console.WriteLine();
      //                     Console.WriteLine();
      //                     Console.WriteLine("-----------------------------------------------------------------------");
      //                     Console.WriteLine("Please enter the path of your source code file");
      //                     sourceCodeFilePath = Console.ReadLine();

      //                     Console.WriteLine("Please enter the path of your output documentation file");
      //                     documentationFilePath = Console.ReadLine();

      //                     Console.WriteLine("Documentation generated based on source code file provided.");
      //                     break;
      //                   // Option 2 to sign out
      //                   case "2":
      //                     signInRunning = false;
      //                     break;
      //                   // Option 3 to exit the application
      //                   case "3":
      //                     Console.WriteLine("Exiting the application...");
      //                     signInRunning = false;
      //                     appRunning = false;
      //                     break;
      //                 }

      //                   Console.WriteLine();
      //                   Console.WriteLine();
      //                   Console.WriteLine("-----------------------------------------------------------------------");
      //               }
      //               signIn = false;
      //           }
      //           break;
      //       // Option 2 to exit the application
      //       case "2":
      //           Console.WriteLine("Exiting the application...");
      //           appRunning = false;
      //           break;
      //       default:
      //           Console.WriteLine("Invalid option. Please choose again.");
      //           break;
      //   }
      //   Console.WriteLine();
      //   Console.WriteLine();
      //   Console.WriteLine("-----------------------------------------------------------------------");
      // }

      Console.WriteLine("DocGen exited successfully");
    }

    public static async void ExecuteLogin()
    {
      var loginResult = await client.LoginAsync();
    }
  }
}