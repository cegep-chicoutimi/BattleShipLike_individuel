
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
    }
}
