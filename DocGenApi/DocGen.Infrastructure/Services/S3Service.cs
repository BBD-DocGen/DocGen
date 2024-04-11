using Amazon.S3;
using Amazon.S3.Model;
using DocGen.Core.Interfaces;
public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;

    public S3Service(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<string> UploadFileContentAsync(string content, string bucketName, string key)
    {
        try
        {
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                ContentBody = content
            };

            PutObjectResponse response = await _s3Client.PutObjectAsync(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"https://{bucketName}.s3.amazonaws.com/{key}";
            }
            else
            {
                throw new InvalidOperationException(
                    $"Failed to upload object to S3. Status code: {response.HttpStatusCode}");
            }
        }
        catch (AmazonS3Exception e)
        {
            throw new InvalidOperationException($"Error uploading object to S3: {e.Message}", e);
        }
    }
}