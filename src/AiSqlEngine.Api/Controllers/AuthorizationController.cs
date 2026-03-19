using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veracity.Services.Api;

namespace AiSqlEngine.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthorizationController : ControllerBase
{
    private readonly IMy _my;

    public AuthorizationController(IMy my)
    {
        _my = my;
    }

    [Authorize]
    [HttpGet("SignOut")]
    public new IActionResult SignOut()
    {
        return SignOut(new AuthenticationProperties
                       {
                           RedirectUri = "http://localhost:5173"
                       }, CookieAuthenticationDefaults.AuthenticationScheme,
                       OpenIdConnectDefaults.AuthenticationScheme);
    }
    
    [HttpGet("SignIn")]
    public IActionResult SignIn()
    {
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = "http://localhost:5173"
        }, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> MeAsync()
    {
        try
        {
            var userInfo = await _my.Info();
            return Ok(userInfo);
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }
}