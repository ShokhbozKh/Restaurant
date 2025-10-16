using Domain.Entities;
using Domain.Users;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class UserDetailsRepository : IUserDetailsRepository
{
    private readonly AppDbContext _context; // DI

    public UserDetailsRepository(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }
    public async Task UpdateUserDetailsAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
