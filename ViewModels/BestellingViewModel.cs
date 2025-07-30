using CafeBestelTerminal.Data;
using CafeBestelTerminal.Helpers;
using CafeBestelTerminal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace CafeBestelTerminal.ViewModels
{
    public class BestellingViewModel : INotifyPropertyChanged
    {
        public virtual ObservableCollection<Bestelling> Bestellingen { get; set; } = new();
        public virtual ObservableCollection<Klant> Klanten { get; set; } = new();
        public virtual ObservableCollection<Product> Producten { get; set; } = new();
        public virtual ObservableCollection<BestellingProduct> GekozenProducten { get; set; } = new();

        public Bestelling GeselecteerdeBestelling { get; set; }

        public string NieuweNaam { get; set; }
        public string Personeelslid { get; set; }
        public Klant GeselecteerdeKlant { get; set; }
        public Product GeselecteerdProduct { get; set; }
        public int AantalProduct { get; set; } = 1;

        public ICommand VoegToeCommand { get; }
        public ICommand VerwijderCommand { get; }
        public ICommand VoegProductToeCommand { get; }


        public BestellingViewModel()
        {
            LaadData();
            VoegToeCommand = new RelayCommand(VoegToe);
            VerwijderCommand = new RelayCommand(Verwijder);
            VoegProductToeCommand = new RelayCommand(VoegProductToe);
        }

        private void LaadData()
        {
            using var db = new AppDbContext();
            Bestellingen = new ObservableCollection<Bestelling>(
                db.Bestellingen
                  .Include(b => b.Klant)
                  .Include(b => b.BestellingProducten)
                    .ThenInclude(bp => bp.Product)
                  .ToList());

            Klanten = new ObservableCollection<Klant>(db.Klanten.ToList());
            Producten = new ObservableCollection<Product>(db.Producten.ToList());

            OnPropertyChanged(nameof(Bestellingen));
            OnPropertyChanged(nameof(Klanten));
            OnPropertyChanged(nameof(Producten));
        }

        private void VoegProductToe(object parameter = null)
        {
            if (parameter is Product product)
            {
                GeselecteerdProduct = product;
            }

            if (GeselecteerdProduct == null || AantalProduct <= 0)
            {
                MessageBox.Show("Selecteer een product en geef een geldig aantal op.");
                return;
            }

            var bestaand = GekozenProducten.FirstOrDefault(p => p.ProductId == GeselecteerdProduct.ProductId);
            if (bestaand != null)
            {
                bestaand.Aantal += AantalProduct;
            }
            else
            {
                GekozenProducten.Add(new BestellingProduct
                {
                    Product = GeselecteerdProduct,
                    ProductId = GeselecteerdProduct.ProductId,
                    Aantal = AantalProduct
                });
            }

            AantalProduct = 1;
            OnPropertyChanged(nameof(GekozenProducten));
            OnPropertyChanged(nameof(AantalProduct));
        }
        private void Verwijder(object parameter)
        {
            if (parameter is BestellingProduct bp)
            {
                bp.Aantal--;
                if (bp.Aantal <= 0)
                    GekozenProducten.Remove(bp);

                OnPropertyChanged(nameof(GekozenProducten));
            }
        }
        private void VoegToe()
        {
            try
            {
                using var db = new AppDbContext();

                var bestelling = new Bestelling
                {
                    Naam = NieuweNaam,
                    Personeelslid = Personeelslid,
                    Datum = DateTime.Now,
                    KlantId = GeselecteerdeKlant?.KlantId ?? 0,
                    BestellingProducten = GekozenProducten.Select(bp => new BestellingProduct
                    {
                        ProductId = bp.ProductId,
                        Aantal = bp.Aantal
                    }).ToList()
                };

                db.Bestellingen.Add(bestelling);
                db.SaveChanges();

                Bestellingen.Add(bestelling);
                GekozenProducten.Clear();
                NieuweNaam = string.Empty;
                Personeelslid = string.Empty;

                OnPropertyChanged(nameof(NieuweNaam));
                OnPropertyChanged(nameof(Personeelslid));
                OnPropertyChanged(nameof(GekozenProducten));

                MessageBox.Show("Bestelling toegevoegd!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij toevoegen bestelling:\n{ex.Message}\n\nDetails:\n{ex.InnerException?.Message}");
            }
        }
        public void ToonBestellingDetails()
        {
            var b = GeselecteerdeBestelling;
            if (b == null) return;

            var totaal = b.Totaalprijs.ToString("0.00");

            var sb = new StringBuilder();
            sb.AppendLine($"Naam: {b.Naam}");
            sb.AppendLine($"Datum: {b.Datum}");
            sb.AppendLine($"Personeelslid: {b.Personeelslid}");
            sb.AppendLine($"Klant: {b.Klant?.Naam ?? "Onbekend"}");
            sb.AppendLine($"Totaalprijs: € {totaal}");
            sb.AppendLine("Producten:");
            foreach (var bp in b.BestellingProducten)
            {
                sb.AppendLine($" - {bp.Aantal} x {bp.Product?.Naam} (€ {bp.Product?.Prijs.ToString("0.00")})");
            }

            MessageBox.Show(sb.ToString(), "Bestelling Details");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string naam = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naam));
    }
}