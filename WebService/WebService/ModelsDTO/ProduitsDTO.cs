using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.ModelsDTO
{
    public class ProduitsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prix { get; set; }
        public string Stock { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public string UserName { get; set; }
        public string CategorieName { get; set; }

        public int CategorieId { get; set; }

    }
}