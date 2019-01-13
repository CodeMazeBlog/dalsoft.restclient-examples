using Newtonsoft.Json;

namespace DalSoft.RestClient.Examples.Models
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("username")]
        public string UserName { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("address")]
        public Address Address { get; set; }
        
        [JsonProperty("phone")]
        public string Phone { get; set; }
        
        [JsonProperty("website")]
        public string Website { get; set; }
        
        [JsonProperty("company")]
        public Company Company { get; set; }
    }
}
