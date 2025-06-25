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

namespace CafeBestelTerminal.ViewModels
{
    public class KlantViewModel : INotifyPropertyChanged
    {
        public virtual ObservableCollection<Klant> Klanten { get; set; } = new();

        private Klant _geselecteerdeKlant;
        public Klant GeselecteerdeKlant
        {
            get => _geselecteerdeKlant;
            set { _geselecteerdeKlant = value; OnPropertyChanged(); }
        }

        public string NieuweNaam { get; set; }
        public string NieuweBeschrijving { get; set; }

        public ICommand VoegToeCommand { get; }
        public ICommand VerwijderCommand { get; }

        public KlantViewModel()
        {
            LaadKlanten();
            VoegToeCommand = new RelayCommand(VoegToe);
            VerwijderCommand = new RelayCommand(Verwijder, () => GeselecteerdeKlant != null);
        }

        private void LaadKlanten()
        {
            try
            {
                using var db = new AppDbContext();
                var klantenLijst = db.Klanten.ToList();
                Klanten = new ObservableCollection<Klant>(klantenLijst);
                OnPropertyChanged(nameof(Klanten));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij laden van klanten: " + ex.Message);
            }
        }

        private void VoegToe()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NieuweNaam))
                {
                    MessageBox.Show("Naam mag niet leeg zijn.");
                    return;
                }

                using var db = new AppDbContext();
                var klant = new Klant
                {
                    Naam = NieuweNaam,
                    Beschrijving = NieuweBeschrijving
                };
                db.Klanten.Add(klant);
                db.SaveChanges();
                Klanten.Add(klant);

                NieuweNaam = string.Empty;
                NieuweBeschrijving = string.Empty;
                OnPropertyChanged(nameof(NieuweNaam));
                OnPropertyChanged(nameof(NieuweBeschrijving));

                MessageBox.Show("Klant succesvol toegevoegd!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij toevoegen klant: " + ex.Message);
            }
        }

        private void Verwijder()
        {
            if (GeselecteerdeKlant == null) return;

            var bevestiging = MessageBox.Show($"Weet je zeker dat je {GeselecteerdeKlant.Naam} wilt verwijderen?",
                                              "Bevestig verwijdering", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (bevestiging != MessageBoxResult.Yes) return;

            try
            {
                using var db = new AppDbContext();
                var klant = db.Klanten.FirstOrDefault(k => k.KlantId == GeselecteerdeKlant.KlantId);
                if (klant != null)
                {
                    db.Klanten.Remove(klant);
                    db.SaveChanges();
                    Klanten.Remove(GeselecteerdeKlant);
                    GeselecteerdeKlant = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij verwijderen klant: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string naam = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naam));
    }
}
