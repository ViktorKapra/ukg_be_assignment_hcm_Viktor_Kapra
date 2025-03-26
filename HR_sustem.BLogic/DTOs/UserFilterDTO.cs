namespace HR_system.BLogic.DTOs
{
    public class UserFilterDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public decimal UpperSalaryBound { get; set; } = decimal.MaxValue;
        public decimal LowerSalaryBound { get; set; } = decimal.MinValue;
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}
