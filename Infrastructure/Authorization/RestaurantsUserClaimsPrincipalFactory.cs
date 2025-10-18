using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authorization;

public class RestaurantsUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IOptions<IdentityOptions> _optionsAccessor;

    public RestaurantsUserClaimsPrincipalFactory(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options ):base (userManager, roleManager, options)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _optionsAccessor = options;
    }
    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var id = await GenerateClaimsAsync(user); // Identity default claimlarini yaratadi

        if (user.Nationality != null)
        {
            id.AddClaim(new Claim("Nationality", user.Nationality));
        }

        //if (user.DateOfBirth != null)
        //{
        //    id.AddClaim(new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")));
        //}

        return new ClaimsPrincipal(id);
    }

}
