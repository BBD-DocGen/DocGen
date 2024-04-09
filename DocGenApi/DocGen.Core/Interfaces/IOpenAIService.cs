namespace DocGen.Core.Interfaces;

public interface IOpenAIService
{
    Task<string> GenerateSummaryAsync(string codeContent);
}