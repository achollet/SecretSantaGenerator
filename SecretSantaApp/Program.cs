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
            var secretSantaPaticipantsAssociation = new SecretSantaPaticipantsAssociation();
            var secretSantaEmailBuilder = new SecretSantaEmailBuilder();
            //TODO : Retrieve the configuration and the list of participants from a .json file
            //var path = @"./SecretSanta.json"; 
            //var json = new JsonFileLoader().GetDataFromFile(path);

            var config = new Configuration
            {
                MaxAmount = 10,
                DeliveryDate = "21 Janvier 2018",
                EmailSettings = new EmailSettings{
                    SmtpServer = "smtpserver",
                    EmailUserName = "username",
                    EmailPassword = "password",
                    EmailAddress ="SecretSantaCheckout@gmail.com", 
                    EmailSubject = "Secret Santa du Checkout",
                    EmailBody = "Tu es le Secret Santa de <b>{0} </b>.\n Le cadeau que tu lui offriras ne doit pas dépasser une valeur de <b>{1}\u20AC</b> (frais de port non-inclus).\n La remise du cadeau se fera le <b>{2}</b>. \n\n Oh oh oh! \n Joyeux Noël!"
                }
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
            
            // Email part
            secretSantaEmailBuilder.BuildAndSendEmail(config, secretSantaSelection);
            
            Console.ReadLine();
        }
    }
}
