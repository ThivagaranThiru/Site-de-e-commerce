using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    public class Produits
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Prix is required")]
        [DataType(DataType.Text)]
        public string Prix { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [DataType(DataType.Text)]
        public string Stock { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required(ErrorMessage = "ImageUrl is required")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public string UserName { get; set; }
        public string CategorieName { get; set; }

        public string UserId { get; set; }
        public int CategorieId { get; set; }

        public List<SelectListItem> categories { get; set; }

        /*public string UserId { get; set; }
         public Users User { get; set; }

         public int CategorieId { get; set; }
         public Categories Categorie { get; set; }*/
    }
}