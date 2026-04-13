using System.Security.Claims;
using ArmadaEsports.Core.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ArmadaEsports.Web.Auth;

public class TtfAuthStateProvider(
    ProtectedSessionStorage session,
    IAuthService authService) : AuthenticationStateProvider
{
    private const string SessionKey = "ttf_user_id";

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var result = await session.GetAsync<int>(SessionKey);
            if (!result.Success || result.Value == 0)
                return Anonymous();

            var user = await authService.GetByIdAsync(result.Value);
            if (user is null || !user.IsActive)
                return Anonymous();

            var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,           user.Username),
                new Claim(ClaimTypes.Email,          user.Email),
                new Claim(ClaimTypes.Role,           user.Role),
            ], "ttf-auth");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return Anonymous();
        }
    }

    public async Task LoginAsync(int userId)
    {
        await session.SetAsync(SessionKey, userId);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task LogoutAsync()
    {
        await session.DeleteAsync(SessionKey);
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous()));
    }

    private static AuthenticationState Anonymous() =>
        new(new ClaimsPrincipal(new ClaimsIdentity()));
}
