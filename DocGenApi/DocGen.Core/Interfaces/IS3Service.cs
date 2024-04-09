namespace DocGen.Core.Interfaces;

    public interface IS3Service
    {
       // Task<string> GetFileContentAsync(string bucketName, string key);
       // Task<string> GetFileById();
        Task<string> UploadFileContentAsync(string content, string bucketName, string key);
}

