namespace DocGen.Core.Interfaces;

    public interface IS3Service
    {
        Task<string> GetFileContentAsync(string fileUrl);
        Task<string> UploadFileContentAsync(string content, string keyName);
}