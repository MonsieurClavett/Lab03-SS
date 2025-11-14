using GestionBanque.ViewModels.Commands;
using GestionBanque.ViewModels.Interfaces;

namespace GestionBanque.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _viewModelActuel;
        private BanqueViewModel _banqueViewModel;

        public MainViewModel(IInteractionUtilisateur interaction, BanqueViewModel banqueViewModel) : base(interaction)
        {
            _banqueViewModel = banqueViewModel;
            _viewModelActuel = _banqueViewModel;
            CmdGotoBanque = new RelayCommand(GotoBanque, null);
        }

        private void GotoBanque(object? obj)
        {
            ViewModelActuel = _banqueViewModel;
        }

        public RelayCommand CmdGotoBanque { get; private set; }

        public BaseViewModel ViewModelActuel
        {
            get { return _viewModelActuel; }
            set
            {
                _viewModelActuel = value;
                OnPropertyChanged();
            }
        }
    }
}
