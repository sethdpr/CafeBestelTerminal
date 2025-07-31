using System.Windows;
using CafeBestelTerminal.Models;

namespace CafeBestelTerminal.Views
{
    public partial class BestellingWindow : Window
    {
        public BestellingWindow(Bestelling bestelling)
        {
            InitializeComponent();
            DataContext = bestelling;
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
