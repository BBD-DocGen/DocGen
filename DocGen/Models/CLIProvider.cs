using DocGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Classes{
    class CLIProvider
    {
        private static HttpClient client = new HttpClient();

        //private static readonly string API_URL = "http://localhost:5000/api/v1";
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

            Console.WriteLine(response);

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
    }

}