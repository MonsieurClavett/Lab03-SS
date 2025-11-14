using Microsoft.Data.Sqlite;
using System.IO;

namespace GestionBanque.Models.DataService
{
    public abstract class BaseSqliteDataService
    {
        private string _nomBd;

        protected BaseSqliteDataService(string nomBd)
        {
            _nomBd = nomBd;
            if (!File.Exists(_nomBd))
            {
                using SqliteConnection connexion = OuvrirConnexion();
                string contenuFichier = Properties.Resources.bd;
                using SqliteCommand commande = new SqliteCommand(contenuFichier, connexion);
                commande.ExecuteNonQuery();
            }
        }

        protected SqliteConnection OuvrirConnexion()
        {
            SqliteConnection connexion = new SqliteConnection($"Data Source={_nomBd};Cache=Shared");
            connexion.Open();
            return connexion;
        }
    }
}
