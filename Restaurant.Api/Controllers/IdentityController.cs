using Application.AssignUsers;
using Application.Users;
using Application.Users.Dtos;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurants.Api.Controllers;

[ApiController]
[Route("api/identity")]
[Authorize]
public class IdentityController: ControllerBase
{
    private readonly IUserDetailsService _userDetailsService;
    private readonly IAssignUserService _assignUserService;
    public IdentityController(IUserDetailsService userDetailsService, IAssignUserService assignUserService)
    {
        _userDetailsService = userDetailsService;
        _assignUserService = assignUserService;

    }

    [HttpPatch("user")]
    public async Task <IActionResult> UpdateUserDetails(UpdateUserDetailsDto updateUserDetails, CancellationToken ct)
    {
        await _userDetailsService.UpdateUserDetailsAsync(updateUserDetails, ct);
        return NoContent();
    }
    [HttpPost("userRole")]
    [Authorize(Roles =UserRoles.Admin)]
    public async Task<IActionResult> AssignUserRole(AssignUserRole newRole, CancellationToken ct)
    {
        await _assignUserService.AssignUserRoleAsync(newRole, ct);
        return NoContent();
    }
    [HttpDelete("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> UnassignUserRole(UnassignUserRole newRole, CancellationToken ct)
    {
        await _assignUserService.UnassignUserRoleAsync(newRole, ct);
        return NoContent();
    }
}
