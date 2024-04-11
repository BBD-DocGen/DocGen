using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.IO;
using System.Text;
class GenerateDocument{

    public static async void generateDocument(string access_token, string sourceCodeFile, string sourceCodeFileContent){
      HttpClient client = new HttpClient();
      var parameters = new Dictionary<string, string>
      {
          { "fileName", $"{sourceCodeFile}" },
          { "fileContent", $"{sourceCodeFileContent}"}
      };
      try
      {
          // Add custom headers to the request
          client.DefaultRequestHeaders.Add("Authorization", $"Bearer {access_token}");
          client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
          // Encode the parameters
          var content = new FormUrlEncodedContent(parameters);

          // Send a POST request with the parameters in the body
          HttpResponseMessage response = await client.PostAsync("localhost:5000/api/v1/uploaded-documents", content);

          // Check if the response is successful (status code 200)
          if (response.IsSuccessStatusCode)
          {
              // Read the response content as a string
              string responseBody = await response.Content.ReadAsStringAsync();

              // Print the response body
              Console.WriteLine(responseBody);
          }
          else
          {
              Console.WriteLine($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
          }
      }
      catch (HttpRequestException e)
      {
          Console.WriteLine($"HTTP Request Error: {e.Message}");
      }
    }


    public static async Task<string> retrieveAllDocuments(string access_token){
      HttpClient client = new HttpClient();
      var retrieveAllDocumentsUrl = "";
      try
        {
          client.DefaultRequestHeaders.Add("Authorization", $"Bearer {access_token}");
          client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

          HttpResponseMessage response = await client.GetAsync("localhost:5000/api/v1/uploaded-documents");

          if (response.IsSuccessStatusCode)
          {
              string responseBody = await response.Content.ReadAsStringAsync();
              Console.WriteLine(responseBody);
              return responseBody;
          }
          else
          {
              Console.WriteLine($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
              return $"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}";
          }
      }
      catch (HttpRequestException e)
      {
          Console.WriteLine($"HTTP Request Error: {e.Message}");
          return $"HTTP Request Error: {e.Message}";
      }
    }

    public static async Task<string> retrieveOneDocument(string access_token, string fileId){
    HttpClient client = new HttpClient();
    var retrieveOneDocumentUrl = "";
    var parameters = new Dictionary<string, string>
    {
        { "fileId", $"{fileId}" }
    };
    try
      {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {access_token}");
        client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

        HttpResponseMessage response = await client.GetAsync(retrieveOneDocumentUrl);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            return responseBody;
        }
        else
        {
            Console.WriteLine($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
            return $"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}";
        }
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine($"HTTP Request Error: {e.Message}");
        return $"HTTP Request Error: {e.Message}";
    }
    }


















    public static string extractFieldsAndMethods(string inputFile){
      // inputFile = "./InterfaceText/InterfaceText.cs";
      string output = "";

        string cSharpCode = File.ReadAllText(inputFile);

        SyntaxTree tree = CSharpSyntaxTree.ParseText(cSharpCode);
        CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

        var fields = root.DescendantNodes().OfType<FieldDeclarationSyntax>();
        foreach (var field in fields)
        {
            // Extract visibility
            string visibility = field.Modifiers.ToString();

            // Extract field type and names
            string type = field.Declaration.Type.ToString();
            var names = field.Declaration.Variables.Select(variable => variable.Identifier.ToString());
            string fieldNames = string.Join(", ", names);

            Console.WriteLine($"Field: {visibility} {type} {fieldNames}");
            output += $"Field: {visibility} {type} {fieldNames}\n";
        }

        // Extract methods
        var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
        foreach (var method in methods)
        {
            // Extract visibility
            string visibility = method.Modifiers.ToString();

            // Extract method return type, name, and parameters
            string returnType = method.ReturnType.ToString();
            string methodName = method.Identifier.ToString();
            string parameters = string.Join(", ", method.ParameterList.Parameters.Select(param => $"{param.Type} {param.Identifier}"));

            Console.WriteLine($"Method: {visibility} {returnType} {methodName}({parameters})");
            output += $"Method: {visibility} {returnType} {methodName}({parameters})\n";
        }
        return output;
    }



    public static string extractClassInformation(string inputFile){
      // Read the content of MyClass.cs
        var output ="";
        string myClassCode = File.ReadAllText("./GenerateDocument/sample.cs");

        // Create a syntax tree from the source code
        var syntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(myClassCode));

        // Extract class information
        var root = syntaxTree.GetRoot();
        var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();

        if (classDeclaration != null)
        {
            Console.WriteLine($"Class Name: {classDeclaration.Identifier}");
            Console.WriteLine("Properties:");
            foreach (var property in classDeclaration.Members.OfType<PropertyDeclarationSyntax>())
            {
                var propertyComment = property.GetLeadingTrivia().FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
                // Console.WriteLine(propertyComment);
                Console.WriteLine($"- {property.Identifier} ({propertyComment})");
                output += $"- {property.Identifier} ({propertyComment})";

                SyntaxTriviaList trivia = property.GetLeadingTrivia();
                Console.WriteLine("The property has the following trivia:");
                foreach (SyntaxTrivia t in trivia)
                {
                    if (!t.IsKind(SyntaxKind.WhitespaceTrivia) && !t.IsKind(SyntaxKind.EndOfLineTrivia))
                    {
                        Console.WriteLine(t);
                        output += t;
                    }
                }
            }

            Console.WriteLine("Methods:");
            foreach (var method in classDeclaration.Members.OfType<MethodDeclarationSyntax>())
            {
                var methodComment = method.GetLeadingTrivia().FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
                Console.WriteLine($"- {method.Identifier} ({methodComment})");
                output += $"- {method.Identifier} ({methodComment})";

                SyntaxTriviaList trivia = method.GetLeadingTrivia();
                Console.WriteLine("The method has the following trivia:");
                foreach (SyntaxTrivia t in trivia)
                {
                    if (!t.IsKind(SyntaxKind.WhitespaceTrivia) && !t.IsKind(SyntaxKind.EndOfLineTrivia))
                    {
                        Console.WriteLine(t);
                        output += t;
                    }
                }
            }
            return output;
        }
        else
        {
            Console.WriteLine("Class not found in the syntax tree.");
            return "Class not found in the syntax tree.";
        }
    }
}