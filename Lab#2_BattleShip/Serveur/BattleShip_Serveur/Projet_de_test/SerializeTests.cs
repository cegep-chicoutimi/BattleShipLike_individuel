using BattleShip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projet_de_test
{
    [TestClass]
    public class SerializeTests
    {
        [TestMethod]
        public void SerialisationValide()
        {
            var ser = new De_Serialisation();
            string message = "Perdu";
            string json = ser.Serialize(message);
            Assert.AreEqual("\"Perdu\"", json);
        }

        [TestMethod]
        public void SerialisationInvalide()
        {
            var ser = new De_Serialisation();
            string message = "Perdu";
            string json = ser.Serialize(message);
            Assert.AreNotEqual("\"Poutine\"", json);
        }
    }
}
