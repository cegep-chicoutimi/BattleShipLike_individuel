using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class GrilleBattle
    {
        private char[,] emplacementBateau;
        private char[,] emplacement;
        private int lignes;
        private int colonnes;
        private string lettreDuTableau;

        // Nouveau constructeur avec lignes et colonnes
        public GrilleBattle(int lignes, int colonnes)
        {
            this.lignes = lignes;
            this.colonnes = colonnes;
            emplacementBateau = new char[lignes, colonnes];
            emplacement = new char[lignes, colonnes];
            lettreDuTableau = "";
            for (int i = 0; i < colonnes; i++)
                lettreDuTableau += (char)('A' + i);

            for (int x = 0; x < lignes; x++)
                for (int y = 0; y < colonnes; y++)
                    emplacement[x, y] = '-';
        }

        public bool ConvertirCoord(string coord, out int x, out int y, out string e)
        {
            x = -1;
            y = -1;
            if (string.IsNullOrWhiteSpace(coord) || coord.Length != 2)
            {
                e = "Coordonnées invalides: tir valide (ex: A1), réessayez";
                return false;
            }
            coord = coord.ToUpper();
            char col = coord[0];
            char row = coord[1];

            int colIndex = lettreDuTableau.IndexOf(col);
            if (colIndex == -1)
            {
                e = "Coordonnées invalides: colonne hors grille, réessayez";
                return false;
            }
            if (!char.IsDigit(row))
            {
                e = "Coordonnées invalides: ligne non numérique, réessayez";
                return false;
            }
            int rowIndex = (int)char.GetNumericValue(row) - 1;
            if (rowIndex < 0 || rowIndex >= lignes)
            {
                e = "Coordonnées invalides: ligne hors grille, réessayez";
                return false;
            }
            x = rowIndex;
            y = colIndex;
            e = "";
            return true;
        }

        public bool PlacerBateau(string coord, out string e)
        {
            if (string.IsNullOrWhiteSpace(coord))
            {
                e = "Coordonnées invalides: coordonnées vides, réessayez";
                return false;
            }
            string[] coords = coord.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (coords.Length != 2)
            {
                e = "Coordonnées invalides: vous devez donner deux coordonnées, réessayez";
                return false;
            }
            if (!ConvertirCoord(coords[0], out int x1, out int y1, out e))
            {
                return false;
            }
            if (!ConvertirCoord(coords[1], out int x2, out int y2, out e))
            {
                return false;
            }

            if (!SontAdjacentes(x1, y1, x2, y2))
            {
                e = "Coordonnées invalides: les coordonnées doivent être adjacentes, réessayez";
                return false;
            }

            if (emplacementBateau[x1, y1] == 'B' || emplacementBateau[x2, y2] == 'B')
            {
                e = "Coordonnées invalides: un bateau est déjà placé sur une des cases, réessayez";
                return false;
            }

            emplacementBateau[x1, y1] = 'B';
            emplacementBateau[x2, y2] = 'B';
            e = "";
            return true;
        }

        public bool SontAdjacentes(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2 && (y1 == y2 + 1 || y1 == y2 - 1))
                return true;
            if (y1 == y2 && (x1 == x2 + 1 || x1 == x2 - 1))
                return true;
            return false;
        }

        public bool Tirer(string coord, out string e)
        {
            if (!ConvertirCoord(coord, out int x, out int y, out e))
                return false;
            if (emplacement[x, y] == 'T' || emplacement[x, y] == 'X')
            {
                e = "Coordonnées invalides: vous avez déjà tiré sur cette case, réessayez";
                return false;
            }
            if (emplacementBateau[x, y] == 'B')
                emplacement[x, y] = 'T';
            else if (emplacement[x, y] == '-')
                emplacement[x, y] = 'X';
            e = "";
            return true;
        }

        public bool bateauAdversaireMort()
        {
            int count = 0;
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if (emplacement[i, j] == 'T')
                        count++;
                }
            }
            return count >= 2;
        }

        public bool ToujoursVivant()
        {
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if (emplacementBateau[i, j] == 'B')
                        return true;
                }
            }
            return false;
        }

        public void MettreAJourGrille(string coord)
        {
            string e;
            if (!ConvertirCoord(coord, out int x, out int y, out e))
                return;
            if (emplacementBateau[x, y] == 'B')
                emplacementBateau[x, y] = 'T';
            else
                emplacementBateau[x, y] = 'X';
        }

        public void AfficherMaGrilleBateau()
        {
            Console.Write("    ");
            foreach (char letter in lettreDuTableau)
            {
                Console.Write($" {letter}  ");
            }
            Console.WriteLine();
            Console.Write("  ┌");
            for (int i = 0; i < colonnes; i++)
            {
                Console.Write("───");
                if (i < colonnes - 1) Console.Write("┬");
            }
            Console.WriteLine("┐");
            for (int numLigne = 0; numLigne < lignes; numLigne++)
            {
                Console.Write($"{numLigne + 1} │");
                for (int barreHorizontale = 0; barreHorizontale < colonnes; barreHorizontale++)
                {
                    char symbole = emplacementBateau[numLigne, barreHorizontale];
                    if (symbole == '\0') symbole = '-';
                    Console.Write($" {symbole} │");
                }
                Console.WriteLine();
                if (numLigne < lignes - 1)
                {
                    Console.Write("  ├");
                    for (int i = 0; i < colonnes; i++)
                    {
                        Console.Write("───");
                        if (i < colonnes - 1) Console.Write("┼");
                    }
                    Console.WriteLine("┤");
                }
                else
                {
                    Console.Write("  └");
                    for (int i = 0; i < colonnes; i++)
                    {
                        Console.Write("───");
                        if (i < colonnes - 1) Console.Write("┴");
                    }
                    Console.WriteLine("┘");
                }
            }
        }

        public void AfficherGrilleTir()
        {
            Console.Write("    ");
            foreach (char letter in lettreDuTableau)
            {
                Console.Write($" {letter}  ");
            }
            Console.WriteLine();
            Console.Write("  ┌");
            for (int i = 0; i < colonnes; i++)
            {
                Console.Write("───");
                if (i < colonnes - 1) Console.Write("┬");
            }
            Console.WriteLine("┐");
            for (int numLigne = 0; numLigne < lignes; numLigne++)
            {
                Console.Write($"{numLigne + 1} │");
                for (int barreHorizontale = 0; barreHorizontale < colonnes; barreHorizontale++)
                {
                    char symbole = emplacement[numLigne, barreHorizontale];
                    Console.Write($" {symbole} │");
                }
                Console.WriteLine();
                if (numLigne < lignes - 1)
                {
                    Console.Write("  ├");
                    for (int i = 0; i < colonnes; i++)
                    {
                        Console.Write("───");
                        if (i < colonnes - 1) Console.Write("┼");
                    }
                    Console.WriteLine("┤");
                }
                else
                {
                    Console.Write("  └");
                    for (int i = 0; i < colonnes; i++)
                    {
                        Console.Write("───");
                        if (i < colonnes - 1) Console.Write("┴");
                    }
                    Console.WriteLine("┘");
                }
            }
        }
    }
}