using RestSharp;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net;

class LogIn{
    public static string Login()
    {
      try
        {
            var response = requestDeviceCodeUri();

            if (response != null)
            {
                if (response.StatusCode != HttpStatusCode.OK){
                  InterfaceText.insertBoundaryText();
                  Console.WriteLine("Cannot make request to the Autho website, please check your network connection and try again.");
                  return "";
                }
                else{
                  var responseObject = JsonConvert.DeserializeObject<dynamic>(response.Content);
                  string verification_uri = responseObject.verification_uri;
                  openUri(verification_uri);

                  Console.WriteLine("Verification page has been opened on your default browser, or go to below link:");
                  Console.WriteLine($"{responseObject.verification_uri}");
                  Console.WriteLine($"Your One-time Code is: {responseObject.user_code}, please use this to link your device\n");
                  Console.WriteLine();
                  Console.WriteLine("1. Continue if you are successfully connected");
                  Console.WriteLine("2. Exit log in");
                  var oAuthOption = Console.ReadLine();
                  Console.WriteLine();

                  while (oAuthOption != "1"){
                    if (oAuthOption == "2"){
                      InterfaceText.insertBoundaryText();
                      Console.WriteLine("You have exited the log in process.");
                      return "";
                    }
                    Console.WriteLine("Verification page has been opened on your default browser, or go to below link:");
                    Console.WriteLine($"{responseObject.verification_uri}");
                    Console.WriteLine($"Your One-time Code is: {responseObject.user_code}");
                    Console.WriteLine("1. Continue if you are successfully connected");
                    Console.WriteLine("2. Exit log in");
                    oAuthOption = Console.ReadLine();
                  }

                  response = requestTokenUri(responseObject);
                  if (response.StatusCode != HttpStatusCode.OK){
                    InterfaceText.insertBoundaryText();
                    Console.WriteLine("Your device is not succesfully connected, please try again.");
                    return "";
                  }
                  else{
                    responseObject = JsonConvert.DeserializeObject<dynamic>(response.Content);
                    var access_token = responseObject.id_token;
                    InterfaceText.insertBoundaryText();
                    return access_token;
                  }
                }
            }
            else
            {
                Console.WriteLine("No response received.");
                return "";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return "";
        }
    }

    public static RestResponse requestDeviceCodeUri(){
      var client = new RestClient("https://dev-2f8sdpf6pls655l7.us.auth0.com/oauth/device/code");
      var request = new RestRequest();
      request.AddHeader("content-type", "application/x-www-form-urlencoded");
      request.AddParameter("application/x-www-form-urlencoded", "client_id=RvnPGiHVhtjPJ76vzfaIM0WmidvjRwl7&scope=openid&audience=https://docgen.com", ParameterType.RequestBody);
      RestResponse response = null;

      try{
        response = client.ExecutePost(request);

        Console.WriteLine("response: \n" + JsonConvert.DeserializeObject<dynamic>(response.Content) +"\n");

              Console.WriteLine(JsonConvert.DeserializeObject<dynamic>(response.Content).device_code);
      }
      catch (Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
      }
      return response;
    }

    public static RestResponse requestTokenUri(dynamic responseObject){
      var client = new RestClient("https://dev-2f8sdpf6pls655l7.us.auth0.com/oauth/token");
      var request = new RestRequest();
      request.AddHeader("content-type", "application/x-www-form-urlencoded");
      request.AddParameter("application/x-www-form-urlencoded", $"grant_type=urn%3Aietf%3Aparams%3Aoauth%3Agrant-type%3Adevice_code&device_code={responseObject.device_code}&client_id=RvnPGiHVhtjPJ76vzfaIM0WmidvjRwl7&audience=https://docgen.com", ParameterType.RequestBody);
      RestResponse response = null;

      try{
        response = client.ExecutePost(request);

                Console.WriteLine("response: \n" + JsonConvert.DeserializeObject<dynamic>(response.Content) +"\n");
      }
      catch (Exception ex){
            Console.WriteLine($"Error: {ex.Message}");
      }
      return response;
    }

    public static void openUri(string verification_uri){
      string auth0VerificationUri = verification_uri;
      var auth0VerificationUriInfo = new ProcessStartInfo {
          FileName = auth0VerificationUri,
          UseShellExecute = true
      };
      Process.Start(auth0VerificationUriInfo);
    }

    public static async Task<string> LogInUserCheck(string access_token){
      HttpClient client = new HttpClient();
      var parameters = new Dictionary<string, string>{};

      try
      {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {access_token}");
        // client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

        var content = new FormUrlEncodedContent(parameters);
        HttpResponseMessage response = await client.PostAsync("http://docgen-app.eu-west-1.elasticbeanstalk.com/api/v1/users",null);

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("success");
            Console.WriteLine(responseBody);
            return responseBody;
        }
        else
        {
            return $"HTTP Error: {response.StatusCode} - {response.ReasonPhrase}";
        }
      }
      catch (HttpRequestException e)
      {
        return $"HTTP Request Error: {e.Message}";
      }
    }
}