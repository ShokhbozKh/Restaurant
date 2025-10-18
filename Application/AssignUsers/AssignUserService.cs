using Application.Users;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.AssignUsers;

public class AssignUserService: IAssignUserService
{
    private readonly IUserContext _userContext;
    private readonly ILogger _logger;
    private readonly IUserStore<User> _userStore;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AssignUserService(IUserContext userContext, ILogger<UserDetailsService> logger, IUserStore<User> userStore,
        UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userContext = userContext;
        _logger = logger;
        _userStore = userStore;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task AssignUserRoleAsync(AssignUserRole role, CancellationToken ct)
    {
        _logger.LogInformation($"Assigning user role: {role.RoleName}");

        var user = await _userManager.FindByEmailAsync(role.UserEmail);
        if (user == null)
        {
            _logger.LogWarning("User with email {UserEmail} not found", role.UserEmail);
            throw new NotFoundException("User not found");
        }
        var roleExists = await _roleManager.FindByNameAsync(role.RoleName);
        if (roleExists == null)
        {
            _logger.LogWarning("Role {RoleName} does not exist", role.RoleName);
            throw new NotFoundException("Role not found");
        }
        _ = await _userManager.AddToRoleAsync(user, role.RoleName);
    }
    public async Task UnassignUserRoleAsync(UnassignUserRole role, CancellationToken ct)
    {
        _logger.LogInformation($"Unassigning user role: {role.RoleName}");

        var user = await _userManager.FindByEmailAsync(role.UserEmail);
        if (user == null)
        {
            _logger.LogWarning("User with email {UserEmail} not found", role.UserEmail);
            throw new NotFoundException("User not found");
        }
        var roleExists = await _roleManager.FindByNameAsync(role.RoleName);
        if (roleExists == null)
        {
            _logger.LogWarning("Role {RoleName} does not exist", role.RoleName);
            throw new NotFoundException("Role not found");
        }
        _ = await _userManager.RemoveFromRoleAsync(user, role.RoleName);
    }
}
