using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class SendEmail
    {
        public void SendMail(string to, string subject, string body)
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(Configuration.MAIL);
            mail.Subject = subject;
            mail.Body = body;
            if (body == "")
            {
                throw new ArgumentException("This is an empty mail.");
            }
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.Gmail.com";
            //smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(Configuration.MAIL, Configuration.MAIL_PASSWORD);
            smtp.EnableSsl = true;
            //smtp.Port = 587;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception)
            {

                
            }
            

        }
    }
}
