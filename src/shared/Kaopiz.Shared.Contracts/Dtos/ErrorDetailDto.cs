namespace Kaopiz.Shared.Contracts
{
    public class ErrorDetailDto
    {
        public CErrorScope ErrorScope { get; set; }
        public string Field { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
    public enum CErrorScope
    {
        None = 0,
        Field = 1,
        FormSummary = 2,
        PageSumarry = 3,
        RedirectPage = 4,
        Global = 5,
        RedirectToLoginPage = 6
    }
}
