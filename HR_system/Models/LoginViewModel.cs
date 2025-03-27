using HR_system.Constants;
using System.ComponentModel.DataAnnotations;

namespace HR_system.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(ValidationConsts.PASSWORD_REGEX,
            ErrorMessage = " Your password must have at least 8 characters; must contain at least:  1 uppercase character" +
            "1 lowercase character; 1 number")]
        public string Password { get; set; }

    }

}
