using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.ModelsDTO
{
    public class UsersDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string Email { get; set; }
        public string Civilite { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string TelNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
    }
}