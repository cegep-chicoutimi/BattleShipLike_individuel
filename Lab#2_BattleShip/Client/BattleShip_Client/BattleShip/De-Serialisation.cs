
using System.Text.Json;
namespace BattleShip
{
    public class De_Serialisation
    {

        public De_Serialisation() { }

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

        // Quand Sacha le dresseur de Pokemon ayra fini
        /*       public string Serialize(char[,] battleShipBoard)
              {
                  string jsonString = JsonConvert.SerializeObject(battleShipBoard);
                  return jsonString;
              }


             public char[,] Deserialize(string jsonString)
              {
                  char[,] board = JsonConvert.DeserializeObject<char[,]>(jsonString);
                  return board;
              }
            */

        // besoin de test Deserialisation
        //public string Deserialize(string jsonString)
        //{
        //    string messageTest = JsonConvert.DeserializeObject<String>(jsonString);
        //    return messageTest;
        //}

        //// besoin de test Serialisation
        //public string Serialize(string messag)
        //{
        //    string jsonString = JsonConvert.SerializeObject(messag);
        //    return jsonString;
        //}
    }
}
