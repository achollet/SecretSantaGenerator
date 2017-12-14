using Newtonsoft.Json;

namespace SecretSantaApp.Model
{
    public class Configuration
    {
        [JsonPropertyAttribute("MaxAmount")]
        public int MaxAmount { get; set; }
        public string SmtpServer { get; set; }
        public string EmailAddress { get; set; }
        public string EmailUserName { get; set; }
        public string EmailPassword { get; set; }        
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}