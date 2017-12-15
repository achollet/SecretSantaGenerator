using System;
using System.Linq;
using System.Collections.Generic;
using SecretSantaApp.Model;
using SecretSantaApp.Business;
using System.Net.Mail;
using System.Net;

namespace SecretSantaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var secretSantaJsonDeserializer = new SecretSantaJsonDeserializer();
            var secretSantaPaticipantsAssociation = new SecretSantaPaticipantsAssociation();
            var secretSantaEmailBuilder = new SecretSantaEmailBuilder();

            var path = @"./SecretSanta.json"; 
            var json = new JsonFileLoader().GetDataFromFile(path);

            var config = secretSantaJsonDeserializer.GetConfiguration(json);

            var participants = secretSantaJsonDeserializer.GetParticipants(json);

            participants = new List<Participant>
            {
                new Participant{LastName = "Morningstar", FirstName = "Lucifer", EmailAddress = "LuciferMorningstar@gmail.com", Team = "2" },
                new Participant{LastName = "Prinkster", FirstName = "Joanna", EmailAddress = "JoannaPrinkster@gmail.com", Team = "1" },
                new Participant{LastName = "Duch", FirstName = "Dan", EmailAddress = "DanDush@gmail.com", Team = "3" },
                new Participant{LastName = "", FirstName = "Amenedial", EmailAddress = "Amenedial@gmail.com", Team = "1", ExcludedNominees = new List<People>{new People { FirstName = "Lucifer", LastName = "Morningstar"} }},
                new Participant{LastName = "", FirstName = "Amenedial", EmailAddress = "Amenedial@gmail.com", Team = "1", ExcludedNominees = new List<People>{new People { FirstName = "Lucifer", LastName = "Morningstar"} }}
            };

            var gifters  = secretSantaPaticipantsAssociation.RemoveDuplicateParticipants(participants);
            var secretSantaSelection = secretSantaPaticipantsAssociation.AssociateParticipantsTogether(gifters);
            
            // Email part
            secretSantaEmailBuilder.BuildAndSendEmail(config, secretSantaSelection);
            
            Console.ReadLine();
        }
    }
}
