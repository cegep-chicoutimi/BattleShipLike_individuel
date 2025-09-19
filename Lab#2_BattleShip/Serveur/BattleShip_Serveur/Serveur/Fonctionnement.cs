using BattleShip;

namespace Serveur
{
    public class Fonctionnement
    {
        CommunicationServeur communicationServeur = new CommunicationServeur();
        De_Serialisation deSe = new De_Serialisation();
        bool reponse;
        GrilleBattle maGrille;
        GrilleBattle grilleAttaque;
        bool rejouer = true;
        bool gagner;
        string error;

        public void Sequence()
        {
            Console.Clear();
            communicationServeur.StartListening();
            
            do
            {
                rejouer = true;
                while (rejouer)
                {
                    string tailleGrille = deSe.Deserialize(communicationServeur.ReceptionMessage(out reponse));
                    string[] tailles = tailleGrille.Split(',');
                    int lignes = int.Parse(tailles[0]);
                    int colonnes = int.Parse(tailles[1]);
                    Console.Clear();
                    maGrille = new GrilleBattle(lignes, colonnes);
                    grilleAttaque = new GrilleBattle(lignes, colonnes);
                    gagner = false;

                    // === PHASE DE PLACEMENT DES BATEAUX ===
                    Console.WriteLine("Placez vos bateaux (ex: A1 B1 pour un bateau horizontal) :");
                    string coord = Console.ReadLine();
                    while (!maGrille.PlacerBateau(coord, out error))
                    {
                        Console.WriteLine($"{error}, réessayez :");
                        coord = Console.ReadLine();
                    }

                    // === SYNCHRONISATION AVEC LE CLIENT ===
                    string messageClient = communicationServeur.ReceptionMessage(out reponse);
                    messageClient = messageClient.Trim().ToLower();
                    if (messageClient.Contains("ok"))
                    {
                        communicationServeur.EnvoisMessage(deSe.Serialize("pret"));
                    }
                    else
                    {
                        Console.WriteLine("Erreur de synchronisation avec le client.");
                        return;
                    }
                    // Recevoir les coordonnées de bateau du client et envoyer les siennes
                    string coordAdversaire = deSe.Deserialize(communicationServeur.ReceptionMessage(out reponse));
                    communicationServeur.EnvoisMessage(deSe.Serialize(coord));
                    // Placer les bateaux adverses dans emplacementBateau (grille d'attaque)
                    grilleAttaque.PlacerBateau(coordAdversaire, out error);

                    Console.Clear();
                    // === BOUCLE DE JEU ===
                    while (!gagner)
                    {
                        PrintGrilles();

                        // === RÉCEPTION DU TIR DU CLIENT ===
                        string tirClient = deSe.Deserialize(communicationServeur.ReceptionMessage(out reponse));
                        maGrille.MettreAJourGrille(tirClient);

                        PrintGrilles();
                        Console.WriteLine($"Le client a tiré en {tirClient}");
                        if (!maGrille.ToujoursVivant())
                        {
                            Console.WriteLine("Dommage, vous avez perdu !");
                            gagner = false;
                            break;
                        }

                        // === TOUR DU SERVEUR : ENVOI DU TIR AU CLIENT ===
                        Console.WriteLine("Entrez les coordonnées de votre tir (ex: A1) :");
                        string tirServeur = Console.ReadLine();
                        while (!grilleAttaque.Tirer(tirServeur, out error))
                        {
                            Console.WriteLine($"{error}");
                            tirServeur = Console.ReadLine();
                        }



                        communicationServeur.EnvoisMessage(deSe.Serialize(tirServeur));// Envoi du tir au client

                        if (grilleAttaque.bateauAdversaireMort())
                        {
                            Console.WriteLine("Bravo, vous avez gagné !");
                            gagner = true;
                            break;
                        }

                    }
                    // Informe le client qu'il a gagné si le serveur a perdu
                    if (!gagner)
                    {
                        communicationServeur.EnvoisMessage(deSe.Serialize("Gagné"));
                    }
                    // === RECEPTION DE REJOUER ===
                    string reponseClient = deSe.Deserialize(communicationServeur.ReceptionMessage(out reponse));
                    if (reponseClient == "1")
                    {
                        rejouer = true;
                    }
                    else if (reponseClient == "2")
                    {
                        rejouer = false;
                        Console.Clear();
                        communicationServeur.StartListening();
                    }


                }
            } while (true);
        }

        public void PrintGrilles()
        {
            Console.Clear();
            Console.WriteLine("--- Bataille navale ---\n");
            Console.WriteLine("Grille d'attaque :");
            grilleAttaque.AfficherGrilleTir();
            Console.WriteLine("\nVotre grille de bateaux :");
            maGrille.AfficherMaGrilleBateau();
            
        }
    }
}