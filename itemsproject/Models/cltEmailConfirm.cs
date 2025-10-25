using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace itemsproject.Models
{
    public class cltEmailConfirm : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var FMail = "";
            var FPassword = "";

            var theMsg = new MailMessage();
            theMsg.From = new MailAddress(FMail);
            theMsg.Subject = subject;
            theMsg.To.Add(email);
            theMsg.Body = $"<htm> <body> {htmlMessage} </body > </htm >";
            theMsg.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(FMail, FPassword),
                Port = 587
            };
            await smtpClient.SendMailAsync(theMsg);
        }
    }
}
