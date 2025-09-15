using BattleShip;


namespace Projet_de_test
{
    [TestClass]
    public class GrilleBattleTests
    {
        [TestMethod]
        public void PlacementBateauValide()
        {
            var grille = new GrilleBattle();
            bool resultat = grille.PlacerBateau("A1 A2");
            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void PlacementBateauValide2()
        {
            var grille = new GrilleBattle();
            bool resultat = grille.PlacerBateau("C1 D1");
            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void PlacementBateauInvalide()
        {
            var grille = new GrilleBattle();
            bool resultat = grille.PlacerBateau("A8 A9");
            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void PlacementBateauInvalide2()
        {
            var grille = new GrilleBattle();
            bool resultat = grille.PlacerBateau("A8A9");
            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void PlacementBateauInvalide3()
        {
            var grille = new GrilleBattle();
            bool resultat = grille.PlacerBateau("A1 A3");
            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void TirValide()
        {
            var grille = new GrilleBattle();
            bool resultat = grille.Tirer("B4");
            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void TirInvalide()
        {
            var grille = new GrilleBattle();
            bool resultat = grille.Tirer("B9");
            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void TirInvalide9()
        {
            var grille = new GrilleBattle();
            bool resultat = grille.Tirer(",");
            Assert.IsFalse(resultat);
        }
    }
}
