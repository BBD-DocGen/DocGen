using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using DocGen.Infrastructure.Data;
using DocGen.Core.Entities;
using Microsoft.EntityFrameworkCore;

[Route("users")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _userInfoEndpoint = "https://dev-2f8sdpf6pls655l7.us.auth0.com/userinfo";

    public AuthController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        string accessToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (string.IsNullOrEmpty(accessToken))
        {
            return BadRequest("Missing access token.");
        }
        
        var userSub = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        if (string.IsNullOrEmpty(userSub))
        {
            return BadRequest("Missing user identifier in token.");
        }

        var userInfo = await GetUserInfoAsync(accessToken);
        if (userInfo == null)
        {
            return BadRequest("Failed to retrieve user information.");
        }

        var user = await CheckAndCreateUserAsync(userInfo.Name, userInfo.Email, userSub);
        return Ok(new { Message = "Login successful", User = user });
    }

    private async Task<UserInfo> GetUserInfoAsync(string accessToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.GetAsync(_userInfoEndpoint);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserInfo>(
            content, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
    }

    private async Task<User> CheckAndCreateUserAsync(string name, string email, string userSub)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.UserSub == userSub);
        if (user == null)
        {
            user = new User { UserName = name, UserEmail = email, UserSub = userSub };
            _context.User.Add(user);
        }
        else
        {

            user.UserSub = userSub;
            user.UserName = name;
        }
        await _context.SaveChangesAsync();
        return user;
    }

    private class UserInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
