using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalExam.APITests
{
    public class Board
    {
        [JsonPropertyName("id")]
        public long id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }
    }
}
