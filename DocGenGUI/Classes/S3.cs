using Amazon.Runtime;
using Amazon.S3.Model;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DocGen.Classes
{
    internal class S3
    {
        private static string BUCKET_NAME = "docgen-documents-20240403";
        private static AmazonS3Client s3Client = new AmazonS3Client(
            new BasicAWSCredentials(
                "AKIAVUJWQDTHCX7ED6NC",
                "vT3LZM+juGfZkS69/d75pMU3S0h9fYEEmGIbaGD3"), 
            Amazon.RegionEndpoint.EUWest1
        );

        public async static Task uploadFile(string keyName, string fileContent)
        {
            try
            {
                await s3Client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = BUCKET_NAME,
                    Key = keyName,
                    ContentBody = fileContent,
                    ContentType = "text/plain",
                });

                MessageBox.Show("Upload completed!");
            }
            catch (AmazonS3Exception e)
            {
                MessageBox.Show(e.Message, "Error encountered on server.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Unknown encountered on server.");
            }
        }
    }
}
