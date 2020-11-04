using eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Service.Interface
{
    public interface IAdressesService
    {
        IEnumerable<Adresses> GetAdresses(string token);
        Adresses AddAdresses(Adresses adresse, string token);
        Adresses GetAdressesById(int id, string token);
        void Update(int id, Adresses adresse, string token);
        void Delete(int id, string token);
        Adresses GetAdressesByUser(string id, string token);
    }
}
