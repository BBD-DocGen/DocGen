class SendHttpRequest{

  public static async Task<string> SendGetHttpRequest(string requestUrl){
    HttpClient client = new HttpClient();
    try
      {
        // Send a GET request to a URL
        HttpResponseMessage response = await client.GetAsync(requestUrl);

        // Check if the response is successful (status code 200)
        if (response.IsSuccessStatusCode)
        {
            // Read the response content as a string
            string responseBody = await response.Content.ReadAsStringAsync();

            // Print the response body
            Console.WriteLine(responseBody);
            return responseBody;
        }
        else
        {
            Console.WriteLine($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
            return $"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}";
        }
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine($"HTTP Request Error: {e.Message}");
        return $"HTTP Request Error: {e.Message}";
    }
  }

    public static async Task<string> SendPostHttpRequest(string requestUrl){
      HttpClient client = new HttpClient();
        var parameters = new Dictionary<string, string>
        {
            { "name", "morpheus" },
            { "job", "leader"}
        };

      try
            {
              var content = new FormUrlEncodedContent(parameters);
                // Send a GET request to a URL
                HttpResponseMessage response = await client.PostAsync(requestUrl,content);

                // Check if the response is successful (status code 200)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("success");
                    // Print the response body
                    Console.WriteLine(responseBody);
                    return responseBody;
                }
                else
                {
                    Console.WriteLine($"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return $"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HTTP Request Error: {e.Message}");
                return $"HTTP Request Error: {e.Message}";
            }
  }

}