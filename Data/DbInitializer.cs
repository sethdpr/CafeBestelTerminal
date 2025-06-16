using CafeBestelTerminal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBestelTerminal.Data
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();

            if (context.Producten.Any()) return;

            var producten = new List<Product>
        {
            new Product { Naam = "Cola", Prijs = 2.5, DatumToegevoegd = DateTime.Today },
            new Product { Naam = "Bier", Prijs = 3.0, DatumToegevoegd = DateTime.Today }
        };
            context.Producten.AddRange(producten);

            var klanten = new List<Klant>
        {
            new Klant { Naam = "Jan", Beschrijving = "Regelmatige klant" },
            new Klant { Naam = "Emma", Beschrijving = "Nieuwe klant" }
        };
            context.Klanten.AddRange(klanten);
            context.SaveChanges();
        }
    }
}
