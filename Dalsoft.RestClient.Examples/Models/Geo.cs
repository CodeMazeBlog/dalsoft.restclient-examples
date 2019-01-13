using Newtonsoft.Json;

namespace DalSoft.RestClient.Examples.Models
{
    public class Geo
    {
        [JsonProperty("lat")]
        public string Lat { get; set; }
        
        [JsonProperty("lng")]
        public string Lng { get; set; }
    }
}