using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using SecretSantaApp.Model;

namespace SecretSantaApp.Business
{
    public class SecretSantaEmailBuilder
    {
        public void BuildAndSendEmail(Configuration config, IDictionary<Participant, People> secretSantas)
        {              
            var emailSettings = config.EmailSettings;
            var smtpClient = new SmtpClient(emailSettings.SmtpServer);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(emailSettings.EmailUserName, emailSettings.EmailPassword);

            var index = 1;

            foreach(var gifterGifted in secretSantas)
            { 
                Console.WriteLine(String.Format("\t\t email {0}/{1}", index.ToString(), secretSantas.Count.ToString()));

                var giftedName = String.Format("{0} {1}", gifterGifted.Value.FirstName, gifterGifted.Value.LastName).TrimEnd();
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailSettings.EmailAddress),
                    Subject = emailSettings.EmailSubject,
                    Body = String.Format(emailSettings.EmailBody, giftedName, config.MaxAmount, config.DeliveryDate) 
                };
                mailMessage.To.Add(gifterGifted.Key.EmailAddress);
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.IsBodyHtml = true;

                // Console.WriteLine(String.Format("From : {0}", mailMessage.From));
                // Console.WriteLine(String.Format("To : {0}", mailMessage.To.ToString()));
                // Console.WriteLine(String.Format("Subject : {0}", mailMessage.Subject));
                // Console.WriteLine(String.Format("Body : {0}", mailMessage.Body));

                // Console.WriteLine();
                // Console.WriteLine("---------------------------------------------------");
                // Console.WriteLine();

                //try 
                //{
                //  smtpClient.Send(mailMessage);
                //}
                //catch (SmtpException e)
                //{
                //  Console.WriteLine("\t /!\\ Error: {0}", e.StatusCode);
                //}

                index ++;
            }
        }
    }
}