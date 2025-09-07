using System.ComponentModel.DataAnnotations;

namespace Kaopiz.Shared.Contracts
{
    public class RegisterRequestDto : AuthBaseDto
    {
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string DisplayName { get; set; } = string.Empty;
    }

    public class RegisterResponseDto
    {
        public string Message { get; set; } = string.Empty;
    }
}
