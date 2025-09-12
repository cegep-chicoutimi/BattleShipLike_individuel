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
        GrilleBattle GrilleBateauAdversaire = new GrilleBattle();
        bool rejouer = true;

        public void Sequence()
        {
            Console.Clear();
            communicationServeur.StartListening();

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

                string messageClient = communicationServeur.ReceptionMessage(out reponse);
                messageClient = messageClient.Trim().ToLower();
                Console.WriteLine(">> Message brut reçu : " + messageClient);
                Console.WriteLine(">> Message reçu du client : " + messageClient);
                if (messageClient.Contains("ok"))
                {
                    Console.WriteLine(">> on est entré");
                    communicationServeur.EnvoisMessage(deSe.Serialize("pret"));
                }
                else
                {
                    Console.WriteLine("Erreur de synchronisation avec le client.");
                    return;
                }
                string coordAdversaire = deSe.Deserialize(communicationServeur.ReceptionMessage(out reponse));
                communicationServeur.EnvoisMessage(deSe.Serialize(coord));
                grilleAttaque.PlacerBateau(coordAdversaire);
                while (!grilleAttaque.bateauAdversaireMort())
                {
                    Print();

                    // RÉCEPTION DU TIR DU CLIENT127
                    string tirClient = deSe.Deserialize(communicationServeur.ReceptionMessage(out reponse));
                    maGrille.MettreAJourGrille(tirClient);

                    Print();
                    Console.WriteLine($"Le client a tiré en {tirClient}");

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
                    // string resultatServeur = deSe.Deserialize(communicationServeur.ReceptionMessage(out reponse));



                }
                if (grilleAttaque.bateauAdversaireMort())
                {
                    Console.WriteLine("Félicitations ! Vous avez coulé le bateau ennemi !");

                    communicationServeur.EnvoisMessage(deSe.Serialize("Voulez vous rejouer?"));

                    string resultatServeur = deSe.Deserialize(communicationServeur.ReceptionMessage(out reponse));

                    if (resultatServeur == "1")
                    {
                        rejouer = true;
                    }
                    else
                    {
                        rejouer = false;
                    }
                }

                // Boucle de jeu principale

            }

        }

        public void Print()
        {
            Console.Clear();
            Console.WriteLine("Grille d'attaque :");
            grilleAttaque.AfficherGrilleTir();
            Console.WriteLine("Votre grille de bateaux :");
            maGrille.AfficherMaGrilleBateau();
            
        }
    }
}