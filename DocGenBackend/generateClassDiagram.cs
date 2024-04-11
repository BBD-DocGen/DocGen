using System;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
// using PlantUml.Net.Generators;
using PlantUml.Net;

class generateClassDiagram
{
    static async Task Main(string[] args)
    {
      await generateDiagram();
    }

    // public static async Task generateDiagram(){
    //     // Path to the single C# source file
    //     string inputFile = "./InterfaceText/InterfaceText.cs";

    //     // Output directory
    //     string outputDir = "./InterfaceText";

    //     Console.WriteLine($"Generating PlantUML text for {inputFile}...");

    //     // Output file path
    //     string outputFile = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputFile));

    //     string cSharpCode;
    //     using (var reader = new StreamReader(inputFile))
    //     {
    //         cSharpCode = await reader.ReadToEndAsync();
    //     }

    //     // Add @startuml and @enduml
    //     string plantUmlText = ConvertCSharpToPlantUml(cSharpCode);

    //     Console.WriteLine(plantUmlText);

    //     // Parse the C# source file
    //     using (var stream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
    //     {
    //         var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream));
    //         var root = tree.GetRoot();

    //         // Analyze the syntax tree to extract class information
    //         // Here you would need to traverse the syntax tree and extract class information,
    //         // then generate PlantUML text based on that information.

    //         // Save the PlantUML text to a file
    //         File.WriteAllText(outputFile + ".ClassDiagram.plantuml", plantUmlText);

    //         var factory = new RendererFactory();
    //         var renderer = factory.CreateRenderer(new PlantUmlSettings());

    //         try
    //         {
    //             // Render PlantUML text to PNG
    //             var bytes = await renderer.RenderAsync(plantUmlText, OutputFormat.Png);
    //             File.WriteAllBytes("out.png", bytes);
    //             Console.WriteLine("Class diagram saved as PNG: out.png");
    //             // return "Class diagram saved as PNG: out.png";
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine($"Error rendering PNG: {ex.Message}");
    //         }
    //     }

    //     Console.WriteLine($"PlantUML text generated and saved as {outputFile}.ClassDiagram.plantuml");
    // }


    // static string ConvertCSharpToPlantUml(string csharpCode)
    // {
    //     StringBuilder plantUmlText = new StringBuilder();

    //     // Add PlantUML directives
    //     plantUmlText.AppendLine("@startuml");

    //     // Split the C# code into lines
    //     string[] lines = csharpCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

    //     // Process each line of C# code
    //     foreach (string line in lines)
    //     {
    //         // Remove leading and trailing whitespaces
    //         string trimmedLine = line.Trim();

    //         // Ignore empty lines and using directives
    //         if (!string.IsNullOrWhiteSpace(trimmedLine) && !trimmedLine.StartsWith("using"))
    //         {
    //             // Convert class declaration
    //             if (trimmedLine.StartsWith("class"))
    //             {
    //                 // Extract class name
    //                 string className = ExtractClassName(trimmedLine);
    //                 plantUmlText.AppendLine($"class {className} {{");
    //             }
    //             // Convert property declaration
    //             else if (trimmedLine.StartsWith("public"))
    //             {
    //                 // Extract property name
    //                 string propertyName = ExtractMethodName(trimmedLine);
    //                 plantUmlText.AppendLine($"- {propertyName}");
    //             }
    //             // Convert method declaration
    //             else if (trimmedLine.StartsWith("public void"))
    //             {
    //                 // Extract method name
    //                 string methodName = ExtractMethodName(trimmedLine);
    //                 plantUmlText.AppendLine($"+ {methodName}()");
    //             }
    //         }
    //     }

    //     // Add PlantUML end directive
    //     plantUmlText.AppendLine("}\n@enduml");

    //     return plantUmlText.ToString();
    // }

    // static string ExtractMethodName(string methodDeclaration)
    // {
    //     // Find the position of the opening parenthesis
    //     int openParenthesisIndex = methodDeclaration.IndexOf('(');
    //     if (openParenthesisIndex == -1)
    //     {
    //         throw new ArgumentException("Invalid method declaration: Missing opening parenthesis.");
    //     }

    //     // Find the position of the last space before the opening parenthesis
    //     int lastSpaceIndex = methodDeclaration.LastIndexOf(' ', openParenthesisIndex);
    //     if (lastSpaceIndex == -1)
    //     {
    //         throw new ArgumentException("Invalid method declaration: Missing method name.");
    //     }

    //     // Find the position of the second-last space before the last space
    //     int secondLastSpaceIndex = methodDeclaration.LastIndexOf(' ', lastSpaceIndex - 1);
    //     if (secondLastSpaceIndex == -1)
    //     {
    //         throw new ArgumentException("Invalid method declaration: Missing method name.");
    //     }

    //     // Extract the method name
    //     string methodName = methodDeclaration.Substring(secondLastSpaceIndex + 1, lastSpaceIndex - secondLastSpaceIndex - 1);
    //     return methodName;
    // }

    static string ExtractClassName(string classDeclaration)
    {
        // Find the position of the opening curly brace
        int openCurlyBraceIndex = classDeclaration.IndexOf('{');
        if (openCurlyBraceIndex == -1)
        {
            throw new ArgumentException("Invalid class declaration: Missing opening curly brace.");
        }

        // Find the position of the last space before the opening curly brace
        int lastSpaceIndex = classDeclaration.LastIndexOf(' ', openCurlyBraceIndex);
        if (lastSpaceIndex == -1)
        {
            throw new ArgumentException("Invalid class declaration: Missing class name.");
        }

        // Extract the class name
        string className = classDeclaration.Substring(lastSpaceIndex + 1, openCurlyBraceIndex - lastSpaceIndex - 1);
        return className;
    }

    public string extractFields(string inputFile){
      string inputFile = "./InterfaceText/InterfaceText.cs";

        string cSharpCode = File.ReadAllText(inputFile);

        SyntaxTree tree = CSharpSyntaxTree.ParseText(cSharpCode);
        CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

        // Extract fields
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
        }

    }

    public string extractMethods(string inputFile){
      string inputFile = "./InterfaceText/InterfaceText.cs";

        string cSharpCode = File.ReadAllText(inputFile);

        SyntaxTree tree = CSharpSyntaxTree.ParseText(cSharpCode);
        CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

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
        }
    }
}