namespace ScrapingNews.Listener
{
    public sealed class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string DeliveryMethod { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool EnableSsl { get; set; }
        public string BodyTemplate { get; set; }
        public string Subject { get; set; }
        public MailConfig MailSender { get; set; }
        public MailConfig MailReceiver { get; set; }

        public class MailConfig
        {
            public string email { get; set; }
            public string passwd { get; set; }
        }
    }
}
