using System.ComponentModel.DataAnnotations;

namespace LokatyWebApi.Entities
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal AccountBalance { get; set; }
    }
}
