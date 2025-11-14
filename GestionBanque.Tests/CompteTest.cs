using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionBanque.Models;

namespace GestionBanque.Tests
{
    public class CompteTest
    {
        private Compte CreerCompte(double balanceDepart = 0)
        {
            return new Compte(
                id: 1,
                noCompte: "ABC123",
                balanceDepart: balanceDepart,
                clientId: 99
            );
        }

        [Fact]
        public void Deposer_MontantPositif_AugmenteBalance()
        {
            // Arrange
            var compte = CreerCompte(100);
            double montant = 50;

            // Act
            compte.Deposer(montant);

            // Assert
            Assert.Equal(150, compte.Balance);
        }

        [Fact]
        public void Deposer_MontantNegatif_DoitLancerException()
        {
            // Arrange
            var compte = CreerCompte(100);

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => compte.Deposer(-10));
        }

        [Fact]
        public void Deposer_MontantZero_DoitLancerException()
        {
            // Arrange
            var compte = CreerCompte(100);

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => compte.Deposer(0));
        }

        // ----------- TESTS RETIRER -----------

        [Fact]
        public void Retirer_MontantValide_DiminueBalance()
        {
            // Arrange
            var compte = CreerCompte(100);
            double montant = 40;

            // Act
            compte.Retirer(montant);

            // Assert
            Assert.Equal(60, compte.Balance);
        }

        [Fact]
        public void Retirer_MontantSuperieurBalance_DoitLancerException()
        {
            // Arrange
            var compte = CreerCompte(100);

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => compte.Retirer(150));
        }

        [Fact]
        public void Retirer_MontantNegatif_DoitLancerException()
        {
            // Arrange
            var compte = CreerCompte(100);

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => compte.Retirer(-10));
        }

        [Fact]
        public void Retirer_MontantZero_DoitLancerException()
        {
            // Arrange
            var compte = CreerCompte(100);

            // Act + Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => compte.Retirer(0));
        }
    }
}
