using System.Windows;
using CafeBestelTerminal.Models;

namespace CafeBestelTerminal.Views
{
    public partial class ProductWindow : Window
    {
        public ProductWindow(Product product)
        {
            InitializeComponent();
            DataContext = product;
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
