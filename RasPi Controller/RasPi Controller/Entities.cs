using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasPi_Controller
{
    public class RaspberryPi
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
    }

    public class Script
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
