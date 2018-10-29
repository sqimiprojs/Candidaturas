using System;
using System.Net;
using System.Net.Mail;

namespace Candidaturas
{
    public class Email
    {
        public static void SendEmail(String email, String subject, String body)
        {
            MailMessage msg = new MailMessage
            {
                From = new MailAddress("admin@candidaturas.com")
            };

            msg.To.Add(email);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;

            SmtpClient smt = new SmtpClient
            {
                Host = Constants.Host,
                Port = Constants.Port
            };

            System.Net.NetworkCredential ntwd = new NetworkCredential
            {
                UserName = Constants.Email,
                Password = Constants.Password
            };

            smt.UseDefaultCredentials = false;
            smt.Credentials = ntwd;
            smt.EnableSsl = true;
            smt.Send(msg);
        }
    }
}