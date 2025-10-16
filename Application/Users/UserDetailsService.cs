using Application.User;
using Application.User.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Users;

public class UserDetailsService : IUserDetailsService
{
    private readonly IUserContext _userContext;
    private readonly ILogger _logger;
    private readonly IUserStore<Domain.Entities.User> _userStore;
    public UserDetailsService(IUserContext userContext, ILogger<UserDetailsService> logger, IUserStore<Domain.Entities.User> userStore)
    {
        _userContext = userContext;
        _logger = logger;
        _userStore = userStore;
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
}
