using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace WebService.Models
{
    [Table("paniers")]
    public class Paniers
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Quantite is required")]
        [Column("quantite", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Quantite { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Column("status", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Status { get; set; } = "En Cours";

        [Column("userId")]
        public string UserId { get; set; }
        public Users User { get; set; }

        [Column("produitId")]
        public int ProduitId { get; set; }
        public Produits Produit { get; set; }

        public Commandes commandes { get; set; }
    }
}