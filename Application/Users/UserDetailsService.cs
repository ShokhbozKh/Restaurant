using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions;
using Application.Users.Dtos;
using Application.AssignUsers;

namespace Application.Users;

public class UserDetailsService : IUserDetailsService
{
    private readonly IUserContext _userContext;
    private readonly ILogger _logger;
    private readonly IUserStore<Domain.Entities.User> _userStore;
    private readonly UserManager<Domain.Entities.User> _userManager;
    public UserDetailsService(IUserContext userContext, ILogger<UserDetailsService> logger, IUserStore<Domain.Entities.User> userStore, 
        UserManager<Domain.Entities.User> userManager)
    {
        _userContext = userContext;
        _logger = logger;
        _userStore = userStore;
        _userManager = userManager;
    }
    public async Task UpdateUserDetailsAsync(UpdateUserDetailsDto userDto , CancellationToken cancellationToken)
    {
        var user = _userContext.GetCurrentUser();
        _logger.LogInformation("User {UserId} is updating their details", user!.UserId);

        var dbUser = await _userStore.FindByIdAsync(user.UserId.ToString(), cancellationToken);

        if (dbUser == null)
        {
            _logger.LogWarning("User {UserId} not found in the database", user.UserId);
            throw new NotFoundException("User not found");
        }
        dbUser.Nationality = userDto.Nationality;
        dbUser.DateOfBirth = userDto.DateOfBirth;
        
        await _userStore.UpdateAsync(dbUser, cancellationToken);

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


    }

}
