using DocGen.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static async Task<ObservableCollection<GeneratedDocs>> getGeneratedDocs()
        {
            HttpResponseMessage response = await client
                .GetAsync($"{API_URL}/generated-documents");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ObservableCollection<GeneratedDocs>>();

            return new ObservableCollection<GeneratedDocs>();
        }

        public static async Task<ObservableCollection<GeneratedDocs>> getUploadedDocs()
        {
            HttpResponseMessage response = await client
                .GetAsync($"{API_URL}/uploaded-documents");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ObservableCollection<GeneratedDocs>>();

            return new ObservableCollection<GeneratedDocs>();
        }

        public static async Task<String> downloadFile(string url)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client
                .GetAsync(url);

            string content = await response.Content.ReadAsStringAsync();

            return content;
        }
    }
}
