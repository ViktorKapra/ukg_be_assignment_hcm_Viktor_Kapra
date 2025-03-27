using HR_system.Constants;
using System.ComponentModel.DataAnnotations;

namespace HR_system.Models
{
    public class RegisterViewModel
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

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(ValidationConsts.PASSWORD_REGEX,
            ErrorMessage = " Your password must have at least 8 characters; must contain at least:  1 uppercase character" +
            "1 lowercase character; 1 number")]
        public string Password { get; set; }

    }
}
