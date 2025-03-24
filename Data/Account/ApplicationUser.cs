using Microsoft.AspNetCore.Identity;

namespace Data.Account
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public decimal? Salary { get; set; }
        public ApplicationUser() : base() { }

    }
}
