using ThoughtsAligned.IService;
using ThoughtsAligned.Models.Dto;
using ThoughtsAligned.Utility.Helpers;
using ThoughtsAligned.Utility.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace ThoughtsAligned.Service
{
    public class EmailService : IEmailService
    {
        private readonly AppSetting _appSettings;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmailService(IOptions<AppSetting> appSettings, IWebHostEnvironment hostingEnvironment)
        {
            _appSettings = appSettings.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task SendEmail(MailMessage mailmessage, AppSetting _appSettings)
        {
            SendEmailUtility sendEmailUtility = new SendEmailUtility();
            await sendEmailUtility.SendEmail(mailmessage, _appSettings);
        }

        private string ReadPhysicalFile(string path)
        {
            if (_hostingEnvironment == null)
                throw new InvalidOperationException($"{nameof(EmailService)} is not initialized");

            IFileInfo fileInfo = _hostingEnvironment.ContentRootFileProvider.GetFileInfo(path);

            if (!fileInfo.Exists)
                throw new FileNotFoundException($"Template file located at \"{path}\" was not found");

            using (var fs = fileInfo.CreateReadStream())
            {
                using (var sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public async Task NewUserEmail(UserDto model)
        {
            MailMessage mailmessage = new MailMessage();
            string template = ReadPhysicalFile("Templates/Email/WelcomeMailTemplate.html");
            template = template.Replace("@Name", model.Name)
            .Replace("@Email", model.Email);

            mailmessage.To.Add(new MailAddress(model.Email));
            mailmessage.From = new MailAddress(this._appSettings.EmailFrom);
            mailmessage.Subject = this._appSettings.EmailSubjectAccountActivation;
            mailmessage.Body = template;
            mailmessage.IsBodyHtml = true;
            await SendEmail(mailmessage, _appSettings);

        }

    }
}
