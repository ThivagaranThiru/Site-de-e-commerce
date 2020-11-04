using eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Service.Interface
{
    public interface IUsersService
    {
        IEnumerable<Users> GetUsers(string token);
        Users AddUsers(Users user, string token);
        Users GetUserById(string id, string token);
        void Update(string id, Users user, string token);
        void Delete(string id, string token);
        Users Register(Users user);
        Users Login(Login user);
        void Logout(string token);
        void ChangePassword(Users user, string token);
    }
}
