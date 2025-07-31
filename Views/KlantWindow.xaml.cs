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
    }
}
