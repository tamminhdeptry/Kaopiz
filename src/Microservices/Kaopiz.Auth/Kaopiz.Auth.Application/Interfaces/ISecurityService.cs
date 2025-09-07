namespace Kaopiz.Auth.Application
{
    public interface ISecurityService
    {
        string ComputeSha256Hash(string rawData);
    }
}
