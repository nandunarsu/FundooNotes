using Repository.GlobalExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer
{
    public class MailSender
    {
        public static void sendMail(String ToMail, String otp)
        {
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            try
            {
                mailMessage.From = new System.Net.Mail.MailAddress("nandini1113@outlook.com", "FUNDOO NOTES");
                mailMessage.To.Add(ToMail);
                mailMessage.Subject = "Change password for Fundoo Notes";
                mailMessage.Body = "This is your otp please enter to change password " + otp;
                mailMessage.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");

               
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

               
                smtpClient.Port = 587; 

                // Enable SSL/TLS
                smtpClient.EnableSsl = true;

                string loginName = "nandini1113@outlook.com";
                string loginPassword = "Nandu!123";

                System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential(loginName, loginPassword);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = networkCredential;

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: " + ex.Message);
                throw new EmailSendingException("Failed to send email: " + ex.Message);
            }
            finally
            {
                mailMessage.Dispose();
            }
        }

    }
}

