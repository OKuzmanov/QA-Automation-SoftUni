using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp_GitHub_Demo;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitHub_Issues_API_Tests
{
    public class IssuesApiTests
    {
        private RestClient client;
        private RestRequest request;

        private string owner = "OKuzmanov";
        private string repo = "QA-Automation-SoftUni";
        private string token = "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";

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient("https://api.github.com");
            this.client.Authenticator = new HttpBasicAuthenticator(owner, token);
        }

        [Test]
        public async Task Test_GetAllIssuesStatusOk()
        {
            //Arrange
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues", Method.Get);

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
        }

        [Test]
        public async Task Test_GetAllIssuesMiltipleIssues()
        {
            //Arrange
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues", Method.Get);

            //Act
            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));

            var issues = JsonSerializer.Deserialize<List<GithubIssue>>(response.Content);

            //Assert
            Assert.That(issues.Count > 0);

            foreach (var issue in issues)
            {
                Assert.Greater(issue.id, 0);
                Assert.Greater(issue.number, 0);
                Assert.IsNotEmpty(issue.title);
            }
        }

        [Test]
        public async Task Test_GetAllIssuesInvalidRepository()
        {
            this.request = new RestRequest("/repos/OKuzmanov/NonExistentRepo/issues", Method.Get);

            RestResponse response = await this.client.ExecuteAsync(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Test_GetAllIssuesInvalidUser()
        {
            this.request = new RestRequest("/repos/InvalidUser/NonExistentRepo/issues", Method.Get);

            RestResponse response = await this.client.ExecuteAsync(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Test_GetSignleIssue()
        {
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues/2", Method.Get);

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);   

            GithubIssue issue = JsonSerializer.Deserialize<GithubIssue>(response.Content);

            Assert.IsNotNull(issue);
            Assert.That(issue.id, Is.GreaterThan(0));
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title.Length, Is.GreaterThan(0));
        }

        [Test]
        public async Task Test_PostIssueWithTitleAndBody()
        {
            //Arrange
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues", Method.Post);
            string titleToAdd = "Test Post Request from RestSharp";
            string bodyToAdd = "This issue is made via RestSharp API.";
            this.request.AddJsonBody(new { title = titleToAdd, body = bodyToAdd });

            //Act
            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var issue =JsonSerializer.Deserialize<GithubIssue>(response.Content);

            Assert.IsTrue(issue.id > 0);
            Assert.IsTrue(issue.number > 0);
            Assert.AreEqual(titleToAdd, issue.title);
            Assert.AreEqual(bodyToAdd, issue.body);
        }

        [Test]
        public async Task Test_PostIssueWithoutTitle()
        {
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues", Method.Post);
            string bodyToAdd = "This issue is made via RestSharp API.";
            this.request.AddJsonBody(new { body = bodyToAdd });

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Test]
        public async Task Test_UpdateExistingIssue()
        {
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues/27", Method.Patch);
            string newTitle = "Test Update Issue RestSharp";
            string newBody = "The new updated body of the issue.";
            this.request.AddJsonBody(new { title = newTitle, body = newBody });

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            GithubIssue issue = JsonSerializer.Deserialize<GithubIssue>(response.Content);

            Assert.That(issue.id, Is.GreaterThan(0));
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.AreEqual(newTitle, issue.title);
            Assert.AreEqual(newBody, issue.body);
        }

        [Test]
        public async Task Test_UpdateNonExistingIssue()
        {
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues/113000000", Method.Patch);
            string newTitle = "Test Update Issue RestSharp";
            string newBody = "The new updated body of the issue.";
            this.request.AddJsonBody(new { title = newTitle, body = newBody });

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    public class IssueCommentsApiTests
    {
        private RestClient client;
        private RestRequest request;

        private string owner = "OKuzmanov";
        private string repo = "QA-Automation-SoftUni";
        private string token = "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";

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient("https://api.github.com");
            this.client.Authenticator = new HttpBasicAuthenticator(owner, token);
        }

        [Test]
        public async Task Test_IssueCommentsGetAllComments()
        {
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues/27/comments", Method.Get);

            var response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            List<GithubComment> comments = JsonSerializer.Deserialize<List<GithubComment>>(response.Content);

            Assert.That(comments.Count > 0);

            foreach (GithubComment comment in comments)
            {
                Assert.That(comment.id, Is.GreaterThan(0));
                Assert.That(comment.body.Length, Is.GreaterThan(0));
            }
        }

        [Test]
        public async Task Test_IssueCommentsGetSingleComment()
        {
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues/comments/1127436258", Method.Get);

            var response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            GithubComment comment = JsonSerializer.Deserialize<GithubComment>(response.Content);

            Assert.That(comment.id, Is.GreaterThan(0));
            Assert.That(comment.body.Length, Is.GreaterThan(0));

        }

        [Test]
        public async Task Test_IssueCommentCreateComment()
        {
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues/27/comments", Method.Post);
            string bodyToAdd = "Test Comment for issue 27 using RestSharp API.";
            this.request.AddJsonBody(new { body = bodyToAdd });

            var response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            GithubComment comment = JsonSerializer.Deserialize<GithubComment>(response.Content);

            Assert.That(comment.id, Is.GreaterThan(0));
            Assert.AreEqual(bodyToAdd, comment.body);
        }

        [Test]
        public async Task Test_IssueCommentCreateCommentNoBody()
        {
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues/27/comments", Method.Post);

            var response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        }

        [Test]
        public async Task Test_IssueCommentUpdateExistingComment()
        {
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues/comments/1127458188", Method.Patch);
            string updatedBody = "Testing the update issue comment functionality of GitHub RESTful API.";
            this.request.AddBody(new { body = updatedBody });

            var response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            GithubComment comment = JsonSerializer.Deserialize<GithubComment>(response.Content);

            Assert.That(comment.id, Is.GreaterThan(0));
            Assert.AreEqual(comment.body, updatedBody);
        }

        [Test]
        public async Task Test_IssueCommentUpdateNonExistingComment()
        {
            this.request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni/issues/comments/00000000", Method.Patch);
            string updatedBody = "Testing the update issue comment functionality of GitHub RESTful API with an invalid comment.";
            this.request.AddBody(new { body = updatedBody });

            var response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Test_IssueCommentDeleteComment()
        {
            GithubIssue issue = await createNewIssue("Test Issue - Delete Comment", "Testing the delete comment functionality.");

            GithubComment comment = await createNewCommentForIssue(issue, "Comment to be deleted");

            int oldCommentsCount = await CountCommentsForIssue(issue);

            RestRequest deleteRequest = new RestRequest($"/repos/{owner}/{repo}/issues/comments/{comment.id}", Method.Delete);

            RestResponse deleteResponse = await this.client.ExecuteAsync(deleteRequest);

            int newCommentsCount = await CountCommentsForIssue(issue);

            Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            Assert.Greater(oldCommentsCount, newCommentsCount);
        }

        private async Task<GithubComment> createNewCommentForIssue(GithubIssue issue, string body)
        {
            RestRequest createCommentRequest = new RestRequest($"/repos/{owner}/{token}/issues/{issue.number}/comments", Method.Post);
            createCommentRequest.AddJsonBody(new { body = body });
            RestResponse createCommentResponse = await this.client.ExecuteAsync(createCommentRequest);
            GithubComment comment = JsonSerializer.Deserialize<GithubComment>(createCommentResponse.Content);
            return comment;
        }

        private async Task<GithubIssue> createNewIssue(string title, string body)
        {
            RestRequest createIssuerequest = new RestRequest($"/repos/{owner}/{repo}/issues", Method.Post);
            createIssuerequest.AddJsonBody(new { title = title, body = body });
            RestResponse createIssueresponse = await this.client.ExecuteAsync(createIssuerequest);
            GithubIssue issue = JsonSerializer.Deserialize<GithubIssue>(createIssueresponse.Content);
            return issue;
        }

        public async Task<int> CountCommentsForIssue(GithubIssue issue)
        {
            RestRequest listCommentsRequest = new RestRequest($"/repos/{owner}/{repo}/issues/{issue.number}/comments");
            RestResponse restResponse = await this.client.ExecuteAsync(listCommentsRequest);
            List<GithubComment> commentList = JsonSerializer.Deserialize<List<GithubComment>>(restResponse.Content);
            return commentList.Count;
        }
    }
}