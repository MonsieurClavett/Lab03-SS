namespace GestionBanque.ViewModels.Interfaces
{
    public interface IInteractionUtilisateur
    {
        public void AfficherMessageErreur(string msg);
        public bool PoserQuestion(string question);
    }
}
