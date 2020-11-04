using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    public class Adresses
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [DataType(DataType.Text)]
        public string Street { get; set; }

        [Required(ErrorMessage = "PostalCode is required")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "City is required")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [DataType(DataType.Text)]
        public string Country { get; set; }

        public string UserId { get; set; }

        public Users User { get; set; }
    }
}
