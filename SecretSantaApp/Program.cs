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
            
            //TODO : Retrieve the configuration and the list of participants from a .json file
            //var path = @"./SecretSanta.json"; 
            //var json = new JsonFileLoader().GetDataFromFile(path);

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
            
            // Email part

            var smtpClient = new SmtpClient(config.SmtpServer);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(config.EmailUserName, config.EmailPassword);

            foreach(var gifterGifted in secretSantaSelection)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(config.EmailAddress),
                    Subject = config.EmailSubject,
                    Body = String.Format(config.EmailBody, gifterGifted.Value.FirstName, gifterGifted.Value.LastName, config.MaxAmount) 
                };
                mailMessage.To.Add(gifterGifted.Key.EmailAddress);

                //smtpClient.Send(mailMessage);
            }

            Console.ReadLine();
        }
    }
}
