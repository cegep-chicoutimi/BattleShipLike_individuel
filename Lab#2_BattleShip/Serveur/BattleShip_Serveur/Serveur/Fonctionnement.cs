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

        bool response = true;
        De_Serialisation de_Serial = new De_Serialisation();
        CommunicationServeur communicationServeur = new CommunicationServeur();

        public void Sequence()
        {
            Console.Clear();
            communicationServeur.StartListening();

            /*   if (d == true)
               {
                   Console.WriteLine("Message reçu");
                   response = communicationServeur.EnvoisMessage("Message bien reçu");
                   if (response == true)
                   {
                       Console.WriteLine("Message envoyé");
                   }
                   else
                   {
                       Console.WriteLine("Erreur lors de l'envoi du message");
                   }
               }
               else
               {
                   Console.WriteLine("Erreur lors de la réception du message");
               }*/

            //Test echnage message
            while (response)
            {

                if (!communicationServeur.EnvoisMessage(de_Serial.Serialize("TEST message venant du serveur .")))
                {
                    Console.WriteLine(" Erreur lors de l'envoi du message."); break;
                }


                // Réception brute
                try
                {
                    string message = de_Serial.Deserialize(communicationServeur.ReceptionMessage(out response));
                    Console.WriteLine("Message reçu en provenance du client : " + message);
                    // pour besoin de test reponses = false;
                   // response = false;   
                }
                catch (Exception)
                {
                    Console.WriteLine(" Erreur ou déconnexion du client.");
                    break;
                }
            }


        }

        public void Reset()
        {
            // A la fin d'une partie on remt à zéro les variables
        }


    }
}
