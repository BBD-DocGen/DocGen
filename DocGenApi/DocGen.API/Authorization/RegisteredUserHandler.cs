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
        using (IServiceScope scope = _scopeFactory.CreateScope())
        {
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            string subClaim = context.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            if (subClaim is null)
            {
                context.Fail();
                return;
            }
            
            bool userExists = await dbContext.User.AnyAsync(user => user.UserSub == subClaim);
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
