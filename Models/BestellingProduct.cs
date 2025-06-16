using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBestelTerminal.Models
{
    public class BestellingProduct
    {
        public int Id { get; set; }

        public int BestellingId { get; set; }
        public Bestelling Bestelling { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Aantal { get; set; }
    }
}
