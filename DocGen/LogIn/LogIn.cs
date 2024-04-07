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
                    var id_token = responseObject.id_token;
                    InterfaceText.insertBoundaryText();
                    return id_token;
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
      request.AddParameter("application/x-www-form-urlencoded", "client_id=RvnPGiHVhtjPJ76vzfaIM0WmidvjRwl7&scope=openid", ParameterType.RequestBody);
      RestResponse response = null;

      try{
        response = client.ExecutePost(request);
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
      request.AddParameter("application/x-www-form-urlencoded", $"grant_type=urn%3Aietf%3Aparams%3Aoauth%3Agrant-type%3Adevice_code&device_code={responseObject.device_code}&client_id=RvnPGiHVhtjPJ76vzfaIM0WmidvjRwl7", ParameterType.RequestBody);
      RestResponse response = null;

      try{
        response = client.ExecutePost(request);
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
}