using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class CommunicationServeur
    {
        string donneeRecues = null;
        byte[] donneesBrutes = new Byte[2024];
        IPAddress ipAddress = IPAddress.Any;
        IPEndPoint adresseEtPort;

        Socket communicationEntrante;
        Socket communication;

        public void StartListening()
        {
            adresseEtPort = new IPEndPoint(ipAddress, 22222);

            communicationEntrante = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                communicationEntrante.Bind(adresseEtPort);
                communicationEntrante.Listen(10);

                Console.WriteLine("...ATTENTE...");

                communication = communicationEntrante.Accept();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                communication.Shutdown(SocketShutdown.Both);
                communication.Close();
                communicationEntrante.Close();
            }

        }

        public bool EnvoisMessage(string messageTest)
        {

            try
            {
                byte[] msg = Encoding.UTF8.GetBytes(messageTest + "|");
                Thread.Sleep(200);

                int bytesSent = communication.Send(msg);
                return true;
            }
            catch (SocketException e)
            {
                Console.WriteLine("Un problème réseau est survenu!");
                communication.Shutdown(SocketShutdown.Both);
                communication.Close();
                communicationEntrante.Close();

                Console.WriteLine();

                Console.WriteLine(".......Appuyer sur une touche........");
                Console.ReadKey();

                return false;
            }
        }

        public string ReceptionMessage(out bool response)
        {
            int octectsRecu;
            string messageRecu = "";

            try
            {

                octectsRecu = communication.Receive(donneesBrutes);
                donneeRecues = Encoding.UTF8.GetString(donneesBrutes, 0, octectsRecu);

                int index = donneeRecues.IndexOf("|");
                messageRecu = index >= 0 ? donneeRecues.Substring(0, index) : donneeRecues;
                response = true;
            }
            catch (SocketException e)
            {
                Console.WriteLine("Un problème réseau est survenu!");
                communication.Shutdown(SocketShutdown.Both);
                communication.Close();
                communicationEntrante.Close();

                Console.WriteLine();

                Console.WriteLine(".......Appuyer sur une touche........");
                Console.ReadKey();
                response = false;
            }

            return messageRecu;
        }
    }
}
