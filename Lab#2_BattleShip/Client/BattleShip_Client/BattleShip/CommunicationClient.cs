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

        public CommunicationClient()
        {

        }

        //  public void StartClient(string adresseServeur)

        public bool StartClient(string adresseServeur)
        {
            try
            {
                IPHostEntry infosAdresseServeur = Dns.Resolve(adresseServeur);
                IPAddress adresseIp = infosAdresseServeur.AddressList[1];
                // on prend l'lement 1 quaund on fait les test en local et c'est à cette position
                // que se trouve l'adresse ipv4
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
                communicationEntrante.Shutdown(SocketShutdown.Both);
                communicationEntrante.Close();
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
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
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
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

            return messageRecu;
        }
    }
}
