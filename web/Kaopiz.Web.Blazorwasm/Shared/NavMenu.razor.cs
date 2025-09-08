using Blazored.Toast.Services;
using Kaopiz.Shared.Contracts;
using Microsoft.AspNetCore.Components;

namespace Kaopiz.Web.Blazorwasm.Shared
{
    public partial class NavMenu
    {
        [Inject] private NavigationManager _navigationManager { get; set; } = default!;
        [Inject] private IHttpClientHelper _httpClient { get; set; } = default!;
        [Inject] private IToastService _toastService { get; set; } = default!;
        [Inject] private CustomAuthStateProvider _authStateProvider { get; set; } = default!;

        private UserDto? _userDto { get; set; } = null;


        protected override async Task OnParametersSetAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                try
                {
                    var response = await _httpClient.GetAsync<ApiResponse<UserDto>>("api/v1/users/myprofile", CHttpClientType.Private);
                    if (response != null && response.Result.Success && response.Result.Data != null)
                    {
                        _userDto = response.Result.Data;
                    }
                    else
                    {
                        _toastService.ShowError(message: string.Join(",", response?.Result.Errors.Select(s => s.Error) ?? new List<string>()));
                    }
                }
                catch (Exception ex)
                {
                    _toastService.ShowError($"Không lấy được thông tin user: {ex.Message}");
                }
            }
        }


        private async Task LogoutAsync()
        {
            try
            {
                await _httpClient.PostAsync(url: "api/v1/auth/logout", requestType: CHttpClientType.Private);
            }
            catch (Exception ex)
            {
                _toastService.ShowError(message: ex.Message);
            }
            finally
            {
                await _authStateProvider.MarkUserAsLoggedOut();
                _navigationManager.NavigateTo("/login", forceLoad: true);
            }
        }
    }
}