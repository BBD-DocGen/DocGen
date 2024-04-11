namespace DocGen.Core.Interfaces;

    public interface IS3Service
    {
        Task<string> UploadFileContentAsync(string content, string bucketName, string key);
}

