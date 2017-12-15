using Newtonsoft.Json;

namespace SecretSantaApp.Model
{
    public class Configuration
    {
        [JsonPropertyAttribute("MaxAmount")]
        public int MaxAmount { get; set; }
        public string DeliveryDate { get; set; }
        public EmailSettings EmailSettings { get; set; }
    }
}