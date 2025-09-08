using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Kaopiz.Shared.Contracts;
using Microsoft.AspNetCore.Components.Authorization;

namespace Kaopiz.Web.Blazorwasm
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private const string TokenKey = ClientAppConstant.Kaopiz_LocalStorage_App_Key;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<LoginResponseDto>(TokenKey);

            if (token == null || string.IsNullOrWhiteSpace(token.AccessToken))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken? jwtToken = null;

            try
            {
                jwtToken = handler.ReadJwtToken(token.AccessToken);
            }
            catch
            {
                await _localStorage.RemoveItemAsync(TokenKey);
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity(jwtToken.Claims, "jwtAuth");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public async Task MarkUserAsAuthenticated(LoginResponseDto dto)
        {
            await _localStorage.SetItemAsync(TokenKey, dto);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(dto.AccessToken);

            var identity = new ClaimsIdentity(jwtToken.Claims, "jwtAuth");
            var user = new ClaimsPrincipal(identity);

            var authState = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }


        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.RemoveItemAsync(TokenKey);
            var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
        }
    }
}
