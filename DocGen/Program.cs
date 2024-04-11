using IdentityModel.OidcClient;

using DocGen.Models;
using DocGen.Classes;

namespace  Program
{
  class Program{
    private static bool signIn = false;

    static void Main(string[] args){
        MainAsync().Wait();
    }

    static async Task MainAsync(){
      bool appRunning = true;
      string choice;
      string access_token ="";

      InterfaceText.insertWelcomeText();
      InterfaceText.insertBoundaryText();

      while (appRunning)
      {
        Console.WriteLine("Please enter the number to select the option!");

        // If signed in, then take to different options
        while (signIn && appRunning){
            Console.WriteLine("You are signed in!!");
            bool signInRunning = true;
            while (signInRunning){
              InterfaceText.insertSignInOptions();

              choice = Console.ReadLine();
              Console.WriteLine();

              switch (choice){
                // Option 1 to generate document
                case "1":
                  InterfaceText.insertBoundaryText();
                  Console.WriteLine("Please enter the full path of your source code file");

                  var sourceCodeFilePath = Console.ReadLine();
                  var sourceCodeFile = Path.GetFileName(sourceCodeFilePath);
                  var sourceCodeFileContent = File.ReadAllText(sourceCodeFilePath);

                  Console.WriteLine("sourceCodeFilePath " + sourceCodeFilePath);
                  Console.WriteLine("sourceCodeFile " + sourceCodeFile);
                  Console.WriteLine("sourceCodeFileContent " + sourceCodeFileContent);

                  Document inputDocument = new Document(sourceCodeFile,sourceCodeFileContent);

                  Content respone = await Provider.uploadDocuAndGetGeneratedDoc(inputDocument);

                  Console.WriteLine(respone.content);

                  Console.WriteLine("Documentation generated based on source code file provided.");
                  InterfaceText.insertBoundaryText();
                  break;
                // Option 2 to retrieve all past documents
                case "2":
                    Console.WriteLine("Printing all your past generated documents...");
                    var allGeneratedDocs = await Provider.getGeneratedDocs();
                    // Console.WriteLine(await Provider.getAllGeneratedDocs());
                    foreach (var generatedDoc in allGeneratedDocs){
                      Console.WriteLine(generatedDoc);
                      Console.WriteLine(generatedDoc.GenDocID);
                      Console.WriteLine(generatedDoc.GenDocName);
                      Console.WriteLine(generatedDoc.GenDocURL);
                    }
                  break;
                // Option 3 to retrieve one past document
                case "3":
                    Console.WriteLine("Printing all your past uploaded documents...");
                    var allUploadedDocs = await Provider.getUploadedDocs();
                    foreach (GeneratedDocs uploadedDoc in allUploadedDocs){
                      Console.WriteLine(uploadedDoc);
                      Console.WriteLine(uploadedDoc.GenDocID);
                      Console.WriteLine(uploadedDoc.GenDocName);
                      Console.WriteLine(uploadedDoc.GenDocURL);
                    }
                  break;
                // Option 4 to sign out
                case "4":
                  signInRunning = false;
                  InterfaceText.insertBoundaryText();
                  break;
                // Option 5 to exit the application
                case "5":
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
          InterfaceText.insertUnsignInOptions();

          choice = Console.ReadLine();
          Console.WriteLine();

          switch (choice)
          {
              // Option 1 to sign in
              case "1":
                  // If not signed in, then take to OAuth sign process
                  if (signIn != true){
                      InterfaceText.insertBoundaryText();
                      Console.WriteLine("Signing in...");
                      access_token = LogIn.Login();
                      if (access_token != ""){
                        Console.WriteLine("access_token: " + access_token);
                        var loginCheck = await LogIn.LogInUserCheck(access_token);
                        if (loginCheck){
                            signIn = true;
                        }
                      }
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
  }
}
