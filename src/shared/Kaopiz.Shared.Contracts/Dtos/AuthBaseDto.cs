using System.ComponentModel.DataAnnotations;

namespace Kaopiz.Shared.Contracts
{
    public abstract class AuthBaseDto
    {
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters, include uppercase, lowercase, number, and special character.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required.")]
        [EnumDataType(typeof(CUserType), ErrorMessage = "Invalid user type.")]
        public CUserType Type { get; set; }
    }
}