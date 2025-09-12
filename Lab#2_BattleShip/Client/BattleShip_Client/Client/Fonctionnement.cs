using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShip;

namespace Client
{
    internal class Fonctionnement
    {
        CommunicationClient communicationClient = new CommunicationClient();
        De_Serialisation deSe = new De_Serialisation();
        private string adresseServeur;
        bool reponse;
        bool demanderRejouer = true;
        bool rejouer = true;


        GrilleBattle maGrille = new GrilleBattle();
        GrilleBattle grilleAttaque = new GrilleBattle();
        GrilleBattle emplacementBateauAdversaire = new GrilleBattle();

        public void Sequence()
        {
            Console.WriteLine("\nVeuillez entrer l'adresse du serveur s'il vous plait : ");
            adresseServeur = Console.ReadLine() ?? string.Empty;

            reponse = communicationClient.StartClient(adresseServeur);
            if (!reponse)
            {
                Console.WriteLine("Connexion au serveur impossible.");
                return;
            }

            while (rejouer)
            {

                // Placement des bateaux
                Console.WriteLine("Placez vos bateaux (ex: A1 B1 pour un bateau horizontal) :");
                string coord = Console.ReadLine();
                while (!maGrille.PlacerBateau(coord))
                {
                    Console.WriteLine("Coordonnées invalides, réessayez :");
                    coord = Console.ReadLine();
                }

                // Après le placement des bateaux

                communicationClient.EnvoisMessage("Placement terminé");
                communicationClient.EnvoisMessage(deSe.Serialize(coord));
                string coordonneeAdversaire = deSe.Deserialize(communicationClient.ReceptionMessage());
                emplacementBateauAdversaire.PlacerBateau(coordonneeAdversaire);


                // Attendre la confirmation du serveur
                string confirmation = deSe.Deserialize(communicationClient.ReceptionMessage());
                if (confirmation != "pret")
                {
                    Console.WriteLine("Erreur de synchronisation avec le serveur.");
                    return;
                }

                while (!maGrille.bateauMort())
                {
                    Console.Clear();
                    Console.WriteLine("Grille d'attaque :");
                    grilleAttaque.AfficherGrilleTir();
                    Console.WriteLine("Votre grille de bateaux :");
                    maGrille.AfficherMaGrilleBateau();

                    // Tour du joueur : ENVOI DU TIR AU SERVEUR
                    Console.WriteLine("Entrez les coordonnées de votre tir (ex: A1) :");
                    string tir = Console.ReadLine();
                    while (!grilleAttaque.Tirer(tir))
                    {
                        Console.WriteLine("Coordonnées invalides ou déjà jouées, réessayez :");
                        tir = Console.ReadLine();
                    }
                    communicationClient.EnvoisMessage(deSe.Serialize(tir)); // Envoi du tir


                    string tirAdversaire = deSe.Deserialize(communicationClient.ReceptionMessage());
                    maGrille.Tirer(tirAdversaire);
                    Console.WriteLine($"L'adversaire a tiré en {tirAdversaire}");
                    communicationClient.EnvoisMessage(deSe.Serialize("résultat du tir")); // à adapter selon ta logique
                    maGrille.MettreAJourGrille(tirAdversaire);
                    if (maGrille.bateauMort())
                    {
                        Console.WriteLine("Dommage ! Votre bateau a été coulé !");
                        Console.WriteLine("\nVoulez vous rejouer ? ");
                        while (demanderRejouer)
                        {
                            Console.WriteLine("1.Oui");
                            Console.WriteLine("2.Non");
                            string choix = Console.ReadLine();
                            if (choix == "1")
                            {
                                communicationClient.EnvoisMessage(deSe.Serialize("oui"));
                                demanderRejouer = false;
                                rejouer = true;
                            }
                            else if (choix == "2")
                            {
                                communicationClient.EnvoisMessage(deSe.Serialize("non"));
                                demanderRejouer = false;
                                rejouer = false;
                            }
                            else
                            {
                                Console.WriteLine("Choix invalide, veuillez réessayer.");
                                demanderRejouer = true;
                            }
                        }
                        // break;
                    }
                }
            }



        }
    }
}
