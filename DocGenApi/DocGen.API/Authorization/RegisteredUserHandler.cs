namespace DocGen.API.Authorization;

using Microsoft.AspNetCore.Authorization;
using DocGen.Infrastructure.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

public class RegisteredUserHandler : AuthorizationHandler<RegisteredUserRequirement>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public RegisteredUserHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RegisteredUserRequirement requirement)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var subClaim = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (subClaim is null)
            {
                context.Fail();
                return;
            }
            
            var userExists = await dbContext.User.AnyAsync(u => u.UserSub == subClaim);
            if (userExists)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
