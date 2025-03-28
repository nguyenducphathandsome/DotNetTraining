using System.Text;
using Application.Settings;
using BPMaster.Services;
using Common.Controllers;
using DocumentFormat.OpenXml.Spreadsheet;
using DotNetTraining.Domains.Dtos;
using DotNetTraining.Domains.Entities;
using DotNetTraining.Requests;
using DotNetTraining.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/users")]
[ApiController]
public class UsersController : BaseV1Controller<UserService, ApplicationSetting>
{
    private readonly UserService _userService;
    public UsersController(IServiceProvider services, IHttpContextAccessor httpContextAccessor): base(services, httpContextAccessor)
    {
        this._userService = services.GetService<UserService>()!;
    }

    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var user = await _userService.GetAllUsers();
        return Success(user);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        return Success (await _userService.GetUserById(userId));
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }
        return CreatedSuccess(await _service.CreateUser(dto));
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserDto userdto)
    {
        return Success( await _userService.UpdateUser(userId, userdto) );
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _userService.DeleteUser(userId);
        return Success("delete Success");
    }

}


