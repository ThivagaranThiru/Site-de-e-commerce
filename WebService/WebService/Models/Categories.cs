using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    [Table("categories")]
    public class Categories
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Column("name", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Column("description", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}