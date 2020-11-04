using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    [Table("produits")]
    public class Produits
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Column("name", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Prix is required")]
        [Column("prix", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Prix { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Column("stock", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Stock { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Column("description", TypeName = "varchar(100)")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required(ErrorMessage = "ImageUrl is required")]
        [Column("imageurl", TypeName = "ntext")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Column("userId")]
        public string UserId { get; set; }
        public Users User { get; set; }

        [Column("categorieId")]
        public int CategorieId { get; set; }
        public Categories Categorie { get; set; }
    }
}