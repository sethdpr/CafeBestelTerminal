using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CafeBestelTerminal.Helpers
{
    public class RelayCommand : ICommand
    {
        private readonly Action _uitvoeren;
        private readonly Func<bool> _kanUitvoeren;

        public RelayCommand(Action uitvoeren, Func<bool> kanUitvoeren = null)
        {
            _uitvoeren = uitvoeren;
            _kanUitvoeren = kanUitvoeren;
        }

        public bool CanExecute(object parameter) => _kanUitvoeren == null || _kanUitvoeren();
        public void Execute(object parameter) => _uitvoeren();

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
