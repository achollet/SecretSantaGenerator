using Newtonsoft.Json;

namespace SecretSantaApp.Model
{
    public class Configuration
    {
        [JsonPropertyAttribute("MaxAmount")]
        public int MaxAmount { get; set; }

        [JsonPropertyAttribute("SenderEmailAddress")]
        public string EmailAddress { get; set; }
    }
}