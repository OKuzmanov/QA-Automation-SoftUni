using RestSharp;
using System.Text.Json;

RestClient client = new RestClient("https://api.zippopotam.us");

RestRequest request = new RestRequest("/BG/1000");

RestResponse response = await client.ExecuteAsync(request);

Console.WriteLine(response.Content);
