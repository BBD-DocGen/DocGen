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

namespace DocGen.Classes{
    class Provider
    {
        private static HttpClient client = new HttpClient();

        private static readonly string API_URL = "http://localhost:5000/api/v1";
        // private static readonly string API_URL = "http://docgen-app.eu-west-1.elasticbeanstalk.com/api/v1";

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
            Console.WriteLine("doc.fileName \n" + doc.fileName);
            Console.WriteLine("doc.content \n" + doc.content);
            Console.WriteLine("client \n" + client.ToString());

            HttpResponseMessage response = await client
                .PostAsJsonAsync<Document>($"{API_URL}/uploaded-documents", doc);

                Console.WriteLine("response: \n" + response +"\n");

            if (response.IsSuccessStatusCode)

                return await response.Content.ReadFromJsonAsync<Content>();

            return new Content();
        }

        // public static async Task<Content> getAllGeneratedDoc(string id)
        // {
        //     var document = new Document();
        //     HttpResponseMessage response = await client
        //         .GetFromJsonAsync<Document>($"{API_URL}/uploaded-documents",document);

        //         Console.WriteLine("response: \n" + response +"\n");

        //     if (response.IsSuccessStatusCode)

        //         return await response.Content.ReadFromJsonAsync<Content>();

        //     return new Content();
        // }
    }

}