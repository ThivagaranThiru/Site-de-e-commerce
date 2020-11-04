using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    public class Users : IdentityUser
    {
        [Required(ErrorMessage = "Civilite is required")]
        [DataType(DataType.Text)]
        public string Civilite { get; set; }
       
        [Required(ErrorMessage = "FirstName is required")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        //[Required(ErrorMessage = "OldPassword is required")]
        [DataType(DataType.Text)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Text)]
        public string Password { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }

        public string RoleName { get; set; }

        public string token { get; set; }

        [BindProperty, Required(ErrorMessage = "Role is required")]
        public string RoleId { get; set; }

        public List<SelectListItem> role { get; set; }

    }
}