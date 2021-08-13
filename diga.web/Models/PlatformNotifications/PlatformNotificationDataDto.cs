namespace diga.web.Models.PlatformNotifications
{
    public class PlatformNotificationDataDto
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public PlatformNotificationDataDto() { }

        public PlatformNotificationDataDto(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}