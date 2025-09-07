using System.Net;
using Blazored.Toast.Services;
using Kaopiz.Shared.Contracts;
using Microsoft.AspNetCore.Components;

namespace Kaopiz.Web.Blazorwasm.Pages.Login
{
    public partial class LoginPage
    {
        [Inject] private NavigationManager _navigationManager { get; set; } = default!;
        [Inject] private IHttpClientHelper _httpClient { get; set; } = default!;
        [Inject] private IToastService _toastService { get; set; } = default!;
        [Inject] private CustomAuthStateProvider _authProvider { get; set; } = default!;

        private LoginRequestDto _loginRequestDto = new LoginRequestDto { Type = CUserType.User };

        private void SelectUserType(CUserType type)
        {
            _loginRequestDto.Type = type;
        }


        private async Task HandleLoginAsync()
        {
            try
            {
                var loginResponse = await _httpClient.PostAsync<ApiResponse<LoginResponseDto>, LoginRequestDto>(url: "api/v1/auth/login",
                    data: _loginRequestDto, requestType: CHttpClientType.Public);
                if (loginResponse != null && loginResponse.StatusCode == HttpStatusCode.OK
                    && loginResponse.Result.Success && loginResponse.Result.Data != null)
                {
                    await _authProvider.MarkUserAsAuthenticated(token: loginResponse.Result.Data.AccessToken);
                    _navigationManager.NavigateTo("/");
                }
                else
                {
                    _toastService.ShowError(message: $"{string.Join(",", loginResponse?.Result.Errors.Select(s => s.Error) ?? new List<string>())}");
                }
            }
            catch (Exception ex)
            {
                _toastService.ShowError(ex.Message);
            }
        }
    }
}