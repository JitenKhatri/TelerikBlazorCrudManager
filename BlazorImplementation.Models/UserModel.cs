namespace BlazorImplementation.Models
{
    public class UserModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty ;
        public long UserId { get; set; }

        public int TotalCount { get; set; }

        public long CountryId { get; set; }

        public long CityId { get; set; }

        public string CityName { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
    }
}
