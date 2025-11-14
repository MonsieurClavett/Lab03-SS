using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionBanque.Models;

namespace GestionBanque.Tests
{
    public class ClientTest
    {
        private Client CreerClientValide()
        {
            return new Client(
                id: 1,
                nom: "Tremblay",
                prenom: "Alex",
                courriel: "alex.tremblay@example.com"
            );
        }

        [Fact]
        public void Nom_Valide_ModifieLaValeur()
        {
            // Arrange
            var client = CreerClientValide();

            // Act
            client.Nom = "    Gagnon";

            // Assert
            Assert.Equal("Gagnon", client.Nom);
        }

        [Fact]
        public void Nom_Null_DoitLancerException()
        {
            // Arrange
            var client = CreerClientValide();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => client.Nom = null);
        }

        [Fact]
        public void Nom_VideOuEspaces_DoitLancerException()
        {
            // Arrange
            var client = CreerClientValide();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => client.Nom = "   ");
        }

        [Fact]
        public void Prenom_Valide_ModifieLaValeurAvecTrim()
        {
            // Arrange
            var client = CreerClientValide();

            // Act
            client.Prenom = "  Marie  ";

            // Assert
            Assert.Equal("Marie", client.Prenom);
        }

        [Fact]
        public void Prenom_Null_DoitLancerException()
        {
            // Arrange
            var client = CreerClientValide();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => client.Prenom = null);
        }

        [Fact]
        public void Prenom_VideOuEspaces_DoitLancerException()
        {
            // Arrange
            var client = CreerClientValide();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => client.Prenom = "   ");
        }

        [Fact]
        public void Courriel_Valide_ModifieLaValeur()
        {
            // Arrange
            var client = CreerClientValide();

            // Act
            client.Courriel = "nouveau.mail@example.com";

            // Assert
            Assert.Equal("nouveau.mail@example.com", client.Courriel);
        }

        [Theory]
        [InlineData("pasUnVraiCourriel")]     // pas de @
        [InlineData("alex@@example.com")]     // deux @
        [InlineData("alex.example.com")]      // pas de @
        [InlineData("alex@exa_mple.com")]     // _ interdit dans domaine
        [InlineData("alex..dupont@example.com")] // deux points consécutifs
        [InlineData("alex.@example.com")]     // point avant @
        [InlineData("")]                      // vide
        [InlineData(null)]                    // null
        public void CourrielInvalide_DoitLancerException(string courrielInvalide)
        {
            // Arrange
            var client = CreerClientValide();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => client.Courriel = courrielInvalide);
        }

    }
}
