namespace  Program
{
  class Program{
    private static bool signIn = false;

    static void Main(string[] args){
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

              string sourceCodeFilePath = "";
              string documentationFilePath = "";

              switch (choice){
                // Option 1 to generate document
                case "1":
                  InterfaceText.insertBoundaryText();
                  Console.WriteLine("Please enter the path of your source code file");
                  sourceCodeFilePath = Console.ReadLine();

                  Console.WriteLine("Please enter the path of your output documentation file");
                  documentationFilePath = Console.ReadLine();

                  Console.WriteLine("Documentation generated based on source code file provided.");
                  InterfaceText.insertBoundaryText();
                  break;
                // Option 2 to sign out
                case "2":
                  signInRunning = false;
                  InterfaceText.insertBoundaryText();
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
                      string id_token = LogIn.Login();
                      if (id_token != ""){
                        signIn = true;
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