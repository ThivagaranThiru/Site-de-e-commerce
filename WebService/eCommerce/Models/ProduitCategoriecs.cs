using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    public class ProduitCategoriecs
    {
        public PaginatedList<Produits> produits;

        public IEnumerable<Categories> categories;
    }
}
