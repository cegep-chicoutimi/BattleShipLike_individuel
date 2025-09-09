
using System.Text.Json;

namespace BattleShip
{
    public class De_Serialisation
    {
        public De_Serialisation() { }
        /* Quand sacha le dresseur de Pokmon aura fini son code

                public static string Serialize(char[,] battleShipBoard)
                {
                    string jsonString = JsonConvert.SerializeObject(battleShipBoard);
                    return jsonString;
                }

                public static char[,] Deserialize(string jsonString)
                {
                    char[,] board = JsonConvert.DeserializeObject<char[,]>(jsonString);
                    return board;
                }*/
        // besoin de test Deserialisation
        public string Deserialize(string jsonString)
        {
            string messageTest = JsonSerializer.Deserialize<String>(jsonString);
            return messageTest;
        }

        // besoin de test Serialisation
        public string Serialize(string messag)
        {
            string jsonString = JsonSerializer.Serialize(messag);
            return jsonString;
        }
    }
}
