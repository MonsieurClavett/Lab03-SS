
using GestionBanque.Models.DataService;
using GestionBanque.Models;

namespace GestionBanque.Tests
{
    // Ce décorateur s'assure que toutes les classes de tests ayant le tag "Dataservice" soit
    // exécutées séquentiellement. Par défaut, xUnit exécute les différentes suites de tests
    // en parallèle. Toutefois, si nous voulons forcer l'exécution séquentielle entre certaines
    // suites, nous pouvons utiliser un décorateur avec un nom unique. Pour les tests sur les DataService,
    // il est important que cela soit séquentiel afin d'éviter qu'un test d'une classe supprime la 
    // bd de tests pendant qu'un test d'une autre classe utilise la bd. Bref, c'est pour éviter un
    // accès concurrent à la BD de tests!
    [Collection("Dataservice")]
    public class ClientSqliteDataServiceTest
    {
        private const string CheminBd = "test.bd";

        [Fact]
        [AvantApresDataService(CheminBd)]
        public void Get_ShouldBeValid()
        {
            // Préparation
            ClientSqliteDataService ds = new ClientSqliteDataService(CheminBd);
            Client clientAttendu = new Client(1, "Amar", "Quentin", "quentin@gmail.com");
            clientAttendu.Comptes.Add(new Compte(1, "9864", 831.76, 1));
            clientAttendu.Comptes.Add(new Compte(2, "2370", 493.04, 1));

            // Exécution
            Client? clientActuel = ds.Get(1);

            // Affirmation
            Assert.Equal(clientAttendu, clientActuel);
        }

        [Fact]
        [AvantApresDataService(CheminBd)]
        public void GetAll_ShouldReturnAllClientsAvecLeursComptes()
        {
            // Arrange
            var ds = new ClientSqliteDataService(CheminBd);

            // Act
            var clients = ds.GetAll().ToList();

            // Assert de base : il y a au moins un client
            Assert.NotEmpty(clients);

            // On vérifie qu'on retrouve le client 1 avec ses bons champs
            var client = clients.FirstOrDefault(c => c.Id == 1);
            Assert.NotNull(client);
            Assert.Equal("Amar", client.Nom);
            Assert.Equal("Quentin", client.Prenom);
            Assert.Equal("quentin@gmail.com", client.Courriel);

            // Et qu'il a bien ses comptes
            Assert.Equal(2, client.Comptes.Count);
            Assert.Contains(client.Comptes, c => c.Id == 1 && c.NoCompte == "9864");
            Assert.Contains(client.Comptes, c => c.Id == 2 && c.NoCompte == "2370");
        }

        [Fact]
        [AvantApresDataService(CheminBd)]
        public void RecupererComptes_AjouteLesComptesDuClient()
        {
            // Arrange
            var ds = new ClientSqliteDataService(CheminBd);
            var client = new Client(1, "Amar", "Quentin", "quentin@gmail.com");

            // Act
            ds.RecupererComptes(client);

            // Assert
            Assert.Equal(2, client.Comptes.Count);
            Assert.Contains(client.Comptes, c => c.Id == 1 && c.NoCompte == "9864");
            Assert.Contains(client.Comptes, c => c.Id == 2 && c.NoCompte == "2370");
        }


        [Fact]
        [AvantApresDataService(CheminBd)]
        public void Update_ClientExistant_ModifieClientEnBd()
        {
            // Arrange
            var ds = new ClientSqliteDataService(CheminBd);
            var client = ds.Get(1);
            Assert.NotNull(client);

            client.Nom = "NouveauNom";
            client.Prenom = "NouveauPrenom";
            client.Courriel = "nouveau@mail.com";

            // Act
            bool resultat = ds.Update(client);
            var clientMisAJour = ds.Get(1);

            // Assert
            Assert.True(resultat);
            Assert.NotNull(clientMisAJour);
            Assert.Equal("NouveauNom", clientMisAJour.Nom);
            Assert.Equal("NouveauPrenom", clientMisAJour.Prenom);
            Assert.Equal("nouveau@mail.com", clientMisAJour.Courriel);
        }

        [Fact]
        [AvantApresDataService(CheminBd)]
        public void Update_ClientInexistant_RetourneFalse()
        {
            // Arrange
            var ds = new ClientSqliteDataService(CheminBd);
            var client = new Client(9999, "X", "Y", "x@y.com");

            // Act
            bool resultat = ds.Update(client);

            // Assert
            Assert.False(resultat);
        }
    }
}
