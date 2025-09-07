using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Kaopiz.Shared.Contracts;
using Newtonsoft.Json;

namespace Kaopiz.Web.Blazorwasm
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILocalStorageService _localStorageService;

        public HttpClientHelper(
            HttpClient httpClient,
            IConfiguration configuration,
            ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _localStorageService = localStorageService;
        }

        public async Task<TResponse?> PostAsync<TResponse, TRequest>(
            string url,
            TRequest data,
            CHttpClientType requestType = CHttpClientType.Private)
        {
            await PrepareClientAsync(requestType);

            var response = await _httpClient.PostAsJsonAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResponse>();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"API Error ({response.StatusCode}): {errorContent}");
        }

        public async Task PostAsync(string url, CHttpClientType requestType = CHttpClientType.Private)
        {
            await PrepareClientAsync(requestType);

            var response = await _httpClient.PostAsync(url, null);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"API Error ({response.StatusCode}): {errorContent}");
            }
        }

        private async Task PrepareClientAsync(CHttpClientType httpClientType)
        {
            _httpClient.BaseAddress = new Uri(_configuration["ServerHost:AuthService"] ?? string.Empty);

            var jsonData = await _localStorageService.GetItemAsStringAsync(ClientAppConstant.Kaopiz_LocalStorage_App_Key);
            var loginResponseDto = string.IsNullOrEmpty(jsonData) ? null : JsonConvert.DeserializeObject<LoginResponseDto>(jsonData);

            if (loginResponseDto != null && httpClientType == CHttpClientType.Private)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    ClientAppConstant.Kaopiz_API_Auth_Header_Scheme,
                    loginResponseDto.AccessToken);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }
    }
}