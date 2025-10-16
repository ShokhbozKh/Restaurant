using Application.User.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users;

public interface IUserDetailsService
{
    Task UpdateUserDetailsAsync(UpdateUserDetailsDto userDto, CancellationToken ct);
}
