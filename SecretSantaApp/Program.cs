using System;
using System.Linq;
using System.Collections.Generic;
using SecretSantaApp.Model;
using SecretSantaApp.Business;

namespace SecretSantaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var secretSantaPaticipantsAssociation = new SecretSantaPaticipantsAssociation();
            //TODO : Retrieve the configuration and the list of participants from a .json file
            var config = new Configuration
            {
                MaxAmount = 10,
                EmailAddress = "SecretSantaCheckout@gmail.com"
            };

            var participants = new List<Participant>
            {
                new Participant{LastName = "Morningstar", FirstName = "Lucifer", EmailAddress = "LuciferMorningstar@gmail.com", Team = 2 },
                new Participant{LastName = "Prinkster", FirstName = "Joanna", EmailAddress = "JoannaPrinkster@gmail.com", Team = 1 },
                new Participant{LastName = "Duch", FirstName = "Dan", EmailAddress = "DanDush@gmail.com", Team = 3 },
                new Participant{LastName = "", FirstName = "Amenedial", EmailAddress = "Amenedial@gmail.com", Team = 1, ExcludedNominees = new List<People>{new People { FirstName = "Lucifer", LastName = "Morningstar"} }},
                new Participant{LastName = "", FirstName = "Amenedial", EmailAddress = "Amenedial@gmail.com", Team = 1, ExcludedNominees = new List<People>{new People { FirstName = "Lucifer", LastName = "Morningstar"} }}
            };

            var gifters  = secretSantaPaticipantsAssociation.RemoveDuplicateParticipants(participants);
            var secretSantaSelection = secretSantaPaticipantsAssociation.AssociateParticipantsTogether(gifters);
            var secretSantaSelectionV2 = secretSantaPaticipantsAssociation.AssociateParticipantsTogetherV2(gifters);
            
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Associtation gifter-gifted terminated");
            Console.WriteLine("-------------------------------------");

            foreach(var gifterGiftedPair in secretSantaSelection)
            {
                Console.WriteLine(String.Format("{0} {1} offers a gift to {2} {3} of a maximum value of {4}", gifterGiftedPair.Key.FirstName, gifterGiftedPair.Key.LastName, gifterGiftedPair.Value.FirstName, gifterGiftedPair.Value.LastName, config.MaxAmount));
            }

            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Associtation gifter-gifted V2 terminated");
            Console.WriteLine("-------------------------------------");

            foreach(var gifterGiftedPair in secretSantaSelectionV2)
            {
                Console.WriteLine(String.Format("{0} {1} offers a gift to {2} {3} of a maximum value of {4}", gifterGiftedPair.Key.FirstName, gifterGiftedPair.Key.LastName, gifterGiftedPair.Value.FirstName, gifterGiftedPair.Value.LastName, config.MaxAmount));
            }

            //var path = @"./SecretSanta.json"; 
            
            //var json = new JsonFileLoader().GetDataFromFile(path);

            //var config = JsonConvert.DeserializeObject<Configuration>(json);
            Console.ReadLine();
        }
    }
}
