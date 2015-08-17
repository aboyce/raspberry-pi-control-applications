using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServerSimulator.Models
{
    public class Door
    {
        public string DoorId { get; set; }

        public override string ToString()
        {
            return DoorId;
        }
    }
}
