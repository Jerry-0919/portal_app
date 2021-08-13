using System.Text.RegularExpressions;

namespace diga.web.Helpers
{
    public static class RegexValidator
    {
        public static bool ContainsLink(string text)
        {
            Regex linkRegex = new Regex(@"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})");
            return linkRegex.IsMatch(text);
        }

        public static bool ContainsEmail(string text)
        {
            Regex emailRegex = new Regex("(([^<>()[\\]\\\\.,;:\\s@\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\\\"]+)*)|(\\\".+\\\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))");
            return emailRegex.IsMatch(text);
        }

        public static bool ContainsContact(string text)
        {
            Regex contactRegex = new Regex(@"(@[a-z,A-Z])\w+");
            return contactRegex.IsMatch(text);
        }

        public static bool ContainsPhoneNumber(string text)
        {
            Regex phoneNumberRegex = new Regex(@"1?\W*([0-9][0-9][0-9])\W*([0-9][0-9]{2})\W*([0-9]{4})(\se?x?t?(\d*))?");
            return phoneNumberRegex.IsMatch(text);
        }
    }
}
