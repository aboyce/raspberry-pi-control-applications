using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServerSimulator.Models
{
    public class Card
    {
        public string CardId { get; set; }

        public override string ToString()
        {
            return CardId;
        }
    }
}
