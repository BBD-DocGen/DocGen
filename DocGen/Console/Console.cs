using DocGen.Models;
using DocGen.Classes;

namespace CliProgram
{
    class CliConsole{
        private static bool signIn = false;

        public static async Task ConsoleAsync(){
            bool appRunning = true;
            string choice;

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
                                if (!string.IsNullOrEmpty(sourceCodeFilePath))
                                {
                                    var sourceCodeFile = Path.GetFileName(sourceCodeFilePath);
                                    try
                                    {
                                        var sourceCodeFileContent = File.ReadAllText(sourceCodeFilePath);
                                        Document inputDocument = new Document(sourceCodeFile,sourceCodeFileContent);
                                        Content response = await Provider.uploadDocuAndGetGeneratedDoc(inputDocument);

                                        Console.WriteLine();
                                        Console.WriteLine("Generated document content:");
                                        Console.WriteLine(response.content);

                                        Console.WriteLine("\nDocumentation generated based on source code file provided.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"The file could not be found at the given path.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("The file path is empty or null. Please provide a valid file path.");
                                }

                                InterfaceText.insertBoundaryText();
                                break;
                            // Option 2 to retrieve all past documents
                            case "2":
                                var allGeneratedDocs = await Provider.getGeneratedDocs();
                                foreach (var generatedDoc in allGeneratedDocs){
                                  Console.WriteLine("Generated Document ID: " + generatedDoc.GenDocID);
                                  Console.WriteLine("Generated Document Name: " + generatedDoc.GenDocName);
                                  Console.WriteLine("Generated Document URL: " + generatedDoc.GenDocURL + "\n");
                                }
                                InterfaceText.insertBoundaryText();
                                break;
                            // Option 3 to retrieve one past document
                            case "3":
                                var allUploadedDocs = await Provider.getUploadedDocs();
                                foreach (UploadedDocs uploadedDoc in allUploadedDocs){
                                  Console.WriteLine("Uploaded Document ID: " + uploadedDoc.UpDocID);
                                  Console.WriteLine("Uploaded Document Name: " + uploadedDoc.UpDocName);
                                  Console.WriteLine("Uploaded Document URL: " + uploadedDoc.UpDocURL + "\n");
                                }
                                InterfaceText.insertBoundaryText();
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
                            default:
                                InterfaceText.insertBoundaryText();
                                Console.WriteLine("Invalid option. Please choose again.");
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
                            if (signIn != true){
                                InterfaceText.insertBoundaryText();
                                Console.WriteLine("Signing in...");
                                var access_token = LogIn.Login();
                                if (access_token != ""){
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
                            InterfaceText.insertBoundaryText();
                            Console.WriteLine("Invalid option. Please choose again.");
                            break;
                    }
                }
            }

            Console.WriteLine("DocGen exited successfully");
        }
    }
}
