class InterfaceText{
      public static void insertWelcomeText(){
      string welcomeText = "********************************************************\n";
      welcomeText += "********************************************************\n";
      welcomeText += "************                               *************\n";
      welcomeText += "************  Welcome to DocGen Generator! *************\n";
      welcomeText += "************                               *************\n";
      welcomeText += "********************************************************\n";
      welcomeText += "********************************************************";
      Console.WriteLine(welcomeText);
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