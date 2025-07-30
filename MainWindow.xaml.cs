using CafeBestelTerminal.Data;
using CafeBestelTerminal.Models;
using CafeBestelTerminal.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CafeBestelTerminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
        private void Bestelling_DubbelKlik(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.BestellingVM.GeselecteerdeBestelling != null)
            {
                ViewModel.BestellingVM.ToonBestellingDetails();
            }
        }

        private void Product_Dubbelklik(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.ProductVM.GeselecteerdProduct != null)
            {
                var p = ViewModel.ProductVM.GeselecteerdProduct;
                MessageBox.Show($"Naam: {p.Naam}\nPrijs: €{p.Prijs:F2}\nDatum toegevoegd: {p.DatumToegevoegd.ToShortDateString()}",
                                "Productdetails");
            }
        }

        private void Klant_Dubbelklik(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.KlantVM.GeselecteerdeKlant != null)
            {
                ToonBestellingenVanKlant(ViewModel.KlantVM.GeselecteerdeKlant);
            }
        }

        private void ToonBestellingenVanKlant(Klant klant)
        {
            using var db = new AppDbContext();

            var klantInfo = db.Klanten
                .Where(k => k.KlantId == klant.KlantId)
                .Select(k => new
                {
                    k.Naam,
                    k.Beschrijving,
                    Bestellingen = k.Bestellingen
                        .OrderByDescending(b => b.Datum)
                        .Select(b => $"{b.Naam} op {b.Datum.ToShortDateString()}")
                        .ToList()
                })
                .FirstOrDefault();

            if (klantInfo == null)
            {
                MessageBox.Show("Klant niet gevonden.", "Bestellingen");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Klant: {klantInfo.Naam}");
            sb.AppendLine($"Beschrijving: {klantInfo.Beschrijving}");
            sb.AppendLine();

            if (klantInfo.Bestellingen.Any())
            {
                sb.AppendLine("Bestellingen:");
                sb.AppendLine(string.Join(Environment.NewLine, klantInfo.Bestellingen));
            }
            else
            {
                sb.AppendLine("Geen bestellingen gevonden.");
            }

            MessageBox.Show(sb.ToString(), "Bestellingen");
        }
    }
}