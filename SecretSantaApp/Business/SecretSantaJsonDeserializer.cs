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

            if (jArray != null)
            {
                foreach (var jToken in jArray)
                {
                    var participant = new Participant();

                    if ((string)jToken["FirstName"] != null)
                    {
                        participant.FirstName = (string)jToken["FirstName"];
                    }

                    if ((string)jToken["LastName"] != null)
                    {
                        participant.LastName = (string)jToken["LastName"];
                    }

                    if ((string)jToken["EmailAddress"] != null)
                    {
                        participant.EmailAddress = (string)jToken["EmailAddress"];
                }

                    if ((string)jToken["Team"] != null)
                    {
                        participant.Team = (string)jToken["Team"];
                    }


                    var jArrayExcludedNomminee = (JArray)jToken["ExcludedNominees"];
                    if (jArrayExcludedNomminee != null)
                    {
                        var excludedNominees = new List<People>();

                        foreach (var excludedNomminee in jArrayExcludedNomminee)
                        {
                            var people = new People();

                            if ((string)jToken["FirstName"] != null)
                            {
                                people.FirstName = (string)excludedNomminee["FirstName"];
                            }

                            if ((string)jToken["LastName"] != null)
                            {
                                people.LastName = (string)excludedNomminee["LastName"];
                            }

                            
                            if ((people.FirstName != null || people.FirstName != string.Empty) ||
                                people.LastName != null)
                            {
                                excludedNominees.Add(people);
                            }
                        }
                        participant.ExcludedNominees = excludedNominees;
                    }

                    // On n'ajoute aux participants que ceux ayant une adress email et un prénom.
                    if ((participant.FirstName != null || participant.FirstName != string.Empty) &&
                        (participant.EmailAddress != null || participant.EmailAddress != string.Empty))
                    {
                        participants.Add(participant);
                    }
                }
            }

            return participants;
        }
    }
}