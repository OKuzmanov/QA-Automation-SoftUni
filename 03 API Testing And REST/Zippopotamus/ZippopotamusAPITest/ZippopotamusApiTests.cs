using NUnit.Framework;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZippopotamusAPITest
{
    public class DataDrivenApiTests
    {
        private RestClient client;

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient("https://api.zippopotam.us");
        }

        [TestCase("IT", "00100", "Roma")]
        [TestCase("GB", "WC2N", "London")]
        [TestCase("US", "90001", "Los Angeles")]
        [TestCase("DE", "01067", "Dresden")]
        [TestCase("GB", "B1", "Birmingham")]
        public async Task Test_GetCountryInfo(string code, string zipCode, string locationName)
        {
            RestRequest request = new RestRequest(code + "/" + zipCode, Method.Get);

            RestResponse response = await this.client.ExecuteAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            //CountryInfo countryInfo = new SystemTextJsonSerializer().Deserialize<CountryInfo>(response);

            CountryInfo countryInfo = JsonSerializer.Deserialize<CountryInfo>(response.Content);

            Assert.AreEqual(code, countryInfo.countryabbreviation);
            Assert.AreEqual(zipCode, countryInfo.postcode);
            Assert.AreEqual(locationName, countryInfo.places[0].placename);
        }
    }
}