using Newtonsoft.Json;

namespace DalSoft.RestClient.Examples.Models
{
    public class Company
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("catchphrase")]
        public string CatchPhrase { get; set; }
        
        [JsonProperty("bs")]
        public string Bs { get; set; }
    }
}