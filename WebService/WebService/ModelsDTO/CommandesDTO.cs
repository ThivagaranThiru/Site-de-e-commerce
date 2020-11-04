using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.ModelsDTO
{
    public class CommandesDTO
    {
        public int Id { get; set; }
        public string Livraison { get; set; }
        public string Payment { get; set; }
        public string TotalQt { get; set; }
        public string TotalPrix { get; set; }

        public ICollection<Paniers> Panier { get; set; }

    }
}
