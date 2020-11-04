using eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Service.Interface
{
    public interface IProduitsService
    {
        IEnumerable<Produits> GetProduits(string token);
        IEnumerable<Produits> GetProduitByUser(string idUser, string token);
        Produits AddProduits(Produits produit, string token);
        Produits GetProduitById(int id, string token);
        void Update(int id, Produits produit, string token);
        void Delete(int id, string token);
    }
}
