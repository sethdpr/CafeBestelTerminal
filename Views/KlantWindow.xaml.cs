using System.Text;
using System.Windows;
using CafeBestelTerminal.Models;

namespace CafeBestelTerminal.Views
{
    public partial class KlantWindow : Window
    {
        public KlantWindow(Klant klant)
        {
            InitializeComponent();
            DataContext = klant;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Verwijder_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Afreken_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Klant klant && klant.Bestellingen != null)
            {
                var sb = new StringBuilder();
                double totaal = 0;

                sb.AppendLine($"Bestellingen van {klant.Naam}:\n");

                foreach (var bestelling in klant.Bestellingen)
                {
                    double prijs = bestelling.BestellingProducten?.Sum(bp => bp.Product?.Prijs * bp.Aantal) ?? 0;
                    sb.AppendLine($"{bestelling.Naam} op {bestelling.Datum.ToShortDateString()} - €{prijs:F2}");
                    totaal += prijs;
                }

                sb.AppendLine($"\nTotaal te betalen: €{totaal:F2}");

                MessageBox.Show(sb.ToString(), "Afrekenen", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
