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

            Console.WriteLine("*****************************************************");
            Console.WriteLine("Retrieving SecretSanta.json file");
                
            var path = @"./SecretSanta.json"; 
            var json = new JsonFileLoader().GetDataFromFile(path);

            Console.WriteLine("\t Deserializing the configuration properties");
            var config = secretSantaJsonDeserializer.GetConfiguration(json);

            Console.WriteLine("\t Deserializing participants List");
            var participants = secretSantaJsonDeserializer.GetParticipants(json);

            Console.WriteLine("\n\t Shuffling and association gifter/gifted :");
            var gifters  = secretSantaPaticipantsAssociation.RemoveDuplicateParticipants(participants);
            var secretSantaSelection = secretSantaPaticipantsAssociation.AssociateParticipantsTogether(gifters);
            
            // Email part
            Console.WriteLine("\n\t Sending emails :");  
            secretSantaEmailBuilder.BuildAndSendEmail(config, secretSantaSelection);
            
            Console.WriteLine("Secret Santa Shuffle Emailing terminated");
            Console.WriteLine("*****************************************************");
            Console.ReadLine();
        }
    }
}
