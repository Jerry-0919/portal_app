namespace diga.web.Models.Admin
{
    public class AdminUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Domain { get; set; }
        public string Language { get; set; }
        public double? Balance { get; set; }
        public bool VerificationRequested { get; set; }
    }
}
