using HR_system.Constants;
using System.ComponentModel.DataAnnotations;


namespace HR_system.Models
{
    public class UserViewModel
    {

        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [RegularExpression(ValidationConsts.NAME_REGEX,
        ErrorMessage = "Only letters are allowed.")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [RegularExpression(ValidationConsts.NAME_REGEX,
        ErrorMessage = "Only letters are allowed.")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Job title is required")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid salary")]
        public decimal Salary { get; set; }

        [Required]
        [RegularExpression(ValidationConsts.ROLES_REGEX, ErrorMessage = "Invalid role")]
        public string Role { get; set; }
    }
}
