using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using SecretSantaApp.Model;

namespace SecretSantaApp.Business
{
    public class SecretSantaEmailBuilder
    {
        public void BuildAndSendEmail(Configuration config, IDictionary<Participant, Participant> secretSantas)
        {              
            var emailSettings = config.EmailSettings;
            var smtpClient = new SmtpClient(emailSettings.SmtpServer);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(emailSettings.EmailUserName, emailSettings.EmailPassword);

            var index = 1;

            foreach(var gifterGifted in secretSantas)
            { 
                Console.WriteLine(String.Format("\t\t email {0}/{1} send to {2}", index.ToString(), secretSantas.Count.ToString(), gifterGifted.Key.FirstName));

                var giftedName = string.IsNullOrEmpty(gifterGifted.Value.Team) ? gifterGifted.Value.FirstName : String.Format("{0} (de l'equipe {1})", gifterGifted.Value.FirstName, gifterGifted.Value.Team).TrimEnd();
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailSettings.EmailAddress),
                    Subject = emailSettings.EmailSubject,
                    Body = String.Format(emailSettings.EmailBody, gifterGifted.Key.FirstName, giftedName, config.MaxAmount, config.DeliveryDate) 
                };
                mailMessage.To.Add(gifterGifted.Key.EmailAddress);
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.IsBodyHtml = true;

                try 
                {
                    smtpClient.Send(mailMessage);
                }
                catch (SmtpException e)
                {
                 Console.WriteLine("\t /!\\ Error: {0}", e.StatusCode);
                }

                index ++;
            }
        }
    }
}