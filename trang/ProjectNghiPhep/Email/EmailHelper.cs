using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace ProjectNghiPhep.Email
{
    public class EmailHelper
    {
        public static bool SendMail(MailModel mailModel)
        {
            bool result;
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new NetworkCredential("hethongqlnghiphep@gmail.com", "Admin@123");
                client.EnableSsl = true;
                client.Timeout = 6000;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("hethongqlnghiphep@gmail.com");
                if (mailModel.ListToEmail != null && mailModel.ListToEmail.Count > 0) { mailModel.ListToEmail.ForEach(x => mail.To.Add(x)); }
                if (mailModel.ListCCEmail != null && mailModel.ListCCEmail.Count > 0) { mailModel.ListCCEmail.ForEach(x => mail.CC.Add(x)); }
                mail.Subject = mailModel.EmailSubject;
                mail.Body = mailModel.Body;
                mail.IsBodyHtml = true;

                client.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }

    public class MailModel
    {
        public string EmailSubject { get; set; }
        public List<string> ListToEmail { get; set; }
        public List<string> ListCCEmail { get; set; }
        public string Body { get; set; }
        public MailModel()
        {
            ListCCEmail = new List<string>();
            ListToEmail = new List<string>();
        }
    }
}