using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users;

public interface IUserContext
{
    public CurrentUser? GetCurrentUser();
}
public class UserContext(IHttpContextAccessor httpContextAccessor)  : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor?.HttpContext?.User;
        if (user == null)
            throw new InvalidOperationException("User is not authenticated");

        if (user.Identity == null || !user.Identity.IsAuthenticated)
            return null;

        // bu claimlarni Identity API orqali qo'shganmiz
        // va ularni token ichida yuboradi
        var userId = user.FindFirst(c=>c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c=>c.Type == ClaimTypes.Email)!.Value;
        var roles = user.Claims.Where(c=>c.Type == ClaimTypes.Role)!.Select(c=>c.Value);
        return new CurrentUser(userId, email, roles);
        

    }

}
