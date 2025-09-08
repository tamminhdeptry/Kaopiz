namespace Kaopiz.Web.Blazorwasm
{
    public interface IHttpClientHelper
    {
        Task<TResponse?> GetAsync<TResponse>(
            string url,
            CHttpClientType requestType = CHttpClientType.Private);
        Task<TResponse?> PostAsync<TResponse, TRequest>(
            string url,
            TRequest data,
            CHttpClientType requestType = CHttpClientType.Private);
        Task PostAsync(
            string url,
            CHttpClientType requestType = CHttpClientType.Private);
    }
}