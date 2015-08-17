using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServerSimulator.Models
{
    public class DbSimulator
    {
        public List<Door> Doors;
        public List<Card> Cards;

        public DbSimulator()
        {
            PopulateWithData();
        }

        private void PopulateWithData()
        {
            Doors = new List<Door>
            {
                new Door { DoorId = "1"},
                new Door { DoorId = "2"},
                new Door { DoorId = "3"},
                new Door { DoorId = "4"}
            };

            Cards = new List<Card>()
            {
                new Card {CardId = "123456"},
                new Card {CardId = "789456"},
                new Card {CardId = "456132"},
                new Card {CardId = "147258"}
            };
        }
    }
}
