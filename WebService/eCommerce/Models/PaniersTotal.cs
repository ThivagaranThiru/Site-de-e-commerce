using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models
{
    public class PaniersTotal
    {
        public IEnumerable<Paniers> paniers { get; set; }
        public int totalPaniers { get; set; }
        public int totalQuantite { get; set; }
        public Users user { get; set; }
        public Adresses adresse { get; set; }
    }
}
