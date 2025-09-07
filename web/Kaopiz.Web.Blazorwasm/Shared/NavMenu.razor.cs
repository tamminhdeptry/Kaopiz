using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace Kaopiz.Web.Blazorwasm.Shared
{
    public partial class NavMenu
    {
        [Inject] private NavigationManager _navigationManager { get; set; } = default!;
        [Inject] private IHttpClientHelper _httpClient { get; set; } = default!;
        [Inject] private IToastService _toastService { get; set; } = default!;
        [Inject] private CustomAuthStateProvider _authStateProvider { get; set; } = default!;

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