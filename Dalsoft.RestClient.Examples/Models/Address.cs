using Newtonsoft.Json;

namespace DalSoft.RestClient.Examples.Models
{
    public class Address
    {
        [JsonProperty("street")]
        public string Street { get; set; }
        
        [JsonProperty("suite")]
        public string Suite { get; set; }
        
        [JsonProperty("city")]
        public string City { get; set; }
        
        [JsonProperty("zipcode")]
        public string ZipCode { get; set; }
        
        [JsonProperty("geo")]
        public Geo Geo { get; set; }
    }
}