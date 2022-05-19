using RestSharp;
using RestSharp.Authenticators;
using RestSharp_GitHub_Demo;
using System.Text.Json;

//// Get Request All Issues
//var client = new RestClient("https://api.github.com");

//var request = new RestRequest("repos/OKuzmanov/QA-Automation-SoftUni/issues", Method.Get);

//var response = await client.ExecuteAsync(request);

//var issues = JsonSerializer.Deserialize<List<GithubIssue>>(response.Content);

//foreach (var issue in issues)
//{
//    Console.WriteLine("ID: " + issue.id);
//    Console.WriteLine("Number: " + issue.number);
//    Console.WriteLine("TITLE: " + issue.title);
//    Console.WriteLine("BODY: " + issue.body);
//    Console.WriteLine("**********");
//}

//// Get Request Signle Issue
//var client = new RestClient("https://api.github.com");

//var request = new RestRequest("repos/OKuzmanov/QA-Automation-SoftUni/issues/1", Method.Get);

//var response = await client.ExecuteAsync(request);

//var issue = JsonSerializer.Deserialize<GithubIssue>(response.Content);

//Console.WriteLine("ID: " + issue.id);
//Console.WriteLine("Number: " + issue.number);
//Console.WriteLine("TITLE: " + issue.title);
//Console.WriteLine("BODY: " + issue.body);
//Console.WriteLine("**********");

var client = new RestClient("https://api.github.com");
client.Authenticator = new HttpBasicAuthenticator("OKuzmanov", "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");

for (int i = 2; i <= 10; i++)
{
    var request = new RestRequest("repos/OKuzmanov/QA-Automation-SoftUni/issues/27/comments", Method.Post);
    request.AddJsonBody(new { body = "Test comment #" + i + " from VS using RestSharp API." });
    var response = await client.ExecuteAsync(request);
}
 