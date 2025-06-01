using System.Net;
using System.Text.Json;

public class MovieTitleDTO
{
    public string Title { get; set; }
}

public class Program
{
    static void GETTest()
    {
        string username = "tk2";
        string authToken = "oNL3NfmImE2VPkMJr4CP1VLTr6v4Wr1hUCdUHgAI2oE=";

        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5177/api/Api/Movies");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "GET";

        httpWebRequest.Headers.Add("X-Username", username);
        httpWebRequest.Headers.Add("X-Auth-Token", authToken);

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            Console.WriteLine(result);
        }
    }

    static void POSTest()
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5177/api/Api/MovieWatch");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        string username = "tk2";
        string authToken = "oNL3NfmImE2VPkMJr4CP1VLTr6v4Wr1hUCdUHgAI2oE=";
        httpWebRequest.Headers.Add("X-Username", username);
        httpWebRequest.Headers.Add("X-Auth-Token", authToken);


        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string json = JsonSerializer.Serialize(new
            {
                Title = "Avatar",
                Priority = 100 
            });
            streamWriter.Write(json);
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            Console.WriteLine(result);
        }
    }
    static void DELETest()
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5177/api/Api/Watchlists");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "DELETE";
        string username = "tk2";
        string authToken = "oNL3NfmImE2VPkMJr4CP1VLTr6v4Wr1hUCdUHgAI2oE=";
        httpWebRequest.Headers.Add("X-Username", username);
        httpWebRequest.Headers.Add("X-Auth-Token", authToken);


        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            string json = JsonSerializer.Serialize(new
            {
                Title = "Avatar"
            });
            streamWriter.Write(json);
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            Console.WriteLine(result);
        }
    }
    public static void Main()
    {
        POSTest();
    }

}
