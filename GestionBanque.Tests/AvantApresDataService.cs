using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Reflection;
using Xunit.Sdk;

namespace GestionBanque.Tests
{
    /// <summary>
    /// Cette classe permet de devenir un décorateur dans les tests sur les DataService.
    /// Si le décorateur [AvantApresDataService(cheminBd)] est mis au dessus d'une méthode
    /// de tests, alors la BD de tests sera supprimée avant et après chaque test. La suppression
    /// avant un test est seulement là en guise de sécurité. 
    /// 
    /// Voir ClientSqliteDataServiceTest.cs ou CompteSqliteDataServiceTest.cs pour un exemple.
    /// </summary>
    public class AvantApresDataService : BeforeAfterTestAttribute
    {
        private string _cheminFichierBd;

        public AvantApresDataService(string cheminFichierBd)
        {
            _cheminFichierBd = cheminFichierBd;
        }

        public override void Before(MethodInfo methodUnderTest)
        {
            Debug.WriteLine("Avant le test " + methodUnderTest);
            SupprimerBd();
        }

        public override void After(MethodInfo methodUnderTest)
        {
            Debug.WriteLine("Après le test " + methodUnderTest);
            SupprimerBd();
        }

        private void SupprimerBd()
        {
            try
            {
                SqliteConnection.ClearAllPools();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            { 
                if (File.Exists(_cheminFichierBd)) File.Delete(_cheminFichierBd);
            }
        }
    }
}