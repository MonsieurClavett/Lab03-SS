using GestionBanque.Models.DataService;
using GestionBanque.Models;
using GestionBanque.ViewModels.Interfaces;
using GestionBanque.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionBanque.Tests
{
    public class BanqueViewModelTest
    {
        private readonly Mock<IInteractionUtilisateur> _interactionMock = new Mock<IInteractionUtilisateur>();
        private readonly Mock<IDataService<Client>> _dsClientsMock = new Mock<IDataService<Client>>();
        private readonly Mock<IDataService<Compte>> _dsComptesMock = new Mock<IDataService<Compte>>();

        public BanqueViewModelTest()
        {
            _interactionMock.Setup(i => i.AfficherMessageErreur(It.IsAny<string>()));
        }

        private List<Client> ListeClientsAttendues()
        {
            var c1 = new Client(1, "Amar", "Quentin", "quentin@gmail.com");
            var c2 = new Client(2, "Gagnon", "Marie", "marie@gagnon.ca");
            return new List<Client> { c1, c2 };
        }

        // 1) Constructeur

        [Fact]
        public void Constructeur_ShouldBeValid()
        {
            // Préparation
            _dsClientsMock.Setup(ds => ds.GetAll()).Returns(ListeClientsAttendues());

            // Exécution
            BanqueViewModel vm = new BanqueViewModel(
                _interactionMock.Object,
                _dsClientsMock.Object,
                _dsComptesMock.Object);

            // Affirmation
            Assert.NotNull(vm.Clients);
            Assert.Equal(2, vm.Clients.Count);

            Assert.Equal(string.Empty, vm.Nom);
            Assert.Equal(string.Empty, vm.Prenom);
            Assert.Equal(string.Empty, vm.Courriel);

            Assert.NotNull(vm.ModifierCommand);
            Assert.NotNull(vm.DeposerCommand);
            Assert.NotNull(vm.RetirerCommand);

            Assert.False(vm.ModifierCommand.CanExecute(null));
            Assert.False(vm.DeposerCommand.CanExecute(null));
            Assert.False(vm.RetirerCommand.CanExecute(null));

            _dsClientsMock.Verify(ds => ds.GetAll(), Times.Once);
        }

        // 2) Setter de ClientSelectionne

        [Fact]
        public void ClientSelectionne_Setter_ShouldUpdateNomPrenomCourrielAndEnableModifier()
        {
            // Préparation
            _dsClientsMock.Setup(ds => ds.GetAll()).Returns(new List<Client>());
            BanqueViewModel vm = new BanqueViewModel(
                _interactionMock.Object,
                _dsClientsMock.Object,
                _dsComptesMock.Object);

            Client client = new Client(1, "Tremblay", "Alex", "alex@gmail.com");

            // Exécution
            vm.ClientSelectionne = client;

            // Affirmation
            Assert.Equal(client, vm.ClientSelectionne);
            Assert.Equal("Tremblay", vm.Nom);
            Assert.Equal("Alex", vm.Prenom);
            Assert.Equal("alex@gmail.com", vm.Courriel);
            Assert.True(vm.ModifierCommand.CanExecute(null));
        }

        // 3) Modifiier

        [Fact]
        public void Modifier_ShouldUpdateClientAndCallDataService()
        {
            // Préparation
            _dsClientsMock.Setup(ds => ds.GetAll()).Returns(new List<Client>());
            BanqueViewModel vm = new BanqueViewModel(
                _interactionMock.Object,
                _dsClientsMock.Object,
                _dsComptesMock.Object);

            Client client = new Client(1, "Tremblay", "Alex", "alex@old.com");
            vm.ClientSelectionne = client;

            vm.Nom = "Gagnon";
            vm.Prenom = "Marie";
            vm.Courriel = "marie@gagnon.com";

            _dsClientsMock.Setup(ds => ds.Update(client)).Returns(true);

            // Exécution
            vm.Modifier(null);

            // Affirmation
            _dsClientsMock.Verify(ds => ds.Update(client), Times.Once);
            _interactionMock.Verify(i => i.AfficherMessageErreur(It.IsAny<string>()), Times.Never);
            Assert.Equal("Gagnon", client.Nom);
            Assert.Equal("Marie", client.Prenom);
            Assert.Equal("marie@gagnon.com", client.Courriel);
        }

        [Fact]
        public void Modifier_SiUpdateLanceException_DoitRetablirNomPrenomCourrielEtAfficherErreur()
        {
            // Préparation
            _dsClientsMock.Setup(ds => ds.GetAll()).Returns(new List<Client>());
            BanqueViewModel vm = new BanqueViewModel(
                _interactionMock.Object,
                _dsClientsMock.Object,
                _dsComptesMock.Object);

            Client client = new Client(1, "Tremblay", "Alex", "alex@old.com");
            vm.ClientSelectionne = client;

            vm.Nom = "Gagnon";
            vm.Prenom = "Marie";
            vm.Courriel = "marie@gagnon.com";

            _dsClientsMock
                .Setup(ds => ds.Update(client))
                .Throws(new System.Exception("BOOM"));

            // Exécution
            vm.Modifier(null);

            // Affirmation
            Assert.Equal("Tremblay", client.Nom);
            Assert.Equal("Alex", client.Prenom);
            Assert.Equal("alex@old.com", client.Courriel);

            _interactionMock.Verify(i => i.AfficherMessageErreur("BOOM"), Times.Once);
        }

        // 4) Retirer()

        [Fact]
        public void Retirer_ShouldCallRetirerOnCompteAndUpdateDataService()
        {
            // Préparation
            _dsClientsMock.Setup(ds => ds.GetAll()).Returns(new List<Client>());
            BanqueViewModel vm = new BanqueViewModel(
                _interactionMock.Object,
                _dsClientsMock.Object,
                _dsComptesMock.Object);

            Compte compte = new Compte(1, "1234", 200.0, 1);
            vm.CompteSelectionne = compte;
            vm.MontantTransaction = 50.0;

            _dsComptesMock.Setup(ds => ds.Update(compte)).Returns(true);

            // Exécution
            vm.Retirer(null);

            // Affirmation
            Assert.Equal(150.0, compte.Balance);
            Assert.Equal(0.0, vm.MontantTransaction);
            _dsComptesMock.Verify(ds => ds.Update(compte), Times.Once);
            _interactionMock.Verify(i => i.AfficherMessageErreur(It.IsAny<string>()), Times.Never);
        }

        // 5) Deposer()

        [Fact]
        public void Deposer_ShouldCallDeposerOnCompteAndUpdateDataService()
        {
            // Préparation
            _dsClientsMock.Setup(ds => ds.GetAll()).Returns(new List<Client>());
            BanqueViewModel vm = new BanqueViewModel(
                _interactionMock.Object,
                _dsClientsMock.Object,
                _dsComptesMock.Object);

            Compte compte = new Compte(1, "1234", 100.0, 1);
            vm.CompteSelectionne = compte;
            vm.MontantTransaction = 25.0;

            _dsComptesMock.Setup(ds => ds.Update(compte)).Returns(true);

            // Exécution
            vm.Deposer(null);

            // Affirmation
            Assert.Equal(125.0, compte.Balance);
            Assert.Equal(0.0, vm.MontantTransaction);
            _dsComptesMock.Verify(ds => ds.Update(compte), Times.Once);
            _interactionMock.Verify(i => i.AfficherMessageErreur(It.IsAny<string>()), Times.Never);
        }
    }
}



