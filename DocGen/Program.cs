using System;
using System.Diagnostics;
﻿using Auth0.OidcClient;
using RestSharp;
using Newtonsoft.Json;
using System.Net;

namespace  Program
{
  class Program{
    private static bool signIn = false;
    private static string id_token = "";

    static void Main(string[] args){
      bool appRunning = true;
      string choice = "";

      Console.WriteLine("********************************************************");
      Console.WriteLine("********************************************************");
      Console.WriteLine("************                               *************");
      Console.WriteLine("************  Welcome to DocGen Generator! *************");
      Console.WriteLine("************                               *************");
      Console.WriteLine("********************************************************");
      Console.WriteLine("********************************************************");
      Console.WriteLine();
      Console.WriteLine();
      Console.WriteLine("-----------------------------------------------------------------------");

      while (appRunning)
      {
        Console.WriteLine("Please enter the number to select the option!");

        // If signed in, then take to different options
        while (signIn && appRunning){
            Console.WriteLine("You are signed in!!");
            bool signInRunning = true;
            while (signInRunning){
              Console.WriteLine("1. Generate Document");
              Console.WriteLine("2. Sign out");
              Console.WriteLine("3. Exit");

              choice = Console.ReadLine();
              Console.WriteLine();

              string sourceCodeFilePath = "";
              string documentationFilePath = "";

              switch (choice){
                // Option 1 to generate document
                case "1":
                  Console.WriteLine();
                  Console.WriteLine();
                  Console.WriteLine("-----------------------------------------------------------------------");
                  Console.WriteLine("Please enter the path of your source code file");
                  sourceCodeFilePath = Console.ReadLine();

                  Console.WriteLine("Please enter the path of your output documentation file");
                  documentationFilePath = Console.ReadLine();

                  Console.WriteLine("Documentation generated based on source code file provided.");
                  Console.WriteLine();
                  Console.WriteLine();
                  Console.WriteLine("-----------------------------------------------------------------------");
                  break;
                // Option 2 to sign out
                case "2":
                  signInRunning = false;
                  Console.WriteLine();
                  Console.WriteLine();
                  Console.WriteLine("-----------------------------------------------------------------------");
                  break;
                // Option 3 to exit the application
                case "3":
                  Console.WriteLine("Exiting the application...");
                  signInRunning = false;
                  signIn=false;
                  appRunning = false;
                  break;
              }
            }
            signIn = false;
        }

        while (!signIn && appRunning){
          Console.WriteLine("1. Please enter 1 to sign in using your github account");
          Console.WriteLine("2. Exit");

          choice = Console.ReadLine();
          Console.WriteLine();

          switch (choice)
          {
              // Option 1 to sign in
              case "1":
                  // If not signed in, then take to OAuth sign process
                  if (signIn != true){
                      Console.WriteLine();
                      Console.WriteLine();
                      Console.WriteLine("-----------------------------------------------------------------------");
                      Console.WriteLine("Signing in...");
                      Login();
                      break;
                  }
                  break;
              // Option 2 to exit the application
              case "2":
                  Console.WriteLine("Exiting the application...");
                  appRunning = false;
                  signIn = true;
                  break;
              default:
                  Console.WriteLine("Invalid option. Please choose again.");
                  break;
          }
        }
      }

      Console.WriteLine("DocGen exited successfully");
    }

    public static void Login()
    {
      var client = new RestClient("https://dev-2f8sdpf6pls655l7.us.auth0.com/oauth/device/code");
      var request = new RestRequest();
      request.AddHeader("content-type", "application/x-www-form-urlencoded");
      request.AddParameter("application/x-www-form-urlencoded", "client_id=RvnPGiHVhtjPJ76vzfaIM0WmidvjRwl7&scope=openid", ParameterType.RequestBody);

      try
        {
            var response = client.ExecutePost(request);

            if (response != null)
            {
                if (response.StatusCode != HttpStatusCode.OK){
                  Console.WriteLine();
                  Console.WriteLine();
                  Console.WriteLine("-----------------------------------------------------------------------");
                  Console.WriteLine("Cannot make request to the Autho website, please check your network connection and try again.");
                }
                else{
                  var responseObject = JsonConvert.DeserializeObject<dynamic>(response.Content);
                  string auth0VerificationUri = responseObject.verification_uri;
                  var auth0VerificationUriInfo = new ProcessStartInfo {
                      FileName = auth0VerificationUri,
                      UseShellExecute = true
                  };
                  Process.Start(auth0VerificationUriInfo);

                  Console.WriteLine("Verification page has been opened on your default browser, or go to below link:");
                  Console.WriteLine($"{responseObject.verification_uri}");
                  Console.WriteLine($"Your One-time Code is: {responseObject.user_code}, please use this to link your device");
                  Console.WriteLine();
                  Console.WriteLine("1. Continue if you are successfully connected");
                  Console.WriteLine("2. Exit log in");
                  var oAuthOption = Console.ReadLine();
                  Console.WriteLine();

                  while (oAuthOption != "1"){
                    if (oAuthOption == "2"){
                      Console.WriteLine();
                      Console.WriteLine();
                      Console.WriteLine("-----------------------------------------------------------------------");
                      Console.WriteLine("You have exited the log in process.");
                      return;
                    }
                    Console.WriteLine("Verification page has been opened on your default browser, or go to below link:");
                    Console.WriteLine($"{responseObject.verification_uri}");
                    Console.WriteLine($"Your One-time Code is: {responseObject.user_code}");
                    Console.WriteLine("1. Continue if you are successfully connected");
                    Console.WriteLine("2. Exit log in");
                    oAuthOption = Console.ReadLine();
                  }

                  client = new RestClient("https://dev-2f8sdpf6pls655l7.us.auth0.com/oauth/token");
                  request = new RestRequest();
                  request.AddHeader("content-type", "application/x-www-form-urlencoded");
                  request.AddParameter("application/x-www-form-urlencoded", $"grant_type=urn%3Aietf%3Aparams%3Aoauth%3Agrant-type%3Adevice_code&device_code={responseObject.device_code}&client_id=RvnPGiHVhtjPJ76vzfaIM0WmidvjRwl7", ParameterType.RequestBody);
                  response = client.ExecutePost(request);

                  if (response.StatusCode != HttpStatusCode.OK){
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("-----------------------------------------------------------------------");
                    Console.WriteLine("Your device is not succesfully connected, please try again.");
                  }
                  else{
                    responseObject = JsonConvert.DeserializeObject<dynamic>(response.Content);
                    id_token = responseObject.id_token;
                    signIn = true;
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("-----------------------------------------------------------------------");
                  }
                }
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
    }
  }
}