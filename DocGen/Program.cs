using System;

namespace  Program
{
  class Program{
    static void Main(string[] args){
      bool appRunning = true;
      bool signIn = false;

      Console.WriteLine("********************************************************");
      Console.WriteLine("********************************************************");
      Console.WriteLine("Welcome to DocGen Generator!");
      Console.WriteLine("********************************************************");
      Console.WriteLine("********************************************************");
      Console.WriteLine();
      Console.WriteLine();
      Console.WriteLine("-----------------------------------------------------------------------");

      while (appRunning)
      {
        Console.WriteLine("Please enter the number to select the option!");
        Console.WriteLine("1. Please enter 1 to sign in using your github account");
        Console.WriteLine("2. Exit");

        string choice = Console.ReadLine();

        switch (choice)
        {
            // Option 1 to sign in
            case "1":
                // If not signed in, then take to OAuth sign process
                if (signIn != true){
                    Console.WriteLine("Signing in...");
                    signIn = true;
                    break;
                }

                // If signed in, then take to next step
                while (signIn){
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("-----------------------------------------------------------------------");
                    Console.WriteLine("You are already signed in");
                    bool signInRunning = true;
                    while (signInRunning){
                      Console.WriteLine("1. Generate Document");
                      Console.WriteLine("2. Sign out");
                      Console.WriteLine("3. Exit");

                      choice = Console.ReadLine();
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
                          break;
                        // Option 2 to sign out
                        case "2":
                          signInRunning = false;
                          break;
                        // Option 3 to exit the application
                        case "3":
                          Console.WriteLine("Exiting the application...");
                          signInRunning = false;
                          appRunning = false;
                          break;
                      }

                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("-----------------------------------------------------------------------");
                    }
                    signIn = false;
                }
                break;
            // Option 2 to exit the application
            case "2":
                Console.WriteLine("Exiting the application...");
                appRunning = false;
                break;
            default:
                Console.WriteLine("Invalid option. Please choose again.");
                break;
        }
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("-----------------------------------------------------------------------");
      }

      Console.WriteLine("DocGen exited successfully");
    }
  }
}