using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBestelTerminal.Models
{
    public class Bestelling
    {
        public int BestellingId { get; set; }
        public string Naam { get; set; }
        public DateTime Datum { get; set; }
        public string Personeelslid { get; set; }

        public int KlantId { get; set; }
        public Klant Klant { get; set; }

        public List<BestellingProduct> BestellingProducten { get; set; }

        public double Totaalprijs => BestellingProducten?.Sum(p => p.Product?.Prijs * p.Aantal) ?? 0;
    }
}
