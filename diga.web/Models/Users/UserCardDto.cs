namespace diga.web.Models.Users
{
    public class UserCardDto
    {
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }        
        public string Surname { get; set; }
        public int BadReviews { get; set; }
        public int GoodReviews { get; set; }
        public int Rating { get; set; }
    }
}
