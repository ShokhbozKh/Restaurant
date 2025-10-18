using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AssignUsers;

public interface IAssignUserService
{
    Task AssignUserRoleAsync(AssignUserRole role, CancellationToken ct);
    Task UnassignUserRoleAsync(UnassignUserRole role, CancellationToken ct);

}
