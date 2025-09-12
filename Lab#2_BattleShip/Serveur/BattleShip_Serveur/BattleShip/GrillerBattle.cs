using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class GrilleBattle
    {
        /* Tableau dans lequel on va contenir nos symboles, on a choisi
         B pour beateau; X pour bateau touché et O dans le cas où la 
         case est vide. Deplus ce sera lui qui va nous permettre de 
         savoir au fure et à mesure de remplir notre grille et sera envoyé
         du client au serveur plus du serveur au client
        */

        private char[,] emplacementBateau = new char[4, 4];
        private char[,] emplacement = new char[4, 4];
        

        string lettreDuTableau = "ABCD";

        // au debut ce tableau est vide car personne n'a encore joué donc...

        public GrilleBattle()
        {
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    emplacement[x, y] = '-';
        }

        public bool PlacerBateau(string coord)
        {
            if (string.IsNullOrWhiteSpace(coord))
                return false;

            string[] coords = coord.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (coords.Length != 2)
                return false;

            if (!ConvertirCoord(coords[0], out int x1, out int y1)) return false;
            if (!ConvertirCoord(coords[1], out int x2, out int y2)) return false;

            if (!SontAdjacentes(x1, y1, x2, y2))
                return false;

            if (emplacementBateau[x1, y1] == 'B' || emplacementBateau[x2, y2] == 'B')
                return false;

            emplacementBateau[x1, y1] = 'B';
            emplacementBateau[x2, y2] = 'B';
            return true;
        }



        public bool SontAdjacentes(int x1, int y1, int x2, int y2)
        {
            // même ligne, colonnes consécutives
            if (x1 == x2 && (y1 == y2 + 1 || y1 == y2 - 1))
                return true;

            // même colonne, lignes consécutives
            if (y1 == y2 && (x1 == x2 + 1 || x1 == x2 - 1))
                return true;

            return false;
        }



        public bool ConvertirCoord(string coord, out int x, out int y)
        {
            x = -1;
            y = -1;
            if (string.IsNullOrWhiteSpace(coord) || coord.Length != 2)
                return false;

            coord = coord.ToUpper();
            char col = coord[0];
            char row = coord[1];

            // Conversion colonne
            int colIndex = lettreDuTableau.IndexOf(col);
            if (colIndex == -1)
                return false;

            // Conversion ligne
            if (!char.IsDigit(row))
                return false;
            int rowIndex = (int)char.GetNumericValue(row) - 1;
            if (rowIndex < 0 || rowIndex >= emplacement.GetLength(0))
                return false;

            x = rowIndex;
            y = colIndex;
            return true;
        }

        public bool Tirer(string coord)
        {
            if (!ConvertirCoord(coord, out int x, out int y))
                return false; // coordonnées invalides

            if (emplacement[x, y] == 'T' || emplacement[x, y] == 'X')
                return false; // déjà joué

            if (emplacementBateau[x, y] == 'B')
                emplacement[x, y] = 'T'; // touché
            else if (emplacement[x, y] == '-')
                emplacement[x, y] = 'X'; // raté

            return true;
        }

        public bool bateauMort()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (emplacement[i, j] == 'T')
                    {
                        if ((j<4&&(emplacement[i, j+1] == 'T')) || (i<4&&(emplacement[i+1,j] == 'T')) || (j>0&&(emplacement[i, j-1] == 'T')) || (i < 0 && (emplacement[i - 1, j] == 'T')))
                            return true;


                    }
                }
            }
            return false; 
        }

        public void MettreAJourGrille(string coord)
        {
            if (!ConvertirCoord(coord, out int x, out int y))
                return;

            if (emplacementBateau[x,y]=='B')
                emplacementBateau[x,y] = 'T';
            else
                emplacementBateau[x, y] = 'X';
        }



        public void AfficherMaGrilleBateau()
        {
            Console.Write("    ");
            foreach (char letter in "ABCD")
            {
                Console.Write($" {letter}  ");
            }
            Console.WriteLine();
            Console.WriteLine("  ┌───┬───┬───┬───┐");
            for (int numLigne = 0; numLigne < 4; numLigne++)
            {
                Console.Write($"{numLigne + 1} │");
                for (int barreHorizontale = 0; barreHorizontale < 4; barreHorizontale++)
                {
                    char symbole = emplacementBateau[numLigne, barreHorizontale];
                    if (symbole == '\0') symbole = '-'; // case vide
                    Console.Write($" {symbole} │");
                }
                Console.WriteLine();
                if (numLigne < 3)
                    Console.WriteLine("  ├───┼───┼───┼───┤");
                else
                    Console.WriteLine("  └───┴───┴───┴───┘");
            }
        }

        public void AfficherGrilleTir()
        {
            Console.Write("    ");
            foreach (char letter in "ABCD")
            {
                Console.Write($" {letter}  ");
            }
            Console.WriteLine();
            Console.WriteLine("  ┌───┬───┬───┬───┐");
            for (int numLigne = 0; numLigne < 4; numLigne++)
            {
                Console.Write($"{numLigne + 1} │");
                for (int barreHorizontale = 0; barreHorizontale < 4; barreHorizontale++)
                {
                    char symbole = emplacement[numLigne, barreHorizontale];
                    Console.Write($" {symbole} │");
                }
                Console.WriteLine();
                if (numLigne < 3)
                    Console.WriteLine("  ├───┼───┼───┼───┤");
                else
                    Console.WriteLine("  └───┴───┴───┴───┘");
            }
        }






    }
}
