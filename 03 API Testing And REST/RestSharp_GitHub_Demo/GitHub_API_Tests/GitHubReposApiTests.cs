using GitHub_Models;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitHub_Repos_API_Tests
{
   public class RepoApiTests
    {

        private RestClient client;

        [SetUp]
        public void init()
        {
            this.client = new RestClient("https://api.github.com");
            this.client.Authenticator = new HttpBasicAuthenticator("OKuzmanov", "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");
        }

        [Test]
        public async Task Test_GetAllReposForUser()
        {
            RestRequest request = new RestRequest("/users/OKuzmanov/repos", Method.Get);

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var repos = JsonSerializer.Deserialize<List<GithubRepo>>(response.Content);

            foreach (var repo in repos)
            {
                Assert.That(repo.id, Is.GreaterThan(0));
                Assert.IsNotNull(repo.name);
                Assert.IsNotNull(repo.fullName);
            }
        }

        [Test]
        public async Task Test_GetRepository()
        {
            RestRequest request = new RestRequest("/repos/OKuzmanov/QA-Automation-SoftUni", Method.Get);

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            GithubRepo repo = JsonSerializer.Deserialize<GithubRepo>(response.Content);

            Assert.That(repo.id, Is.GreaterThan(0));
            Assert.AreEqual("QA-Automation-SoftUni", repo.name);
            Assert.AreEqual("OKuzmanov/QA-Automation-SoftUni", repo.fullName);
            Assert.IsNull(repo.description);
        }

        [Test]
        public async Task Test_GetRepositoryInvalidRepoName()
        {
            RestRequest request = new RestRequest("/repos/OKuzmanov/NonExistentRepo", Method.Get);

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Test_CreateRepository()
        {
            string nameToAdd = "Test_Repo_RestSharp";
            string descriptionToAdd = "This is a test repo created using the RestSharp Api.";

            RestRequest request = new RestRequest("/user/repos", Method.Post);
            request.AddJsonBody(new { name = nameToAdd, description = descriptionToAdd });
            int oldRepoCount = await getReposCount();

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            GithubRepo repo = JsonSerializer.Deserialize<GithubRepo>(response.Content);

            Assert.That(repo.id, Is.GreaterThan(0));
            Assert.AreEqual(nameToAdd, repo.name);
            Assert.AreEqual("OKuzmanov/" + nameToAdd, repo.fullName);
            Assert.AreEqual(descriptionToAdd, repo.description);

            int newRepoCount = await getReposCount();
            Assert.Greater(newRepoCount, oldRepoCount);
        }

        [Test]
        public async Task Test_DeleteRepository()
        {
            string nameToDelete = "Test_Repo_RestSharp";
            RestRequest request = new RestRequest("/repos/Okuzmanov/" + nameToDelete, Method.Delete);

            int oldRepoCount = await getReposCount();

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            int newRepoCount = await getReposCount();
            Assert.Greater(oldRepoCount, newRepoCount);
        }

        [Test]
        public async Task Test_DeleteRepositoryInvalidRepoName()
        {
            string nameToDelete = "NonExistentRepo";
            RestRequest request = new RestRequest("/repos/Okuzmanov/" + nameToDelete, Method.Delete);

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Test_UpdateRepository()
        {
            string repoName = "Test_Repo_RestSharp";
            RestRequest request = new RestRequest("/repos/OKuzmanov/" + repoName, Method.Patch);
            string updatedName = "Updated_TestRepo_Name";
            string updatedDescription = "Updated Description using the RestSharp Api.";
            request.AddJsonBody(new GithubRepo(updatedName, updatedDescription, true));

            int oldRepos = await getReposCount();
            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            GithubRepo repo = JsonSerializer.Deserialize<GithubRepo>(response.Content);

            Assert.AreEqual(updatedName, repo.name);
            Assert.AreEqual(updatedDescription, repo.description);
            Assert.AreEqual(true, repo.access);

            int newRepos = await getReposCount();
            Assert.AreEqual(oldRepos, newRepos);
        }

        [Test]
        public async Task Test_UpdateRepositoryInvalidRepoName()
        {
            string repoName = "NonExistentRepoName";
            RestRequest request = new RestRequest("/repos/OKuzmanov/" + repoName, Method.Patch);

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        private async Task<int> getReposCount()
        {
            return JsonSerializer.Deserialize<List<GithubRepo>>((await this.client.ExecuteAsync(new RestRequest("/user/repos"))).Content).Count;
        }
    }
}
