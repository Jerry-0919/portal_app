namespace diga.web.Models.Account
{
    public class PasswordForgotCheckRequestDto
    {
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
