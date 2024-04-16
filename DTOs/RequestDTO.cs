using System.ComponentModel.DataAnnotations;

namespace BAS_Project.DTOs
{
    public class RequestDTO
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Incorrect format! No more than 100 symbols")]
        public string? Message { get; set; }
    }
}

