using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    [Table("adresses")]
    public class Adresses
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [Column("street", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Street { get; set; }

        [Required(ErrorMessage = "PostalCode is required")]
        [Column("postalcode", TypeName = "varchar(100)")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Column("city", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [Column("country", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Country { get; set; }


        [Column("userId")]
        public string UserId { get; set; }
        public Users User { get; set; }
    }
}