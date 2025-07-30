using System;
using System.ComponentModel;

namespace CafeBestelTerminal.Models
{
    public class BestellingProduct : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public int BestellingId { get; set; }
        public Bestelling Bestelling { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        private int _aantal;
        public int Aantal
        {
            get => _aantal;
            set
            {
                if (_aantal != value)
                {
                    _aantal = value;
                    OnPropertyChanged(nameof(Aantal));
                    OnPropertyChanged(nameof(Totaalprijs));
                }
            }
        }

        public double Totaalprijs => Product?.Prijs * Aantal ?? 0;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string naam)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(naam));
    }
}