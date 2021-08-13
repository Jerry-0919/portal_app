using System;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }

    }

    public class TrialRequest
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Language { get; set; }
        public string Promocode { get; set; }
        public string PacketToken { get; set; }
    }
}
