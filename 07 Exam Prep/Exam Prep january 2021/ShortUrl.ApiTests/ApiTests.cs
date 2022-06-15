using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace ShortUrl.ApiTests
{
    public class ApiTests
    {
        const string ApiKey = "https://shorturl.nakov.repl.co/api";

        private RestClient client;

        [OneTimeSetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Test_ListAllShortUrls()
        {
            var request = new RestRequest(ApiKey + "/urls", Method.Get);

            RestResponse response = this.client.Execute(request);

            List<ShortCodeUrl> shortCodeUrls = JsonSerializer.Deserialize<List<ShortCodeUrl>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(shortCodeUrls.Count, Is.GreaterThan(0));
        }

        [Test]
        public void Test_FindUrlByShortCode_ValidData()
        {
            const string shortCode = "nak";

            var request = new RestRequest(ApiKey + "/urls/" + shortCode, Method.Get);

            RestResponse response = this.client.Execute(request);

            ShortCodeUrl shortCodeUrl = JsonSerializer.Deserialize<ShortCodeUrl>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.NotNull(shortCodeUrl.url);
            Assert.NotNull(shortCodeUrl.shortCode);
            Assert.NotNull(shortCodeUrl.dateCreated);
        }

        [Test]
        public void Test_FindUrlByShortCode_InvalidData()
        {
            string shortCode = "rand" + DateTime.Now.Ticks;

            var request = new RestRequest(ApiKey + "/urls/" + shortCode, Method.Get);

            RestResponse response = this.client.Execute(request);

            ShortCodeUrl shortCodeUrl = JsonSerializer.Deserialize<ShortCodeUrl>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Test_CreateNewShortUrl_ValidData()
        {
            string uniqueUrl = "testUrl" + DateTime.Now.Ticks;
            string uniqueShortCode = "te" + DateTime.Now.Ticks;

            var request = new RestRequest(ApiKey + "/urls/", Method.Post);
            request.AddJsonBody(new {url = uniqueUrl, shortCode = uniqueShortCode});

            RestResponse response = this.client.Execute(request);

            ShortCodeUrl shortCodeUrl = JsonSerializer.Deserialize<ShortCodeUrl>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(shortCodeUrl.url, Is.EqualTo(uniqueUrl));
            Assert.That(shortCodeUrl.shortCode, Is.EqualTo(uniqueShortCode));
        }

        [Test]
        public void Test_CreateNewShortUrl_InvalidData()
        {
            //Not Used
            //string uniqueUrl = "testUrl" + DateTime.Now.Ticks;
            //string uniqueShortCode = "nak" + DateTime.Now.Ticks;

            var request = new RestRequest(ApiKey + "/urls/", Method.Post);
            request.AddJsonBody(new {});

            RestResponse response = this.client.Execute(request);

            ShortCodeUrl shortCodeUrl = JsonSerializer.Deserialize<ShortCodeUrl>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Test_DeleteShortUrl_ValidData()
        {
            string uniqueUrl = "testUrl" + DateTime.Now.Ticks;
            string uniqueShortCode = "te" + DateTime.Now.Ticks;

            var requestCreateSCU = new RestRequest(ApiKey + "/urls/", Method.Post);
            requestCreateSCU.AddJsonBody(new { url = uniqueUrl, shortCode = uniqueShortCode });

            RestResponse responseCreateSCU = this.client.Execute(requestCreateSCU);

            Assert.That(responseCreateSCU.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var requestGetAllSCU = new RestRequest(ApiKey + "/urls", Method.Get);

            RestResponse responseGetAllSCU = this.client.Execute(requestGetAllSCU);

            Assert.That(responseGetAllSCU.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var countUrlsBeforeDelete = JsonSerializer.Deserialize<List<ShortCodeUrl>>(responseGetAllSCU.Content).Count;

            var requestDeleteSCU = new RestRequest(ApiKey + "/urls/" + uniqueShortCode, Method.Delete);

            var responseDeleteSCU = this.client.Execute(requestDeleteSCU);

            Assert.That(responseDeleteSCU.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            RestResponse responsepostDelete = this.client.Execute(requestGetAllSCU);

            Assert.That(responsepostDelete, Is.EqualTo(HttpStatusCode.OK));

            var countUrlsAfterDelete = JsonSerializer.Deserialize<List<ShortCodeUrl>>(responsepostDelete.Content).Count;

            Assert.That(countUrlsBeforeDelete, Is.GreaterThan(countUrlsAfterDelete));
        }

        [Test]
        public void Test_DeleteShortUrl_InvalidData()
        {
            string randShortCode = "rand" + DateTime.Now.Ticks;

            var request = new RestRequest(ApiKey + "/urls/" + randShortCode, Method.Delete);

            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}