using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBestelTerminal.ViewModels
{
    public class MainViewModel
    {
        public ProductViewModel ProductVM { get; set; } = new();
        public KlantViewModel KlantVM { get; set; } = new();
        public BestellingViewModel BestellingVM { get; set; } = new();
    }
}
