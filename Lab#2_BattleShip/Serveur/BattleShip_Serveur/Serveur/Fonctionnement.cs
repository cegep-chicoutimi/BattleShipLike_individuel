using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShip;

namespace Serveur
{
    public class Fonctionnement
    {
        CommunicationServeur communicationServeur = new CommunicationServeur();
        De_Serialisation deSe = new De_Serialisation();
        bool reponse;
        GrilleBattle maGrille = new GrilleBattle();
        GrilleBattle grilleAttaque = new GrilleBattle();

        public void Sequence()
        {
            Console.Clear();
            communicationServeur.StartListening();

            // Placement des bateaux
            Console.WriteLine("Placez vos bateaux (ex: A1 B1 pour un bateau horizontal) :");
            string coord = Console.ReadLine();
            while (!maGrille.PlacerBateau(coord))
            {
                Console.WriteLine("Coordonnées invalides, réessayez :");
                coord = Console.ReadLine();
            }

            // Après le placement des bateaux
           
            string messageClient = communicationServeur.ReceptionMessage(out reponse);
            messageClient = messageClient.Trim().ToLower();
            Console.WriteLine(">> Message brut reçu : " + messageClient);
            Console.WriteLine(">> Message reçu du client : " + messageClient);
            if (messageClient.Contains("placement terminé"))
            {
                communicationServeur.EnvoisMessage(deSe.Serialize("pret"));
            }
            else
            {
                Console.WriteLine("Erreur de synchronisation avec le client.");
                return;
            }
                while (!maGrille.bateauMort())
                {
                    Console.Clear();
                    Console.WriteLine("Grille d'attaque :");
                    grilleAttaque.AfficherGrilleTir();
                    Console.WriteLine("Votre grille de bateaux :");
                    maGrille.AfficherMaGrilleBateau();

                    // RÉCEPTION DU TIR DU CLIENT
                    string tirClient = deSe.Deserialize(communicationServeur.ReceptionMessage(out reponse));
                    maGrille.Tirer(tirClient);
                    Console.WriteLine($"Le client a tiré en {tirClient}");
                    maGrille.MettreAJourGrille(tirClient);
                // ENVOI DU RÉSULTAT AU CLIENT
                //string resultatClient = maGrille.bateauMort() ? "coule" : "continue";
                //    communicationServeur.EnvoisMessage(deSe.Serialize(resultatClient));

                //    if (maGrille.bateauMort())
                //    {
                //        Console.WriteLine("Dommage ! Votre bateau a été coulé !");
                //        break;
                //    }

                    // TOUR DU SERVEUR : ENVOI DU TIR AU CLIENT
                    Console.WriteLine("Entrez les coordonnées de votre tir (ex: A1) :");
                    string tirServeur = Console.ReadLine();
                    while (!grilleAttaque.Tirer(tirServeur))
                    {
                        Console.WriteLine("Coordonnées invalides ou déjà jouées, réessayez :");
                        tirServeur = Console.ReadLine();
                    }
                    communicationServeur.EnvoisMessage(deSe.Serialize(tirServeur));

                    // RÉCEPTION DU RÉSULTAT DU CLIENT
                    string resultatServeur = deSe.Deserialize(communicationServeur.ReceptionMessage(out reponse));
                


                }
                if (grilleAttaque.bateauMort())
                {
                    Console.WriteLine("Félicitations ! Vous avez coulé le bateau ennemi !");
            }
            
            // Boucle de jeu principale
      
        }
    }
}