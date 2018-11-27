using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Bataille_navale_GitHub
{
    class Program
    {
        static void Main(string[] args)
        {
            int nbLignes = 10;
            int nbColonnes = 10;
            string recommencer;
            string[,] grilleJoueur = new string[2 * nbLignes + 1, nbColonnes + 1]; //la grille joueur contient à la fois les coups donnés par l'ordi et les bateaux du joueur
            string[,] grilleAttaque = new string[2 * nbLignes + 1, nbColonnes + 1];
            string[,] grilleOrdi = new string[2 * nbLignes + 1, nbColonnes + 1];
            int[][] bateauxJoueur = new int[5][];
            for (int i = 0; i < 5; i++)
            {
                bateauxJoueur[i] = new int[4];
            }
            int[][] bateauxOrdi = new int[5][];
            for (int i = 0; i < 5; i++)
            {
                bateauxOrdi[i] = new int[4];


            }
            string chemin = "C:\\Users\\Estelle\\Documents\\GitHub\\bataillenavale\\sauvegarde_partie_github.txt";
            do
            {
                string reprendrePartie = "n";
                StreamReader sauvegarde = new StreamReader(chemin);
                bool fichierVide = sauvegarde.EndOfStream; //la méthode EndOfStream teste si le curseur se trouve à la fin du fichier texte. Si dès l'ouverture du fichier, il est à la fin, c'est que le fichier est vide.
                sauvegarde.Close();
                if (fichierVide == false)
                {
                    Console.Write("Voulez-vous reprendre la partie sauvegardée? (o/n) ");
                    reprendrePartie = Console.ReadLine();
                }
                if (fichierVide == false && reprendrePartie == "o")
                {
                    ChargerPartie(chemin, ref grilleJoueur, ref grilleAttaque, ref grilleOrdi, ref bateauxJoueur, ref bateauxOrdi);
                }
                else
                {
                    grilleAttaque = InitialiserGrille(nbLignes, nbColonnes); //la grille attaque s'affichera et ne contient que les coups donnés par le joueur et non les bateaux de l'ordi
                    grilleOrdi = InitialiserGrille(nbLignes, nbColonnes); //la grille ordi contient à la fois les coups donnés par le joueur et les bateaux de l'ordi
                    string convenir;
                    do
                    {
                        grilleJoueur = InitialiserGrille(nbLignes, nbColonnes);
                        PlacerBateaux(grilleJoueur, nbLignes, nbColonnes, bateauxJoueur);
                        AfficherGrille(grilleJoueur, nbLignes, nbColonnes);
                        Console.Write("Ce placement vous convient-il? (o/n) : ");
                        convenir = Console.ReadLine();
                    }
                    while (convenir == "n");
                    PlacerBateaux(grilleOrdi, nbLignes, nbColonnes, bateauxOrdi);
                    Console.WriteLine("Voici respectivement la grille d'attaque des bateaux de l'adversaire et la grille de vos bateaux: ");
                    AfficherDeuxGrilles(grilleJoueur, grilleAttaque, nbLignes, nbColonnes);
                }

                int compteur = 0;
                bool resultatPartie = false;
                string choix;
                int joueur;
                int ordinateur;
                Console.Write("Souhaitez-vous commencer ? (o/n) ");
                choix = Console.ReadLine();
                if (choix == "o")
                {
                    joueur = 0;
                    ordinateur = 1;
                }
                else
                {
                    ordinateur = 0;
                    joueur = 1;
                }
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                do
                {
                    if (compteur % 2 == 0 && joueur == 0) //si joueur = 0, le joueur commence
                    {
                        Console.WriteLine("C'est à vous de jouer!");
                        resultatPartie = JouerJoueur(grilleOrdi, grilleAttaque, nbLignes, nbColonnes, bateauxOrdi);
                        AfficherDeuxGrilles(grilleJoueur, grilleAttaque, nbLignes, nbColonnes);
                    }
                    else
                    {
                        if (compteur % 2 == 0 && ordinateur == 0) //si ordinateur = 0, l'ordinateur commence
                        {
                            Console.Write("L'ordinateur joue...");
                            resultatPartie = JouerOrdi(grilleJoueur, nbLignes, nbColonnes, bateauxJoueur);
                            AfficherDeuxGrilles(grilleJoueur, grilleAttaque, nbLignes, nbColonnes);
                        }
                        else
                        {
                            if (compteur % 2 == 1 && joueur == 1)
                            {
                                Console.WriteLine("C'est à vous de jouer!");
                                resultatPartie = JouerJoueur(grilleOrdi, grilleAttaque, nbLignes, nbColonnes, bateauxOrdi);
                                AfficherDeuxGrilles(grilleJoueur, grilleAttaque, nbLignes, nbColonnes);
                            }
                            else
                            {
                                Console.Write("L'ordinateur joue...");
                                resultatPartie = JouerOrdi(grilleJoueur, nbLignes, nbColonnes, bateauxJoueur);
                                AfficherDeuxGrilles(grilleJoueur, grilleAttaque, nbLignes, nbColonnes);
                            }
                        }
                    }
                    Console.Write("Voulez-vous sauvegarder la partie? (o/n)");
                    string rep = Console.ReadLine();
                    if (rep == "o")
                    {
                        SauvegarderPartie(chemin, grilleJoueur, grilleAttaque, grilleOrdi, bateauxJoueur, bateauxOrdi, nbLignes, nbColonnes);
                    }
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    compteur++;
                }
                while (resultatPartie == false);
                Console.Write("Voulez-vous recommencer une partie? (o/n) ");
                recommencer = Console.ReadLine();
            }
            while (recommencer == "o");


            Console.ReadKey();
        }
        static string[,] InitialiserGrille(int nbLignes, int nbColonnes)
        {
            //arguments: nbLignes entier naturel, nbColonnes entier naturel
            //sortie: grille navale vide
            string[,] grille = new string[2 * nbLignes + 1, nbColonnes + 1];
            string empty = "   |";
            string bordure = "---+";
            //Console.WriteLine("    A   B   C   D   E   F   G   H   I   J");
            for (int i = 0; i < 2 * nbLignes + 1; i++)
            {
                if (i % 2 == 0)
                {
                    grille[i, 0] = "  +";
                    //Console.Write(grille[i, 0]);
                    for (int j = 1; j < nbColonnes + 1; j++)
                    {
                        grille[i, j] = bordure;
                        //Console.Write(grille[i, j]);
                    }
                }
                else
                {
                    if (i == 19) //Cas de la 10ème ligne --> pb: nombre à deux chiffres qui décale la grille d'où la suppression d'un espace
                    {
                        grille[i, 0] = "" + (i / 2 + 1) + "|";
                    }
                    else
                    {
                        grille[i, 0] = "" + (i / 2 + 1) + " |";
                    }
                    //Console.Write(grille[i, 0]);
                    for (int j = 1; j < nbColonnes + 1; j++)
                    {
                        grille[i, j] = empty;
                        //Console.Write(grille[i, j]);
                    }
                }
                //Console.WriteLine("");
            }
            return grille;
        }
        static void AfficherGrille(string[,] grille, int nbLignes, int nbColonnes)
        {
            //sortie: affiche la grille
            Console.WriteLine("    A   B   C   D   E   F   G   H   I   J");
            for (int i = 0; i < 2 * nbLignes + 1; i++)
            {
                for (int j = 0; j < nbColonnes + 1; j++)
                {
                    Console.Write(grille[i, j]);
                }
                Console.Write("\n");
            }
        }
        static void AfficherDeuxGrilles(string[,] grilleJoueur, string[,] grilleAttaque, int nbLignes, int nbColonnes)
        {
            string alphabet = "    A   B   C   D   E   F   G   H   I   J";
            string espace = "          ";
            Console.WriteLine(alphabet + "            " + alphabet);
            for (int i = 0; i < 2 * nbLignes + 1; i++)
            {
                for (int j = 0; j < nbColonnes + 1; j++)
                {
                    Console.Write(grilleAttaque[i, j]);
                }
                Console.Write(espace);
                for (int j = nbColonnes; j < 2 * nbColonnes + 1; j++)
                {
                    Console.Write(grilleJoueur[i, j - nbColonnes]);
                }
                Console.Write("\n");
            }
        }
        static void PlacerBateaux(string[,] grille, int nbLignes, int nbColonnes, int[][] bateaux)
        {
            //place aléatoirement les 5 bateaux sur une grille
            int nbLignesGrille = 2 * nbLignes + 1;
            int[] longueursBateaux = new int[5] { 5, 4, 3, 3, 2 };
            int[] portee = new int[5] { 6, 7, 8, 8, 9 }; //permet de ne pas sortir de la grille
            int n = longueursBateaux.GetLength(0);
            Random rnd = new Random();
            for (int k = 0; k < n; k++)
            {
                int orientation = rnd.Next(0, 2);
                int ligne;
                int ligneGrille;
                int colonne;
                int lBateau = longueursBateaux[k];

                bateaux[k][0] = orientation;
                bateaux[k][1] = lBateau;
                if (orientation == 0)
                {
                    ligne = rnd.Next(1, 11);
                    ligneGrille = 2 * ligne - 1;
                    colonne = rnd.Next(1, portee[k] + 1);
                    while (PlacementLigneLibre(grille, colonne, ligneGrille, lBateau) == false)
                    {
                        ligneGrille = 2 * rnd.Next(1, 11) - 1;
                        colonne = rnd.Next(1, portee[k] + 1);
                    }
                    bateaux[k][2] = ligneGrille;
                    bateaux[k][3] = colonne;
                    for (int j = 0; j < lBateau; j++)
                    {
                        grille[ligneGrille, colonne + j] = " X |";
                    }
                }
                else
                {
                    ligne = rnd.Next(1, portee[k] + 1);
                    ligneGrille = 2 * ligne - 1;
                    colonne = rnd.Next(1, 11);
                    while (PlacementColonneLibre(grille, colonne, ligneGrille, lBateau) == false)
                    {
                        ligneGrille = 2 * rnd.Next(1, portee[k] + 1) - 1;
                        colonne = rnd.Next(1, portee[k] + 1);
                    }
                    bateaux[k][2] = ligneGrille;
                    bateaux[k][3] = colonne;
                    for (int i = 0; i < lBateau; i++)
                    {
                        grille[ligneGrille + 2 * i, colonne] = " X |";
                    }
                }
            }
        }
        static bool PlacementLigneLibre(string[,] grille, int colonne, int ligneGrille, int lBateau)
        {
            //vérifie si la place est libre sur une ligne de la grille pour mettre un bateau de longueur lBateau en postion (ligneGrille,colonne)
            string empty = "   |";
            bool res = false;
            int j = 0;
            while (j < lBateau && grille[ligneGrille, colonne + j] == empty)
            {
                j++;
            }
            if (j == lBateau)
            {
                res = true;
            }
            return res;
        }
        static bool PlacementColonneLibre(string[,] grille, int colonne, int LigneGrille, int lBateau)
        {
            //vérifie si la place est libre sur une colonne de la grille pour mettre un bateau de longueur lBateau en postion (ligneGrille,colonne)
            string empty = "   |";
            bool res = false;
            int i = 0;
            while (i < lBateau && grille[LigneGrille + 2 * i, colonne] == empty)
            {
                i++;
            }
            if (i == lBateau)
            {
                res = true;
            }
            return res;
        }
        static bool JouerJoueur(string[,] grilleOrdi, string[,] grilleAttaque, int nbLignes, int nbColonnes, int[][] bateauxOrdi)
        {
            //fait jouer le joueur après le placement initial aléatoire des bateaux
            bool res = false;
            Console.Write("Choissisez la cellule où tirer: ");
            int[] cellule = StringToInt(Console.ReadLine());
            int ligne = 2 * cellule[1] - 1; //pour avoir la ligne de la grille
            int colonne = cellule[0] - 64; //conversion pour la table ASCII
            string empty = "   |";
            while (grilleOrdi[ligne, colonne] == " * |" || grilleOrdi[ligne, colonne] == " O |")
            {
                Console.Write("Déjà tenté! Choisissez une nouvelle cellule : ");
                cellule = StringToInt(Console.ReadLine());
                ligne = 2 * cellule[1] - 1;
                colonne = cellule[0] - 64;
            }

            if (grilleOrdi[ligne, colonne] == empty)
            {
                grilleOrdi[ligne, colonne] = " * |";
                grilleAttaque[ligne, colonne] = " * |";
                Console.WriteLine("A l'eau !!!");
            }
            else
            {
                grilleOrdi[ligne, colonne] = " O |";
                grilleAttaque[ligne, colonne] = " O |";
                Console.WriteLine("Touché !!!");
                if (Coule(grilleOrdi, ligne, colonne, bateauxOrdi) == true)
                {
                    Console.WriteLine("Bateau coulé!");
                    if (PartieGagnee(grilleOrdi, nbLignes, nbColonnes, bateauxOrdi) == true)
                    {
                        Console.WriteLine("Vous avez gagné!");
                        res = true;
                    }
                }
            }
            return res;
        }
        static bool JouerOrdi(string[,] grilleJoueur, int nbLignes, int nbColonnes, int[][] bateauxJoueur)
        {
            Random rnd = new Random();
            bool res = false;
            int ligne = rnd.Next(1, 11);
            int ligneGrille = 2 * ligne - 1;
            int colonne = rnd.Next(1, 11);
            string empty = "   |";
            while (grilleJoueur[ligneGrille, colonne] == " * |" || grilleJoueur[ligneGrille, colonne] == " O |")
            {
                Console.Write("Déjà tenté! Choisissez une nouvelle cellule : ");
                ligneGrille = 2 * rnd.Next(1, 11) - 1;
                colonne = rnd.Next(1, 11);
            }
            Console.WriteLine(" cellule {0}{1}", IntToChar(colonne), ligne);
            if (grilleJoueur[ligneGrille, colonne] == empty)
            {
                grilleJoueur[ligneGrille, colonne] = " * |";
                Console.WriteLine("A l'eau !!!");
            }
            else
            {
                grilleJoueur[ligneGrille, colonne] = " O |";
                Console.WriteLine("Touché !!!");
                if (Coule(grilleJoueur, ligneGrille, colonne, bateauxJoueur) == true)
                {
                    Console.WriteLine("Bateau coulé!");
                    if (PartieGagnee(grilleJoueur, nbLignes, nbColonnes, bateauxJoueur) == true)
                    {
                        Console.WriteLine("L'ordinateur a gagné!");
                        res = true;
                    }
                }
            }
            return res;
        }
        static int[] StringToInt(string chaine)
        {
            int[] cellule = new int[2];
            char[] caractere = chaine.ToCharArray();
            cellule[0] = Convert.ToInt32(caractere[0]);
            if (chaine.Length == 3)
            {
                cellule[1] = 10;
            }
            else
            {
                cellule[1] = (int)Char.GetNumericValue(caractere[1]);
            }
            return cellule;
        }
        static char IntToChar(int num)
        {
            char[] alphabet = new char[10] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            return alphabet[num - 1];
        }
        static bool Coule(string[,] grille, int ligne, int colonne, int[][] bateaux)
        {
            bool appartientBateau = false;
            bool bateauCoule = true;
            int i = 0;
            while (i < bateaux.GetLength(0) && appartientBateau == false)
            {
                int orientation = bateaux[i][0];
                int lBateau = bateaux[i][1];
                int ligneBateau = bateaux[i][2];
                int colonneBateau = bateaux[i][3];

                if (orientation == 0)
                {
                    if (ligne == ligneBateau && colonne >= colonneBateau && colonne <= colonneBateau + lBateau - 1)
                    {
                        appartientBateau = true;
                        int k = 0;
                        while (k < lBateau && bateauCoule == true)
                        {
                            if (grille[ligneBateau, colonneBateau + k] == " O |")
                            {
                                k++;
                            }
                            else
                            {
                                bateauCoule = false;
                            }
                        }
                    }
                }
                else
                {
                    if (colonne == colonneBateau && ligne >= ligneBateau && ligne <= ligneBateau + 2 * (lBateau - 1))
                    {
                        appartientBateau = true;
                        int k = 0;
                        while (k < lBateau && bateauCoule == true)
                        {
                            if (grille[ligneBateau + 2 * k, colonneBateau] == " O |")
                            {
                                k++;
                            }
                            else
                            {
                                bateauCoule = false;
                            }
                        }
                    }
                }
                i++;
            }

            return bateauCoule;
        }
        static bool PartieGagnee(string[,] grille, int nbLignes, int nbColonnes, int[][] bateaux)
        {
            bool res = true;
            int i = 0;
            while (i < bateaux.GetLength(0) && res == true)
            {
                int ligne = bateaux[i][2];
                int colonne = bateaux[i][3];
                if (Coule(grille, ligne, colonne, bateaux) == false)
                {
                    res = false;
                }
                i++;
            }
            return res;
        }
        static void SauvegarderPartie(string chemin, string[,] grilleJoueur, string[,] grilleAttaque, string[,] grilleOrdi, int[][] bateauxJoueur, int[][] bateauxOrdi, int nbLignes, int nbColonnes)
        {
            try
            {
                StreamWriter sauvegarde = new StreamWriter(chemin);
                SauvegarderGrille(sauvegarde, grilleJoueur, nbLignes, nbColonnes);
                SauvegarderTableau(sauvegarde, bateauxJoueur);
                SauvegarderGrille(sauvegarde, grilleAttaque, nbLignes, nbColonnes);
                SauvegarderGrille(sauvegarde, grilleOrdi, nbLignes, nbColonnes);
                SauvegarderTableau(sauvegarde, bateauxOrdi);
                sauvegarde.Close();
                Console.WriteLine("Votre partie a été sauvegardée avec succès.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        static void SauvegarderGrille(StreamWriter sauvegarde, string[,] grille, int nbLignes, int nbColonnes)
        {
            try
            {
                for (int i = 0; i < 2 * nbLignes + 1; i++)
                {
                    for (int j = 0; j < nbColonnes + 1; j++)
                    {
                        sauvegarde.Write(grille[i, j] + ";");
                    }
                    sauvegarde.Write("\r\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        static void SauvegarderTableau(StreamWriter sauvegarde, int[][] bateaux)
        {
            int nbLignes = 5; //bateaux.GetLength(0);
            int nbColonnes = 4; //bateaux.GetLength(1);
            try
            {
                for (int i = 0; i < nbLignes; i++)
                {
                    for (int j = 0; j < nbColonnes; j++)
                    {
                        sauvegarde.Write(bateaux[i][j] + ";");
                    }
                    sauvegarde.Write("\r\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        static void ChargerPartie(string chemin, ref string[,] grilleJoueur, ref string[,] grilleAttaque, ref string[,] grilleOrdi, ref int[][] bateauxJoueur, ref int[][] bateauxOrdi)
        {
            try
            {
                StreamReader sauvegarde = new StreamReader(chemin);
                grilleJoueur = ChargerGrille(sauvegarde);
                bateauxJoueur = ChargerTableau(sauvegarde);
                grilleAttaque = ChargerGrille(sauvegarde);
                grilleOrdi = ChargerGrille(sauvegarde);
                bateauxOrdi = ChargerTableau(sauvegarde);
                sauvegarde.Close();
                Console.WriteLine("Votre partie a été chargée avec succès.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        static string[,] ChargerGrille(StreamReader sauvegarde)
        {
            char[] separateur = new char[1] { ';' };
            int nbLignes = 10;
            int nbColonnes = 10;
            string[,] grille = new string[2 * nbLignes + 1, nbColonnes + 1];
            try
            {
                for (int i = 0; i < 2 * nbLignes + 1; i++)
                {
                    string ligne = sauvegarde.ReadLine();
                    string[] ligneGrille = new string[nbColonnes + 1];
                    ligneGrille = ligne.Split(separateur);
                    for (int j = 0; j < nbColonnes + 1; j++)
                    {
                        grille[i, j] = ligneGrille[j];
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            return grille;
        }
        static int[][] ChargerTableau(StreamReader sauvegarde)
        {
            char[] separateur = new char[1] { ';' };
            int[][] tableau = new int[5][];
            for (int k = 0; k < 5; k++)
            {
                tableau[k] = new int[4];
            }
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    string ligne = sauvegarde.ReadLine();
                    string[] ligneTableau = new string[4];
                    ligneTableau = ligne.Split(separateur);
                    for (int j = 0; j < 4; j++)
                    {
                        tableau[i][j] = Convert.ToInt32(ligneTableau[j]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            return tableau;
        }
    }
}
