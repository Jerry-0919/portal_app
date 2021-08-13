namespace diga.web.Models.Account
{
    public class SignUpRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Domain { get; set; }
        public string Language { get; set; }
    }
}
