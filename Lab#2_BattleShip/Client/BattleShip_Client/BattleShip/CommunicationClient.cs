using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class CommunicationClient
    {
        private string donneeRecues = null;
        private byte[] donneesBrutes = new Byte[1024];
        private Socket communicationEntrante;
        private De_Serialisation de_Serialisation = new De_Serialisation();
        bool connexionEtablie;
        string erreur;

        public CommunicationClient()
        {

        }

        //  public void StartClient(string adresseServeur)

        public bool StartClient(string adresseServeur)
        {
            try
            {
                IPHostEntry infosAdresseServeur = Dns.GetHostEntry(adresseServeur);
                // Sélectionne la première adresse IPv4 disponible
                IPAddress adresseIp = infosAdresseServeur.AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

                if (adresseIp == null)
                {
                    Console.WriteLine("Aucune adresse IPv4 trouvée pour le serveur.");
                    connexionEtablie = false;
                    return false;
                }

                IPEndPoint adresseEntrante = new IPEndPoint(adresseIp, 22222);

                communicationEntrante = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                communicationEntrante.Connect(adresseEntrante);
                Console.WriteLine("Socket connected");
                connexionEtablie = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                connexionEtablie = false;
                communicationEntrante?.Shutdown(SocketShutdown.Both);
                communicationEntrante?.Close();
            }
            return connexionEtablie;
        }

        public bool EnvoisMessage(string messageTest)
        {
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(messageTest.ToString() + "|");
                Thread.Sleep(200);

                int bytesSent = communicationEntrante.Send(msg);
                return true;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("ArgumentNullException : {0}", erreur = "Le message à envoyer ne doit pas être vide.");
            }
            catch (SocketException)
            {
                Console.WriteLine("SocketException : {0}", erreur = "Le serveur est déconnecté! Impossible d'envoyer le message");
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected exception : {0}", erreur = "Un problème inattendu est arrivé! le message ne peut pas être envoyé!");
            }

            return false;
        }

        public string ReceptionMessage()
        {
            int octectsRecu;
            string messageRecu = "";

            try
            {

                octectsRecu = communicationEntrante.Receive(donneesBrutes);
                donneeRecues = Encoding.ASCII.GetString(donneesBrutes, 0, octectsRecu);

                messageRecu = donneeRecues.Substring(0, donneeRecues.IndexOf("|"));
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("ArgumentNullException : {0}", erreur = "Le message à envoyer ne doit pas être vide.");
            }
            catch (SocketException)
            {
                Console.WriteLine("SocketException : {0}", erreur = "Le serveur est déconnecté! Impossible d'envoyer le message");
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected exception : {0}", erreur = "Un problème inattendu est arrivé! le message ne peut pas être envoyé!");
            }

            return messageRecu;
        }
    }
}
