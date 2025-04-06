using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Models
{
    public class Customer
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Range(19, 120, ErrorMessage = "Age must be greater than 18")] 
        public int Age { get; set; }

        [Required]
        public int Id { get; set; }
    }
}
