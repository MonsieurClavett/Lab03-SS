using GestionBanque.Models;
using GestionBanque.Models.DataService;
using GestionBanque.ViewModels.Commands;
using GestionBanque.ViewModels.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace GestionBanque.ViewModels
{
    public class BanqueViewModel : BaseViewModel
    {
        private ObservableCollection<Client> _clients;
        private Client _clientSelectionne;
        private Compte _compteSelectionne;

        private string _nom;
        private string _prenom;
        private string _courriel;
        private double _montantTransaction;

        private IDataService<Client> _dsClients;
        private IDataService<Compte> _dsComptes;

        public BanqueViewModel(
            IInteractionUtilisateur interaction, 
            IDataService<Client> dsClients, 
            IDataService<Compte> dsComptes) : base(interaction)
        {
            _dsClients = dsClients;
            _dsComptes = dsComptes;
            _clients = new ObservableCollection<Client>(_dsClients.GetAll());

            Nom = string.Empty;
            Prenom = string.Empty;
            Courriel = string.Empty;

            ModifierCommand = new RelayCommand(Modifier, (obj) => ClientSelectionne != null);
            DeposerCommand = new RelayCommand(Deposer, (obj) => CompteSelectionne != null);
            RetirerCommand = new RelayCommand(Retirer, (obj) => CompteSelectionne != null);
        }

        public void Modifier(object? obj)
        {
            if (ClientSelectionne != null)
            {
                string vieuxNom = ClientSelectionne.Nom;
                string vieuxPrenom = ClientSelectionne.Prenom;
                string vieuxCourriel = ClientSelectionne.Courriel;
                try 
                {
                    ClientSelectionne.Nom = Nom;
                    ClientSelectionne.Prenom = Prenom;
                    ClientSelectionne.Courriel = Courriel;
                    _dsClients.Update(ClientSelectionne);
                }
                catch (Exception ex) 
                { 
                    //bogue
                    ClientSelectionne.Nom = vieuxNom;
                    ClientSelectionne.Prenom = vieuxPrenom;
                    ClientSelectionne.Courriel = vieuxCourriel;
                    _interaction.AfficherMessageErreur(ex.Message);

                }
            }
        }

        public void Retirer(object? obj)
        {
            try
            {
                CompteSelectionne.Retirer(MontantTransaction);
                _dsComptes.Update(CompteSelectionne);
                MontantTransaction = 0;
            }
            catch (Exception ex)
            {
                _interaction.AfficherMessageErreur(ex.Message);
            }
        }

        public void Deposer(object? obj)
        {
            try
            {
                CompteSelectionne.Deposer(MontantTransaction);
                _dsComptes.Update(CompteSelectionne);
                MontantTransaction = 0;
            }
            catch (Exception ex)
            {
                _interaction.AfficherMessageErreur(ex.Message);
            }
        }

        public RelayCommand DeposerCommand { get; private set; }
        public RelayCommand RetirerCommand { get; private set; }
        public RelayCommand ModifierCommand { get; private set; }

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        public Client ClientSelectionne
        {
            get => _clientSelectionne;
            set
            {
                _clientSelectionne = value;
                Nom = value?.Nom ?? string.Empty;
                Prenom = value?.Prenom ?? string.Empty;
                //bogue
                Courriel = value?.Courriel ?? string.Empty;
                OnPropertyChanged();
            }
        }

        public Compte CompteSelectionne
        {
            get => _compteSelectionne;
            set
            {
                _compteSelectionne = value;
                OnPropertyChanged();
            }
        }

        public string Nom
        {
            get { return _nom; }
            set
            {
                _nom = value;
                OnPropertyChanged();
            }
        }

        public string Prenom
        {
            get { return _prenom; }
            set
            {
                _prenom = value;
                OnPropertyChanged();
            }
        }

        public string Courriel
        {
            get { return _courriel; }
            set
            {
                _courriel = value;
                OnPropertyChanged();
            }
        }

        public double MontantTransaction
        {
            get { return _montantTransaction; }
            set
            {
                _montantTransaction = value;
                OnPropertyChanged();
            }
        }
    }
}
