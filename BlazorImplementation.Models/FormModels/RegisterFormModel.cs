namespace BlazorImplementation.Models.FormModels
{
    public class RegisterFormModel
    {
        //[Required]
        //[EmailAddress]
        public string UserName { get; set; } = string.Empty;

        //[Required]
        //[RegularExpression("^.*(?=.{8,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$", ErrorMessage = "Password must be at least 8 characters long, must contain an UpperCase Letter, LowerCase Letter, a special character and a Number")]
        public string Password { get; set; } = string.Empty;

        //[Required]
        //[Compare(nameof(Password),ErrorMessage = "Password and Re Entered Password must match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        //[Required]
        //[RegularExpression(@"^(?!\s*$).{3,20}$", ErrorMessage = "The FirstName must be between 3 and 20 characters long and cannot be empty or whitespace.")]
        public string FirstName { get; set; } = string.Empty;

        //[Required]
        //[RegularExpression(@"^(?!\s*$).{3,20}$", ErrorMessage = "The LastName must be between 3 and 20 characters long and cannot be empty or whitespace.")]
        public string LastName { get; set; } = string.Empty;

        //[Required]
        //[DateAttribute(ErrorMessage = "You should be at least 20 years old and at max 80 years old.")]
        public DateTime? DateOfBirth { get; set; }

        //[Required]
        //[RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}",ErrorMessage ="Please enter a valid PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        //[Required]
        public string Role { get; set; } = string.Empty;

        public long UserId { get; set; }

        public long CountryId { get; set; }

        public long CityId { get; set; }
    }
    //public class DateAttribute : RangeAttribute
    //{
    //    public DateAttribute()
    //      : base(typeof(DateTime), DateTime.Now.AddYears(-80).ToShortDateString(), DateTime.Now.AddYears(-20).ToShortDateString()) { }
    //}
}
