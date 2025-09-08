using System.Net;
using Blazored.Toast.Services;
using Kaopiz.Shared.Contracts;
using Microsoft.AspNetCore.Components;

namespace Kaopiz.Web.Blazorwasm.Pages.Register
{
    public partial class RegisterPage
    {
        [Inject] private IHttpClientHelper _httpClient { get; set; } = default!;
        [Inject] private IToastService _toastService { get; set; } = default!;
        [Inject] private NavigationManager _navigationManager { get; set; } = default!;
        [Inject] private CustomAuthStateProvider _authProvider { get; set; } = default!;

        private bool _isLoading { get; set; } = false;

        private RegisterRequestDto _registerModel = new RegisterRequestDto()
        {
            Type = CUserType.User
        };

        private List<ErrorDetailDto> _errors = new List<ErrorDetailDto>();

        protected override async Task OnInitializedAsync()
        {
            var state = await _authProvider.GetAuthenticationStateAsync();
            if (state.User.Identity?.IsAuthenticated == true)
            {
                _navigationManager.NavigateTo("/", forceLoad: true);
            }
        }

        private async Task HandleRegisterAsync()
        {
            try
            {
                _isLoading = true;
                var registerResponse = await _httpClient.PostAsync<ApiResponse<RegisterResponseDto>, RegisterRequestDto>("api/v1/auth/register", _registerModel);
                if (registerResponse != null && registerResponse.StatusCode == HttpStatusCode.Created
                    && registerResponse.Result.Data != null && registerResponse.Result.Success)
                {
                    _toastService.ShowSuccess(message: registerResponse.Result.Data.Message);
                    await Task.Delay(5000);
                    _navigationManager.NavigateTo("/login");
                }
                else
                {
                    _errors = registerResponse?.Result.Errors ?? new List<ErrorDetailDto>();
                }
            }
            catch (Exception ex)
            {
                _toastService.ShowSuccess(message: ex.Message);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void SelectUserType(CUserType type)
        {
            _registerModel.Type = type;
        }
    }
}