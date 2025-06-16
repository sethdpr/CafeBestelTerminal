using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBestelTerminal.Models
{
    public class Klant
    {
        public int KlantId { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public List<Bestelling> Bestellingen { get; set; }
    }
}
