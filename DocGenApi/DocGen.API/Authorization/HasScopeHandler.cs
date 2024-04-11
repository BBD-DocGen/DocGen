using Microsoft.AspNetCore.Authorization;

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
    {
        if (!context.User.HasClaim(claim => claim.Type == "scope" && claim.Issuer == requirement.Issuer))
            return Task.CompletedTask;
        
        string[] scopes = context.User.FindFirst(claim => claim.Type == "scope" && claim.Issuer == requirement.Issuer).Value.Split(' ');
        
        if (scopes.Any(s => s == requirement.Scope))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}