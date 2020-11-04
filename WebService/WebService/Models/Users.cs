using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    [Table("users")]
    public class Users : IdentityUser
    {
        [Required(ErrorMessage = "Civilite is required")]
        [Column("civilite", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Civilite { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [Column("firstname", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Column("name", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required")]
        [Column("dateofbirth", TypeName = "datetime")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }
    }
}