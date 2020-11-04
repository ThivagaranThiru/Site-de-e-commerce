using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    [Table("commandes")]
    public class Commandes
    {
        public int Id { get; set; }

        [Column("livraison", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Livraison { get; set; } = "En Cours de livraison";

        [Column("payment", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Payment { get; set; } = "En Cours de Payement";

        [Required(ErrorMessage = "TotalQt is required")]
        [Column("totalqt", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string TotalQt { get; set; }

        [Required(ErrorMessage = "TotalPrix is required")]
        [Column("totalPrix", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string TotalPrix { get; set; }

        public ICollection<Paniers> Panier { get; set; }
    }
}