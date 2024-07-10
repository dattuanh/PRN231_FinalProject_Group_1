using System.ComponentModel.DataAnnotations;

namespace PRN231_FinalProject_API.DTOs.Debts
{
    public class DebtsDTO
    {
        [Required]
        public int? UserId { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The Amount field must be a positive number.")]
        public decimal? Amount { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "The InterestRate field must be a percentage between 0 and 100.")]
        public decimal? InterestRate { get; set; }

        public string? Description { get; set; }
    }
}
