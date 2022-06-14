using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace ContactBook.APITests
{
    public class ApiTests
    {
        private RestClient client;
        private const string ApiKey = "http://localhost:8080/api";

        [OneTimeSetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        [Test]
        public void Test_GetAllContacts_CheckFirstContact()
        {
            RestRequest request = new RestRequest(ApiKey + "/contacts", Method.Get);

            RestResponse response = this.client.Execute(request);

            List<Contact> contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            Assert.AreEqual(HttpStatusCode.OK , response.StatusCode);
            Assert.That(contacts.Count, Is.GreaterThan(0));
            Assert.AreEqual("Steve", contacts[0].FirstName);
            Assert.AreEqual("Jobs", contacts[0].LastName);
        }

        [Test]
        public void Test_SerchContactsByKeyword_AssertFirstResultIsAlbertEinstein()
        {
            string keyword = "albert";

            RestRequest request = new RestRequest(ApiKey + "/contacts/search/" + keyword, Method.Get);

            RestResponse response = this.client.Execute(request);

            List<Contact> contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.GreaterThan(0));
            Assert.AreEqual("Albert", contacts[0].FirstName);
            Assert.AreEqual("Einstein", contacts[0].LastName);
        }

        [Test]
        public void Test_SerchContactsByInvalidKeyword_AssertResultsAreEmpty()
        {
            string keyword = "missing1231";

            RestRequest request = new RestRequest(ApiKey + "/contacts/search/" + keyword, Method.Get);

            RestResponse response = this.client.Execute(request);

            List<Contact> contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.EqualTo(0));
        }

        [Test]
        public void Test_CreateNewContact_InvalidData()
        {
            RestRequest requestCreate = new RestRequest(ApiKey + "/contacts", Method.Post);
            requestCreate.AddJsonBody(new { firstName = "Oleg", lastName = "Kuzmanov", phone = "0878639910"});

            RestResponse response = this.client.Execute(requestCreate);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Test_CreateNewContact_ValidData()
        {
            string testFirstName = "Oleg";
            string testLastName = "Kuzmanov";
            string testPhone = "0878639910";
            string testEmail = "ok@gmail.com";

            RestRequest requestCreate = new RestRequest(ApiKey + "/contacts", Method.Post);
            requestCreate.AddJsonBody(new { firstName = testFirstName, lastName = testLastName, phone = testPhone, email = testEmail});

            RestResponse response = this.client.Execute(requestCreate);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            RestRequest reqGetContacts = new RestRequest(ApiKey + "/contacts", Method.Get);

            RestResponse resGetContacts = this.client.Execute(reqGetContacts);

            List<Contact> contacts = JsonSerializer.Deserialize<List<Contact>>(resGetContacts.Content);

            Assert.That(resGetContacts.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts[contacts.Count - 1].FirstName, Is.EqualTo(testFirstName));
            Assert.That(contacts[contacts.Count - 1].LastName, Is.EqualTo(testLastName));
            Assert.That(contacts[contacts.Count - 1].Phone, Is.EqualTo(testPhone));
            Assert.That(contacts[contacts.Count - 1].Email, Is.EqualTo(testEmail));
        }
    }
}