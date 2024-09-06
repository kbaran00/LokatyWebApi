using System.ComponentModel.DataAnnotations;

namespace LokatyWebApi.Entities
{
    public class Deposit
    {
        [Key]
        public int DepositId { get; set; }

        public decimal Amount { get; set; }

        public decimal InterestRate { get; set; }

        public int DurationMonths { get; set; }

        public decimal EstimatedProfit { get; set; }

        // Klucz obcy dla klienta
        [Required]
        public int ClientId { get; set; }

        // Nawigacyjna właściwość do klienta
        public Client Client { get; set; }
    }
}
