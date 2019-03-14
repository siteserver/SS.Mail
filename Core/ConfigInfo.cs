namespace SS.Mail.Core
{
    public class ConfigInfo
    {
        public bool IsEnabled { get; set; } = true;

        public string Provider { get; set; } = MailProvider.Default.Value;

        public string Host { get; set; }

        public bool IsEnableSsl { get; set; }

        public int Port { get; set; } = 25;

        public string Address { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }
    }
}