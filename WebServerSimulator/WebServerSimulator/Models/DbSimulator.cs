using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServerSimulator.Helpers;

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
            string[] doorIds = DbSimulatorHelper.LoadInFromTextFile("doors.txt");
            string[] cardIds = DbSimulatorHelper.LoadInFromTextFile("cards.txt");

            Doors = new List<Door>();
            Cards = new List<Card>();

            if (doorIds == null || cardIds == null)
                return;

            foreach (string doorId in doorIds)
            {
                Doors.Add(new Door {DoorId = doorId});
            }
            
            foreach (string cardId in cardIds)
            {
                Cards.Add(new Card { CardId = cardId });
            }
        }
    }
}
