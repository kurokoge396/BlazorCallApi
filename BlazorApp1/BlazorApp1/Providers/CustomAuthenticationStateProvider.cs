using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorApp1.Providers
{
    //public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    //{
    //    private readonly IHttpContextAccessor _httpContextAccessor;

    //    public CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
    //    {
    //        _httpContextAccessor = httpContextAccessor;
    //    }

    //    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    //    {
    //        var context = _httpContextAccessor.HttpContext;
    //        var token = context?.Request.Cookies["AuthToken"]; // ← クッキー名を合わせる

    //        if (string.IsNullOrEmpty(token))
    //        {
    //            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
    //        }

    //        var handler = new JwtSecurityTokenHandler();
    //        var jwtToken = handler.ReadJwtToken(token);

    //        var identity = new ClaimsIdentity(jwtToken.Claims, "jwtAuth");
    //        var user = new ClaimsPrincipal(identity);

    //        return Task.FromResult(new AuthenticationState(user));
    //    }

    //    public void NotifyStateChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    //}

    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }

        public void AuthenticateUser(string userIdentifier)
        {
            var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, userIdentifier),
        ], "Custom Authentication");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user)));
        }
    }
}
