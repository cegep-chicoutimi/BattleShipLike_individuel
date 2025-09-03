using BattleShip;
namespace Serveur
  
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GrilleBattle Grille = new GrilleBattle('x');

            Grille.Tir("D2", 'X');
            Grille.AfficherGrille();

        }
    }
}
