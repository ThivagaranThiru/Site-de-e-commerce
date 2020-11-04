using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.ModelsDTO
{
    public class PaniersDTO
    {
        public int Id { get; set; }
        public string Quantite { get; set; }
        public string Status { get; set; }

        public string UserEmail { get; set; }
        public int ProduitId { get; set; }
    }
}
