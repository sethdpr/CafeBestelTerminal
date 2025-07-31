using CafeBestelTerminal.Data;
using CafeBestelTerminal.Models;
using CafeBestelTerminal.ViewModels;
using CafeBestelTerminal.Views;
using Microsoft.EntityFrameworkCore;
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
            var b = ViewModel.BestellingVM.GeselecteerdeBestelling;
            if (b == null) return;

            var dlg = new BestellingWindow(b) { Owner = this };
            var verwijder = dlg.ShowDialog() == true;
            if (verwijder)
            {
                ViewModel.BestellingVM.VerwijderCommand.Execute(null);
            }
        }

        private void Product_Dubbelklik(object sender, MouseButtonEventArgs e)
        {
            var p = ViewModel.ProductVM.GeselecteerdProduct;
            if (p == null) return;

            var dlg = new ProductWindow(p) { Owner = this };
            var verwijder = dlg.ShowDialog() == true;
            if (verwijder)
            {
                ViewModel.ProductVM.VerwijderCommand.Execute(null);
            }
        }

        private void Klant_Dubbelklik(object sender, MouseButtonEventArgs e)
        {
            var geselecteerdeKlant = ViewModel.KlantVM.GeselecteerdeKlant;
            if (geselecteerdeKlant == null)
                return;

            using var db = new AppDbContext();
            var klant = db.Klanten
                .Include(k => k.Bestellingen)
                .FirstOrDefault(k => k.KlantId == geselecteerdeKlant.KlantId);

            if (klant != null)
            {
                var dlg = new KlantWindow(klant) { Owner = this };
                var verwijder = dlg.ShowDialog() == true;
                if (verwijder)
                {
                    ViewModel.KlantVM.VerwijderCommand.Execute(null);
                }
            }
        }
    }
}