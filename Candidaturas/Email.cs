using System;
using System.Net;
using System.Net.Mail;

namespace Candidaturas
{
    public class Email
    {
        public static void MailPassword(String email, String pwd)
        {
            MailMessage msg = new MailMessage
            {
                From = new MailAddress("admin@candidaturas.com")
            };

            msg.To.Add(email);
            msg.Subject = "Password de Acesso";
            msg.Body = "A password de acesso para a sua conta é a seguinte: " + pwd;
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