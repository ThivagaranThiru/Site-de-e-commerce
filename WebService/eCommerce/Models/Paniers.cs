using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    public class Paniers
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Quantite is required")]
        [DataType(DataType.Text)]
        public string Quantite { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [DataType(DataType.Text)]
        public string Status { get; set; }

        public string UserEmail { get; set; }

        public string UserId { get; set; }
        public Users User { get; set; }

        public int ProduitId { get; set; }
        public Produits Produit { get; set; }

        public int totalPrix { get; set; }
    }
}
