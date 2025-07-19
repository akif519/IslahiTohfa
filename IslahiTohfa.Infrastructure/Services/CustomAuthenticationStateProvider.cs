using IslahiTohfa.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IslahiTohfa.Infrastructure.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (string.IsNullOrWhiteSpace(token))
                    return new AuthenticationState(_anonymous);

                var user = await _localStorage.GetItemAsync<UserDto>("user");
                if (user == null)
                    return new AuthenticationState(_anonymous);

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.GivenName, user.FirstName ?? ""),
                    new(ClaimTypes.Surname, user.LastName ?? ""),
                    new(ClaimTypes.Role, user.Role.ToString()),
                    new("preferred_language", user.PreferredLanguage ?? "ar")
                };

                var identity = new ClaimsIdentity(claims, "jwt");
                var principal = new ClaimsPrincipal(identity);

                return new AuthenticationState(principal);
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }

        public void NotifyUserAuthentication(UserDto user, string token)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.GivenName, user.FirstName ?? ""),
                new(ClaimTypes.Surname, user.LastName ?? ""),
                new(ClaimTypes.Role, user.Role.ToString()),
                new("preferred_language", user.PreferredLanguage ?? "ar")
            };

            var identity = new ClaimsIdentity(claims, "jwt");
            var principal = new ClaimsPrincipal(identity);

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public void NotifyUserLogout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
