using eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Service.Interface
{
    public interface IRolesService
    {
        IEnumerable<Roles> GetRoles();
        Roles AddRoles(Roles role, string token);
        Roles GetRolesById(string id, string token);
        void Update(string id, Roles role, string token);
        void Delete(string id, string token);
        void AddRoleToUser(string idRole, string Email, string token);
        Roles SelectRole(string token);
    }
}
