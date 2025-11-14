using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace GestionBanque.Models.DataService
{
    public class CompteSqliteDataService : BaseSqliteDataService, IDataService<Compte>
    {
        public CompteSqliteDataService(string nomBd) : base(nomBd)
        {
        }

        public IEnumerable<Compte> GetAll()
        {
            throw new NotImplementedException();
        }

        public Compte? Get(long id)
        {
            Compte? compte = null;

            using SqliteConnection connexion = OuvrirConnexion();

            using SqliteCommand commande = new SqliteCommand("SELECT * FROM compte WHERE id = @id", connexion);

            commande.Parameters.AddWithValue("@id", id);
            commande.Prepare();

            using SqliteDataReader lecteur = commande.ExecuteReader();
            if (lecteur.Read())
            {
                compte = new Compte(
                    lecteur.GetInt32(lecteur.GetOrdinal("id")),
                    lecteur.GetString(lecteur.GetOrdinal("no_compte")),
                    lecteur.GetDouble(lecteur.GetOrdinal("balance")),
                    lecteur.GetInt32(lecteur.GetOrdinal("client_id"))
                    );
            }

            return compte;
        }

        public bool Insert(Compte record)
        {
            throw new NotImplementedException();
        }

        public bool Update(Compte record)
        {
            using SqliteConnection connexion = OuvrirConnexion();

            //bogue: Id au lieu de Balance
            using SqliteCommand commande = new SqliteCommand("UPDATE compte SET balance = @balance WHERE id = @id", connexion);
            commande.Parameters.AddWithValue("@balance", record.Balance);
            commande.Parameters.AddWithValue("@id", record.Id);
            commande.Prepare();

            int nbLignesAffectees = commande.ExecuteNonQuery();
            if (nbLignesAffectees != 1) return false;

            return true;
        }

        public bool Delete(Compte record)
        {
            throw new NotImplementedException();
        }
    }
}
