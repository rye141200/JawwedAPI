using System.Security.Claims;

namespace JawwedAPI.Core.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(idClaim, out var userId))
            throw new InvalidOperationException("User ID not found in claims");
        return userId;
    }
}
