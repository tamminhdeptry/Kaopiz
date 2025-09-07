namespace Kaopiz.Shared.Contracts
{
    public class LoginRequestDto : AuthBaseDto
    {
        public bool RememberMe { get; set; } = false;
    }

    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}