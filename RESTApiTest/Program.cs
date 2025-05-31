using System.Net;
using System.Text.Json;

string username = "asdf";
string authToken = "ceQ78t4rBXH421mHoLIhFqz5vnTfFnFaU0LK24pEcUk=";

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