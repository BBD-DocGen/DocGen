using DocGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Classes
{
    internal class Provider
    {
        private static HttpClient client = new HttpClient();

        private static readonly string API_URL = "http://localhost:3010/api/v1";
        //private static readonly string API_URL = "http://docgen-app.eu-west-1.elasticbeanstalk.com/api/v1";

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
            var values = new Dictionary<string, string>
            {
                { "fileName", "hello.cs" },
                { "content", "world" }
            };

            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await client
                .PostAsync($"{API_URL}/uploaded-documents", content);

            Content con = null;
            string res = "";
            if (response.IsSuccessStatusCode) 
                res = await response.Content.ReadAsStringAsync();

            return con;
        }
    }
}
