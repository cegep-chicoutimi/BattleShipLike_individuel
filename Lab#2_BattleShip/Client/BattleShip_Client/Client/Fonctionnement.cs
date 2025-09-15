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
        GrilleBattle maGrille;
        GrilleBattle grilleAttaque;
        bool reponse;
        bool rejouer = true;
        bool gagner;
        bool demanderRejouer;

        public void Sequence()
        {
            // === CONNEXION AU SERVEUR ===
            Console.WriteLine("\nVeuillez entrer l'adresse du serveur s'il vous plait : ");
            adresseServeur = Console.ReadLine() ?? string.Empty;

            reponse = communicationClient.StartClient(adresseServeur);
            if (!reponse)
            {
                Console.WriteLine("Connexion au serveur impossible.");
                return;
            }

            // === BOUCLE PRINCIPALE ===
            while (rejouer)
            {
                Console.Clear();
                maGrille = new GrilleBattle();
                grilleAttaque = new GrilleBattle();

                gagner = false;
                // Placement des bateaux
                Console.WriteLine("Placez vos bateaux (ex: A1 B1 pour un bateau horizontal) :");
                string coord = Console.ReadLine();
                while (!maGrille.PlacerBateau(coord))
                {
                    Console.WriteLine("Coordonnées invalides, réessayez :");
                    coord = Console.ReadLine();
                }

                // Indiquer au serveur que le client est prêt
                communicationClient.EnvoisMessage(deSe.Serialize("OK"));

                // Attendre la confirmation du serveur
                string confirmation = deSe.Deserialize(communicationClient.ReceptionMessage());
                if (confirmation != "pret")
                {
                    Console.WriteLine("Erreur de synchronisation avec le serveur.");
                    return;
                }

                // Envoyer ses coordonnées de bateau au serveur et recevoir celles de cellui-ci
                communicationClient.EnvoisMessage(deSe.Serialize(coord));
                string coordonneeAdversaire = deSe.Deserialize(communicationClient.ReceptionMessage());
                // Placer les bateaux adverses dans emplacementBateau (grille d'attaque)
                grilleAttaque.PlacerBateau(coordonneeAdversaire);

                Console.Clear();
                while (!gagner)
                {
                    PrintGrilles();

                    // Tour du joueur
                    Console.WriteLine("Entrez les coordonnées de votre tir (ex: A1) :");
                    string tir = Console.ReadLine();
                    while (!grilleAttaque.Tirer(tir))
                    {
                        Console.WriteLine("Coordonnées invalides ou déjà jouées, réessayez :");
                        tir = Console.ReadLine();
                    }
                    PrintGrilles();

                    communicationClient.EnvoisMessage(deSe.Serialize(tir)); // Envoi du tir

                    if (grilleAttaque.bateauAdversaireMort())
                    {
                        Console.WriteLine("Bravo, vous avez gagné !");
                        gagner =true;
                        break;
                    }

                    // === TOUR DU SERVEUR ===
                    string tirAdversaire = deSe.Deserialize(communicationClient.ReceptionMessage());
                    maGrille.MettreAJourGrille(tirAdversaire);
                    Console.WriteLine($"L'adversaire a tiré en {tirAdversaire}");
                    if (!maGrille.ToujoursVivant())
                    {
                        Console.WriteLine("Dommage, vous avez perdu !");
                        gagner=false;
                        break;
                    }
                }

                if (gagner)
                {
                    string re = deSe.Deserialize(communicationClient.ReceptionMessage());

                }

                // === DEMANDE DE REJOUER ===
                do
                {
                    Console.WriteLine("Voulez-vous rejouer ?");
                    Console.WriteLine("1.Oui");
                    Console.WriteLine("2.Non");
                    string choix = Console.ReadLine();
                    if (choix == "1")
                    {
                        communicationClient.EnvoisMessage(deSe.Serialize("1"));
                        demanderRejouer = false;
                        rejouer = true;
                    }
                    else if (choix == "2")
                    {
                        communicationClient.EnvoisMessage(deSe.Serialize("2"));
                        demanderRejouer = false;
                        rejouer = false;
                    }
                    else
                    {
                        Console.WriteLine("Choix invalide, veuillez réessayer.");
                        demanderRejouer = true;
                    }
                }while (demanderRejouer);
            }



        }

        public void PrintGrilles()
        {
            Console.Clear();
            Console.WriteLine("___ Bataille navale ___\n");
            Console.WriteLine("Grille d'attaque :");
            grilleAttaque.AfficherGrilleTir();
            Console.WriteLine("\nVotre grille de bateaux :");
            maGrille.AfficherMaGrilleBateau();

        }
    }
}
