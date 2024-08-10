using Newtonsoft.Json;

namespace UserPortal.Entities
{
    public class Token
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("totalSupply")]
        public int TotalSupply { get; set; }
    }
}
