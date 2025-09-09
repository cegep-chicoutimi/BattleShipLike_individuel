using BattleShip;

namespace Serveur
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CommunicationServeur communicationServeur = new CommunicationServeur();
            communicationServeur.StartListening();
            string json = System.Text.Json.JsonSerializer.Serialize("test serveur ");
            communicationServeur.EnvoisMessage(json);
            string r = communicationServeur.ReceptionMessage(out bool reponse);
            Console.WriteLine("\n------------");
            Console.WriteLine("\n------------");
            Console.WriteLine(r);
            Console.WriteLine("\n------------");
            Console.WriteLine("\n------------");
            communicationServeur.EnvoisMessage("test serveur |");
        }
    }
}
