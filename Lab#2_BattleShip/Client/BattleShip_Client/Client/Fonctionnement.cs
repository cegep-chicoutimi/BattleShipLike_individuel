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
        
        public void Sequence()
        {
            Console.WriteLine("\nVeuillez entrer l'adresse du serveur s'il vous plait : ");
            adresseServeur = Console.ReadLine() ?? string.Empty;

           reponse = communicationClient.StartClient(adresseServeur);

            // recepetion et deserialisation du message du serveur


            // envois de message serialiser une fois que ce sera fait

            //test communication


        
           // Console.ForegroundColor = ConsoleColor.Black;

            while (reponse)
            {
                string messageRecu = deSe.Deserialize(communicationClient.ReceptionMessage());
                Console.WriteLine("\n------------");
                Console.WriteLine("\n------------");
                Console.WriteLine("Message reçu venant du serveur : " + messageRecu);
                Console.WriteLine("\n------------");
                Console.WriteLine("\n------------");

              reponse =  communicationClient.EnvoisMessage(deSe.Serialize("message en provenance du  CLIENT"));












                // pour linstant reponse message = false juste pour les besoin de test 
               // reponse = false;

            }


        }
    }
}
