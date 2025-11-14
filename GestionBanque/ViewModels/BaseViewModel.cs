using GestionBanque.ViewModels.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GestionBanque.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected IInteractionUtilisateur _interaction;

        public BaseViewModel(IInteractionUtilisateur interaction)
        {
            _interaction = interaction;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
