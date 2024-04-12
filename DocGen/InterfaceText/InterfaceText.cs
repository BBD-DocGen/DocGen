
namespace CliProgram
{
    class InterfaceText{
        public static void insertWelcomeText(){
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"*********************************************");
            Console.WriteLine(@"*********************************************");
            Console.WriteLine(@"**   _____              _____              **");
            Console.WriteLine(@"**  |  __ \            / ____|             **");
            Console.WriteLine(@"**  | |  | | ___   ___| |  __  ___ _ __    **");
            Console.WriteLine(@"**  | |  | |/ _ \ / __| | |_ |/ _ \ '_ \   **");
            Console.WriteLine(@"**  | |__| | (_) | (__| |__| |  __/ | | |  **");
            Console.WriteLine(@"**  |_____/ \___/ \___|\_____|\___|_| |_|  **");
            Console.WriteLine(@"**                                         **");
            Console.WriteLine(@"*********************************************");
            Console.WriteLine(@"*********************************************");
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void insertBoundaryText(){
            string boundaryText = "\n\n-----------------------------------------------------------------------";
            Console.WriteLine(boundaryText);
        }

        public static void insertUnsignInOptions(){
            string unsignInOptions = "1. Please enter 1 to sign in\n";
            unsignInOptions += "2. Exit";
            Console.WriteLine(unsignInOptions);
        }

        public static void insertSignInOptions(){
            string signInOptions = "1. Generate Document\n";
            signInOptions += "2. Check your past generated documents\n";
            signInOptions += "3. Check your past uploaded documents\n";
            signInOptions += "4. Sign out\n";
            signInOptions += "5. Exit";
            Console.WriteLine(signInOptions);
        }
    }
}