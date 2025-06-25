using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBestelTerminal.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Naam { get; set; }
        public double Prijs { get; set; }
        public DateTime DatumToegevoegd { get; set; }
    }
}