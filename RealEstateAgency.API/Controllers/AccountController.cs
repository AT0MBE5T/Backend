using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.API.Dto;
using RealEstateAgency.API.Mapper;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController(UserManager<User> userManager,
    IAccountService accountService,
    ApiMapper mapper,
    IRefreshService refreshService,
    IJwtService jwtService,
    RoleManager<IdentityRole<Guid>> roleManager,
    IAuditService auditService,
    SignInManager<User> signInManager,
    IImageService imageService): ControllerBase
{
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            return NotFound();
        }
        
        var res = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        return Ok(res.Errors);
    }
    
    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequestDto request)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            return NotFound();
        }
        
        var res = await userManager.ChangeEmailAsync(user, request.NewEmail,  await userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail));
        return Ok(!res.Errors.Any());
    }
    
    [HttpPost("change-avatar")]
    public async Task<IActionResult> ChangeAvatar([FromForm] ChangeAvatarRequestDto request)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null || request.Avatar is null || request.Avatar.Length == 0)
            return NotFound();
        
        await imageService.DeleteAvatarAsync(request.UserId);
        var newAvatar = await imageService.UploadImageAsync(request.Avatar);
        
        if (newAvatar.StatusCode != HttpStatusCode.OK)
            return BadRequest();
        
        var url = newAvatar.SecureUrl.ToString();
        var publicId = newAvatar.PublicId;
        var res = await accountService.UpdateUserAvatarAsync(user.Id, url, publicId);
        
        return res
            ? Ok(new { url })
            : BadRequest();
    }
    
    [HttpPost("change-phone")]
    public async Task<IActionResult> ChangePhone([FromBody] ChangePhoneRequestDto request)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            return NotFound();
        }
        
        var res = await userManager.ChangePhoneNumberAsync(user, request.NewPhone, await userManager.GenerateChangePhoneNumberTokenAsync(user, request.NewPhone));
        return Ok(res.Errors);
    }
    
    [HttpGet("get-user-dto-by-id")]
    public async Task<IActionResult> GetUserDtoById()
    {
        var userId = User.GetUserId();
        var user = await accountService.GetUserDtoById(userId);

        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }
    
    [HttpGet("get-stats-by-user-id")]
    public async Task<IActionResult> GetPersonalStatsByUserId()
    {
        var userId = User.GetUserId();
        var stats = await accountService.GetPersonalStatsByUserId(userId);
        return Ok(stats);
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterRequestDto registerRequest)
    {
        var photo = registerRequest.Avatar;
        
        if (photo.Length == 0)
            return BadRequest("No avatar uploaded");
        
        var existingUser = await userManager.FindByNameAsync(registerRequest.Login);
        
        if (existingUser != null)
            return Unauthorized("User with this login already exists");
        
        var imageResult = await imageService.UploadImageAsync(registerRequest.Avatar);
        
        if (imageResult.Error != null) 
            throw new Exception(imageResult.Error.Message);

        var userId = Guid.NewGuid();
        var user = mapper.RegisterRequestDtoToUser(registerRequest, DateTime.UtcNow);

        user.Avatar = imageResult.SecureUrl.ToString();
        user.PublicAvatarId = imageResult.PublicId;
        
        var response = mapper.RegisterRequestToResponse(registerRequest);
        response.Id = userId;
        var result = await userManager.CreateAsync(user, registerRequest.Password);
        
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid> { Name = "User" });
        }
        
        await userManager.AddToRoleAsync(user, "User");
        var refreshToken = await refreshService.GenerateRefreshToken(user.Id);
        var accessToken = await jwtService.GenerateAccessToken(user);
        SetRefreshTokenCookie(refreshToken);
        response.AccessToken = accessToken;
        var auditDto = new AuditDto
        {
            ActionId = Guid.Parse(AuditAction.Register),
            UserId = user.Id,
            Details = $"User {user.UserName} registered"
        };
        await auditService.InsertAudit(auditDto);
        
        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var user = await userManager.FindByNameAsync(loginRequest.Login);
        if (user == null)
        {
            return NotFound("Login not found");
        }

        var response = mapper.LoginRequestToResponse(loginRequest);
        response.Id = user.Id;
        var result = await signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized("Incorrect password");
        }

        var refreshToken = await refreshService.GenerateRefreshToken(user.Id);
        var accessToken = await jwtService.GenerateAccessToken(user);
        SetRefreshTokenCookie(refreshToken);
        response.AccessToken = accessToken;

        var auditDto = new AuditDto
        {
            ActionId = Guid.Parse(AuditAction.Login),
            UserId = user.Id,
            Details = $"User {user.UserName} logged in"
        };
        await auditService.InsertAudit(auditDto);
        
        return Ok(response);
    }
    
    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = refreshService.GetCookieOptions();
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}