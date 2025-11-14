using Autofac;
using GestionBanque.Models;
using GestionBanque.Models.DataService;
using GestionBanque.ViewModels;
using System.Reflection.Metadata;
using System.Windows;

//Alex Marcouiller
//resume des bogues:
//Compte Retirer():  ajout de :  || montant <= 0
//Compte Deposer(): peux deposer 0 donc < devient <=
//Compte Deposer(): pas -= mais +=
//Client : mauvais pattern courriel
//Client setter Nom: pas de trim()
//CompteSqliteDataService Update(): Id au lieu de Balance
//ClientSqliteDataService GetAll(): nom et prenom inverses
//BanqueViewModel Modifier(): ClientSelectionne.Nom = vieuxNom;
//BanqueViewModel Setter clientselectionne: Courriel = value?.Courriel ?? string.Empty;

namespace GestionBanque.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            //InteractionUtilisateurGui iug = new InteractionUtilisateurGui();
            //DataContext = new MainViewModel(iug, new BanqueViewModel(iug, new ClientSqliteDataService("banque.bd"), new CompteSqliteDataService("banque.bd")));
            DataContext = FournisseurDI.Container.Resolve<MainViewModel>();

        }
    }
}
