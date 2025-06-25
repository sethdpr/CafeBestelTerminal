using CafeBestelTerminal.Data;
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
using CafeBestelTerminal.Helpers;

namespace CafeBestelTerminal.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public virtual ObservableCollection<Product> Producten { get; set; } = new();
        private Product _geselecteerdProduct;
        public Product GeselecteerdProduct
        {
            get => _geselecteerdProduct;
            set { _geselecteerdProduct = value; OnPropertyChanged(); }
        }

        public string NieuweNaam { get; set; }
        public double NieuwePrijs { get; set; }

        public ICommand VoegToeCommand { get; }
        public ICommand VerwijderCommand { get; }

        public ProductViewModel()
        {
            LaadProducten();

            VoegToeCommand = new RelayCommand(VoegToe);
            VerwijderCommand = new RelayCommand(Verwijder, () => GeselecteerdProduct != null);
        }

        private void LaadProducten()
        {
            using var db = new AppDbContext();
            Producten = new ObservableCollection<Product>(db.Producten.ToList());
            OnPropertyChanged(nameof(Producten));
        }

        private void VoegToe()
        {
            try
            {
                using var db = new AppDbContext();
                var nieuwProduct = new Product
                {
                    Naam = NieuweNaam,
                    Prijs = NieuwePrijs,
                    DatumToegevoegd = DateTime.Today
                };
                db.Producten.Add(nieuwProduct);
                db.SaveChanges();
                Producten.Add(nieuwProduct);
                MessageBox.Show("Product toegevoegd!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij toevoegen: " + ex.Message);
            }
        }

        private void Verwijder()
        {
            if (GeselecteerdProduct == null) return;

            try
            {
                using var db = new AppDbContext();
                db.Producten.Remove(GeselecteerdProduct);
                db.SaveChanges();
                Producten.Remove(GeselecteerdProduct);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij verwijderen: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string naam = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naam));
    }
}
