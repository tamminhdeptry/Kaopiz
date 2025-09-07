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

        private RegisterRequestDto _registerModel = new RegisterRequestDto()
        {
            Type = CUserType.User
        };

        private List<ErrorDetailDto> _errors = new List<ErrorDetailDto>();

        private async Task HandleRegisterAsync()
        {
            try
            {
                var registerResponse = await _httpClient.PostAsync<ApiResponse<RegisterResponseDto>, RegisterRequestDto>("/auth/register", _registerModel);
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
        }

        private void SelectUserType(CUserType type)
        {
            _registerModel.Type = type;
        }
    }
}