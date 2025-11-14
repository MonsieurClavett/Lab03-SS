using GestionBanque.ViewModels.Interfaces;
using System.Windows;

namespace GestionBanque.Views
{
    internal class InteractionUtilisateurGui : IInteractionUtilisateur
    {
        public void AfficherMessageErreur(string msg)
        {
            MessageBox.Show(msg, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool PoserQuestion(string question)
        {
            var resultat = MessageBox.Show(question, "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return resultat == MessageBoxResult.Yes;
        }
    }
}
