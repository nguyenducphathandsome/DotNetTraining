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
        return Success(await _userService.AuthenticateAsync(request));
    }
}
