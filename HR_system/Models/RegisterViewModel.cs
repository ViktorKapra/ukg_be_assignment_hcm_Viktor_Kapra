using System.ComponentModel.DataAnnotations;

namespace HR_system.Models
{
    public class RegisterViewModel : UserViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

    }
}
