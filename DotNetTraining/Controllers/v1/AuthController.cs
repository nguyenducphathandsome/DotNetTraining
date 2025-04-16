using Microsoft.AspNetCore.Mvc;
using DotNetTraining.Services;
using DotNetTraining.Requests;
using Common.Controllers;
using Application.Settings;

[Route("api/auth")]
[ApiController]
public class AuthController : BaseV1Controller<UserService, ApplicationSetting>
{
    private readonly UserService _userService;

    public AuthController(IServiceProvider services, IHttpContextAccessor httpContextAccessor)
        : base(services, httpContextAccessor)
    {
        this._userService = services.GetService<UserService>()!;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _service.AuthenticateAsync(request);
        return Ok(new
        {
            accessToken = result.accessToken,
            refreshToken = result.refreshToken
        });
    }

    [HttpPost("Refresh-Token")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest model)
    {
        var (accessToken, refreshToken) = await _service.RefreshTokenAsync(model);
        return Ok(new { accessToken, refreshToken });
    }

    [HttpPost("logout/{email}")]
    public async Task<IActionResult> Logout(string email)
    {
        await _service.LogoutAsync(email);
        return Ok("Logged out.");
    }
}
