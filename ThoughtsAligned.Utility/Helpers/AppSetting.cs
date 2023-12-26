namespace ThoughtsAligned.Utility.Helpers
{
    public class AppSetting
    {
        public string Secret { get; set; }
        public string UploadedFilePath { get; set; }
        public string EmailFrom { get; set; }
        public string MailServer { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public bool EnableSsl { get; set; }
        public string BaseUrl { get; set; }
        public string EmailSubjectAccountActivation { get; set; }
    }
}
