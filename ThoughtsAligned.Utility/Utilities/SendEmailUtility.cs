using ThoughtsAligned.Utility.Helpers;
using System.Net.Mail;

namespace ThoughtsAligned.Utility.Utilities
{
    public class SendEmailUtility
    {
        public async Task SendEmail(MailMessage mailMessage, AppSetting appSettings)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = appSettings.MailServer;//outgoing smtp email server
            smtp.EnableSsl = appSettings.EnableSsl;
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = appSettings.UserName;
            NetworkCred.Password = appSettings.Password;
            ////smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = Convert.ToInt32(appSettings.Port); //this is Gmail port for e-mail
            await smtp.SendMailAsync(mailMessage);//send an e-mail
        }
    }
}
