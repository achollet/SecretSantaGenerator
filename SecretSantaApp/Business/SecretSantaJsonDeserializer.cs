using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SecretSantaApp.Model;

namespace SecretSantaApp.Business
{
    public class SecretSantaJsonDeserializer
    {
        public Configuration GetConfiguration(string json)
        {
            var jObject = JObject.Parse(json);
            var configuration = new Configuration
            {
                MaxAmount = (int)jObject["configuration"]["MaxAmount"],
                DeliveryDate = (string)jObject["configuration"]["DeliveryDate"]
            };
            
            var emailSetting = new EmailSettings()
            {
                SmtpServer = (string)jObject["configuration"]["EmailSettings"]["SmtpServer"],
                EmailUserName = (string)jObject["configuration"]["EmailSettings"]["EmailUserName"],
                EmailPassword = (string)jObject["configuration"]["EmailSettings"]["EmailPassword"],
                EmailAddress = (string)jObject["configuration"]["EmailSettings"]["EmailAddress"],
                EmailSubject = (string)jObject["configuration"]["EmailSettings"]["EmailSubject"],
                EmailBody = (string)jObject["configuration"]["EmailSettings"]["EmailBody"]
            };

            configuration.EmailSettings = emailSetting;
            return configuration;
        }

        public List<Participant> GetParticipants(string json)
        {
            var participants = new List<Participant>();
            var jObject = JObject.Parse(json);
            var jArray = (JArray)jObject["participants"];

            foreach(var jToken in jArray)
            {
                var participant = new Participant
                {
                    FirstName = (string)jToken["FirstName"],
                    LastName = (string)jToken["LastName"],
                    EmailAddress = (string)jToken["EmailAddress"],
                    Team = (string)jToken["Team"]
                };

                var jArrayExcludedNomminee = (JArray)jToken["ExcludedNominees"];
                if (jArrayExcludedNomminee != null)
                {
                    var excludedNominees = new List<People>();

                    foreach(var excludedNomminee in jArrayExcludedNomminee)
                    {
                        var people = new People
                        {
                            FirstName = (string)excludedNomminee["FirstName"],
                            LastName = (string)excludedNomminee["LastName"]
                        };
                        excludedNominees.Add(people);
                    }
                    participant.ExcludedNominees = excludedNominees;
                }

                participants.Add(participant);
            }

            return participants;
        }
    }
}