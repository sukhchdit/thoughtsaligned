using System.ComponentModel.DataAnnotations;

namespace ThoughtsAligned.Models.ViewModels
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

    }
}
