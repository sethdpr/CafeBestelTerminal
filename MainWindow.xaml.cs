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

        private void ToonDetails_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.ProductVM.GeselecteerdProduct != null)
            {
                var p = ViewModel.ProductVM.GeselecteerdProduct;
                MessageBox.Show($"Naam: {p.Naam}\nPrijs: €{p.Prijs}\nDatum toegevoegd: {p.DatumToegevoegd.ToShortDateString()}");
            }
        }

        private void Klant_Dubbelklik(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.KlantVM.GeselecteerdeKlant != null)
            {
                ToonBestellingenVanKlant(ViewModel.KlantVM.GeselecteerdeKlant);
            }
        }

        private void ToonBestellingen_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.KlantVM.GeselecteerdeKlant != null)
            {
                ToonBestellingenVanKlant(ViewModel.KlantVM.GeselecteerdeKlant);
            }
        }

        private void ToonBestellingenVanKlant(Klant klant)
        {
            using var db = new AppDbContext();
            var klantMetBestellingen = db.Klanten
                .Where(k => k.KlantId == klant.KlantId)
                .Select(k => new
                {
                    k.Naam,
                    Bestellingen = k.Bestellingen.Select(b => b.Naam + " op " + b.Datum.ToShortDateString())
                }).FirstOrDefault();

            if (klantMetBestellingen != null)
            {
                var bericht = $"Bestellingen voor {klantMetBestellingen.Naam}:\n" +
                              string.Join("\n", klantMetBestellingen.Bestellingen);
                MessageBox.Show(bericht, "Bestellingen");
            }
        }
    }
}