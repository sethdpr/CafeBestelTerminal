using System;
using System.Windows.Input;

namespace CafeBestelTerminal.Helpers
{
    public class RelayCommand : ICommand
    {
        private readonly Action _uitvoerenZonderParameter;
        private readonly Action<object> _uitvoerenMetParameter;
        private readonly Func<bool> _kanUitvoeren;

        public RelayCommand(Action uitvoeren, Func<bool> kanUitvoeren = null)
        {
            _uitvoerenZonderParameter = uitvoeren;
            _kanUitvoeren = kanUitvoeren;
        }

        public RelayCommand(Action<object> uitvoerenMetParameter)
        {
            _uitvoerenMetParameter = uitvoerenMetParameter;
        }

        public bool CanExecute(object parameter)
            => _kanUitvoeren == null || _kanUitvoeren();

        public void Execute(object parameter)
        {
            if (_uitvoerenMetParameter != null)
                _uitvoerenMetParameter(parameter);
            else if (_uitvoerenZonderParameter != null)
                _uitvoerenZonderParameter();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
