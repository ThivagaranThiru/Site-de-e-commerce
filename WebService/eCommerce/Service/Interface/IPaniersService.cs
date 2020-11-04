using eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Service.Interface
{
    public interface IPaniersService
    {
        IEnumerable<Paniers> GetPaniers(string token);
        IEnumerable<Paniers> GetPaniersByUser(string idUser, string status, string token);
        IEnumerable<Paniers> GetCommandsByUser(string idUser, string token);
        Paniers AddPaniers(Paniers panier, string token);
        Paniers GetPanierById(int id, string token);
        void Update(int id, Paniers panier, string token);
        void Delete(int id, string token);
    }
}
