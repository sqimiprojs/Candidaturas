﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Candidaturas
{
    public class Password
    {

        public string GeneratePassword()
        {
            string PasswordLength = "8";
            string NewPassword = "";

            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";


            char[] sep = {
                ','
            };

            string[] arr = allowedChars.Split(sep);


            string IDString = "";
            string temp = "";

            Random rand = new Random();

            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewPassword = IDString;

            }
            return NewPassword;
        }

        public void MailPassword(String email, String pwd)
        {
            MailMessage msg = new MailMessage
            {
                From = new MailAddress("admin@candidaturas.com")
            };

            msg.To.Add(email);
            msg.Subject = "Random Password for your Account";
            msg.Body = "Your Random password is: " + pwd;
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