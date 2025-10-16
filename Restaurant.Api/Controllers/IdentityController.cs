using Application.User.Dtos;
using Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurants.Api.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController: ControllerBase
{
    private readonly IUserDetailsService _userDetailsService;
    public IdentityController(IUserDetailsService userDetailsService)
    {
        _userDetailsService = userDetailsService;
    }

    [HttpPatch("user")]
    [Authorize]
    public async Task <IActionResult> UpdateUserDetails(UpdateUserDetailsDto updateUserDetails, CancellationToken ct)
    {
        await _userDetailsService.UpdateUserDetailsAsync(updateUserDetails, ct);
        return NoContent();
    }
}
