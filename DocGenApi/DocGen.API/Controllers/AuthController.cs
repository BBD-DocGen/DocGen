using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DocGen.Infrastructure.Data;
using DocGen.Core.Entities;
using Microsoft.EntityFrameworkCore;

[Route("users")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        var name = "test user1";//User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var email = "test@email.com";//User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
        {
            return BadRequest("Missing user information in token.");
        }

        var user = await CheckAndCreateUserAsync(name, email);
        return Ok(new { Message = "Login successful", User = user });
    }

    private async Task<User> CheckAndCreateUserAsync(string name, string email)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.UserEmail == email);
        if (user == null)
        {
            user = new User { UserName = name, UserEmail = email };
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }
        return user;
    }
}