using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dtos.Requests;
using RealEstateAgency.Application.Dtos;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using ApiMapper = RealEstateAgency.API.Mappers.ApiMapper;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountsController(
    IAccountService accountService,
    ApiMapper mapper,
    IRefreshService refreshService): ControllerBase
{
    [AllowAnonymous]
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Pong");
    }
    
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        var command = new ChangePasswordCommandDto(userId, request.OldPassword, request.NewPassword);
        var result = await accountService.ChangePasswordAsync(command);
        
        return result.Count != 0
            ? BadRequest(result)
            : NoContent();
    }
    
    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequest request)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        var command = new ChangeEmailCommandDto(userId, request.NewEmail);
        var result = await accountService.ChangeEmailAsync(command);
        
        return result.Count != 0
            ? BadRequest(result)
            : NoContent();
    }
    
    [HttpPost("change-avatar")]
    public async Task<IActionResult> ChangeAvatar([FromForm] ChangeAvatarRequest request)
    {
        var userId = User.GetUserId();
        if (request.Avatar == null || request.Avatar.Length == 0)
            return NotFound("Invalid file");

        try 
        {
            await using var stream = request.Avatar.OpenReadStream();
            var command = new ChangeAvatarCommandDto(userId, stream, request.Avatar.FileName);
        
            var url = await accountService.ChangeUserAvatarAsync(command);
        
            return Ok(new { url });
        }
        catch (Exception) { return BadRequest("Upload failed"); }
    }
    
    [HttpPost("change-phone")]
    public async Task<IActionResult> ChangePhone([FromBody] ChangePhoneRequest request)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty) return Unauthorized();

        var command = new ChangePhoneCommandDto(userId, request.NewPhone);
        var result = await accountService.ChangePhoneAsync(command);
        
        return result.Count != 0
            ? BadRequest(result)
            : NoContent();
    }
    
    [HttpGet("get-user-dto")]
    public async Task<IActionResult> GetUserDto()
    {
        var userId = User.GetUserId();
        var user = await accountService.GetUserDtoById(userId);

        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }
    
    [HttpGet("get-user-dto-by-id/{userId:guid}")]
    public async Task<IActionResult> GetUserDtoById(Guid userId)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return BadRequest();
        
        var user = await accountService.GetUserDtoById(userId);

        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }
    
    [HttpGet("get-stats")]
    public async Task<IActionResult> GetPersonalStatsByUserId()
    {
        var userId = User.GetUserId();
        var stats = await accountService.GetReportByUserId(userId);
        return Ok(stats);
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        var users = await accountService.GetAll();
        return Ok(users);
    }
    
    [HttpPost("set-role")]
    public async Task<IActionResult> SetRole([FromBody] RoleRequest request)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        await accountService.SetRole(request.UserId, request.RoleName);
        return Ok();
    }
    
    [HttpPost("set-ban")]
    public async Task<IActionResult> SetBan([FromBody] BanRequest request)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        await accountService.SetBan(request.UserId, request.BanTime);
        return Ok();
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] Guid userId)
    {
        if (!User.IsInRole(Roles.ADMIN))
            return Unauthorized();
        
        await accountService.Delete(userId);
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterRequest request)
    {
        if (request.Avatar.Length == 0)
            return BadRequest("Avatar is required");

        await using var stream = request.Avatar.OpenReadStream();
        var command = new RegisterCommandDto(
            request.Login, 
            request.Email, 
            request.Password, 
            stream, 
            request.Avatar.FileName,
            request.Name,
            request.Surname,
            request.Age,
            request.PhoneNumber
        );

        var result = await accountService.RegisterAsync(command);

        switch (result.StatusCode)
        {
            case (int)HttpStatusCode.Unauthorized:
                return Unauthorized(result.Errors);
            
            case (int)HttpStatusCode.BadRequest:
                return BadRequest(result.Errors);
        }

        refreshService.SetRefreshToken(result.RefreshToken);
        
        var response = mapper.RegisterRequestToResponse(request);
        response.Id = result.UserId;
        response.AccessToken = result.AccessToken;
    
        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var command = new LoginCommandDto(loginRequest.Login, loginRequest.Password);
        var response = await accountService.LoginAsync(command);

        switch (response.StatusCode)
        {
            case (int)HttpStatusCode.Unauthorized:
                return Unauthorized(response.Errors);
            case (int)HttpStatusCode.NotFound:
                return NotFound(response.Errors);
        }

        var result = mapper.LoginRequestToResponse(loginRequest);
        result.Id = response.UserId;
        refreshService.SetRefreshToken(response.RefreshToken);
        result.AccessToken = response.AccessToken;
        
        return Ok(result);
    }
}