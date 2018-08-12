using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GabinetStomatologiczny.Services
{

    public interface IUserRoleService
    {
        string GetUserRole(ClaimsIdentity user);
    }
    public class UserRoleService : IUserRoleService
    {
        public string GetUserRole(ClaimsIdentity user)
        {
            var claims = user.Claims;
            var roleClaimType = user.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            string rol = "";
            foreach (var item in roles)
            {
               rol = item.Value;
            }
            return rol;
        }
    }
}
