using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GitHub_Models
{
    public class GithubRepo
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("full_name")]
        public string fullName { get; set; }

        [JsonPropertyName("description")]
        public string description { get; set; }

        [JsonPropertyName("private")]
        public Boolean access { get; set; }

        public GithubRepo()
        {
        }

        public GithubRepo(string name, string description, bool access)
        {
            this.name = name;
            this.description = description;
            this.access = access;
        }
    }
}
