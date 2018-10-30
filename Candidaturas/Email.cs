using System;
using System.Net;
using System.Net.Mail;

namespace Candidaturas
{
    public class Email
    {
        public static void SendEmail(String email, String subject, String body)
        {
            //criar mensagem
            MailMessage msg = new MailMessage
            {
                From = new MailAddress(Constants.Email)
            };

            msg.To.Add(email);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;


            //enviar mensagem
            SmtpClient smtp = new SmtpClient
            {
                Host = Constants.Host,
                Port = Constants.Port
            };

            System.Net.NetworkCredential ntwd = new NetworkCredential
            {
                UserName = Constants.Email,
                Password = Constants.Password
            };

            smtp.UseDefaultCredentials = false;
            smtp.Credentials = ntwd;
            smtp.EnableSsl = true;
            smtp.Send(msg);
        }
    }
}