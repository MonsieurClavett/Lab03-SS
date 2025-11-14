using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace GestionBanque.Models.DataService
{
    public class ClientSqliteDataService : BaseSqliteDataService, IDataService<Client>
    {
        public ClientSqliteDataService(string nomBd) : base(nomBd)
        {
        }

        public IEnumerable<Client> GetAll()
        {
            List<Client> clients = new List<Client>();

            using SqliteConnection connexion = OuvrirConnexion();

            using SqliteCommand commande = new SqliteCommand("SELECT * FROM client", connexion);

            using SqliteDataReader lecteur = commande.ExecuteReader();
            while (lecteur.Read())
            {
                //bogue : nom et prenom inverses
                Client c = new Client(
                    lecteur.GetInt32(lecteur.GetOrdinal("id")),
                    lecteur.GetString(lecteur.GetOrdinal("nom")),
                    lecteur.GetString(lecteur.GetOrdinal("prenom")),
                    lecteur.GetString(lecteur.GetOrdinal("courriel"))
                    );

                RecupererComptes(c);

                clients.Add(c);
            }

            return clients;
        }

        public Client? Get(long id)
        {
            Client? client = null;

            using SqliteConnection connexion = OuvrirConnexion();

            using SqliteCommand commande = new SqliteCommand("SELECT * FROM client WHERE id = @id", connexion);

            commande.Parameters.AddWithValue("@id", id);
            commande.Prepare();

            using SqliteDataReader lecteur = commande.ExecuteReader();
            if (lecteur.Read())
            {
                client = new Client(
                    lecteur.GetInt32(lecteur.GetOrdinal("id")),
                    lecteur.GetString(lecteur.GetOrdinal("nom")),
                    lecteur.GetString(lecteur.GetOrdinal("prenom")),
                    lecteur.GetString(lecteur.GetOrdinal("courriel"))
                    );

                RecupererComptes(client);
            }

            return client;
        }

        public bool Insert(Client record)
        {
            throw new NotImplementedException();
        }

        public bool Update(Client record)
        {
            using SqliteConnection connexion = OuvrirConnexion();

            using SqliteCommand commande = new SqliteCommand("UPDATE client SET nom = @nom, prenom = @prenom, courriel = @courriel WHERE id = @id", connexion);
            commande.Parameters.AddWithValue("@nom", record.Nom);
            commande.Parameters.AddWithValue("@prenom", record.Prenom);
            commande.Parameters.AddWithValue("@courriel", record.Courriel);
            commande.Parameters.AddWithValue("@id", record.Id);
            commande.Prepare();

            int nbLignesAffectees = commande.ExecuteNonQuery();
            if (nbLignesAffectees != 1) return false;

            return true;
        }

        public bool Delete(Client record)
        {
            throw new NotImplementedException();
        }

        public void RecupererComptes(Client c)
        {
            using SqliteConnection connexion = OuvrirConnexion();

            using SqliteCommand commande = new SqliteCommand("SELECT * FROM compte WHERE client_id=@client_id", connexion);
            commande.Parameters.AddWithValue("@client_id", c.Id);

            using SqliteDataReader lecteur = commande.ExecuteReader();

            while (lecteur.Read())
            {
                Compte compte = new Compte(
                    lecteur.GetInt32(lecteur.GetOrdinal("id")),
                    lecteur.GetString(lecteur.GetOrdinal("no_compte")),
                    lecteur.GetDouble(lecteur.GetOrdinal("balance")),
                    lecteur.GetInt32(lecteur.GetOrdinal("client_id"))
                    );
                c.Comptes.Add(compte);
            }
        }
    }
}
