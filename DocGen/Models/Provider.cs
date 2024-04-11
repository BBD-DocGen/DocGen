using DocGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DocGen.Classes
{
    internal class Provider
    {
        private static HttpClient client = new HttpClient();

        // private static readonly string API_URL = "http://localhost:5000/api/v1";
        private static readonly string API_URL = "http://docgen-app.eu-west-1.elasticbeanstalk.com/api/v1";

        public static void setHeader(string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public static async Task<bool> login()
        {
            HttpResponseMessage response = await client
                .PostAsync($"{API_URL}/user", null);

            try
            {
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                return false;
            }
        }

        public static async Task<Content> uploadDocuAndGetGeneratedDoc(Document doc)
        {
            HttpResponseMessage response = await client
                .PostAsJsonAsync<Document>($"{API_URL}/uploaded-documents", doc);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<Content>();

            return new Content();
        }

        // public static async Task<string> getAllGeneratedDocs()
        // {
        //     HttpResponseMessage response = await client
        //         .GetAsync($"{API_URL}/uploaded-documents");

        //     if (response.IsSuccessStatusCode)
        //         return await response.Content.ReadAsStringAsync();

        //     return "There are no ducoments to retrieve.";
        // }

        public static async Task<List<GeneratedDocs>> getGeneratedDocs()
        {
            Console.WriteLine("Printing all your past generated documents...");
            HttpResponseMessage response = await client
                .GetAsync($"{API_URL}/generated-documents");

            if (response.IsSuccessStatusCode){
                string responseString = await response.Content.ReadAsStringAsync();
                // Console.WriteLine("responseString: \n" + responseString);
                List<GeneratedDocs> generatedDocsList = JsonConvert.DeserializeObject<List<GeneratedDocs>>(responseString);
                return generatedDocsList;
            }

            return new List<GeneratedDocs>();
        }

        public static async Task<List<UploadedDocs>> getUploadedDocs()
        {
            Console.WriteLine("Printing all your past uploaded documents...");
            HttpResponseMessage response = await client
                .GetAsync($"{API_URL}/uploaded-documents");

            if (response.IsSuccessStatusCode){
                string responseString = await response.Content.ReadAsStringAsync();
                // Console.WriteLine("responseString: \n" + responseString);
                List<UploadedDocs> generatedDocsList = JsonConvert.DeserializeObject<List<UploadedDocs>>(responseString);
                return generatedDocsList;
            }

            return new List<UploadedDocs>();
        }
    }
}
