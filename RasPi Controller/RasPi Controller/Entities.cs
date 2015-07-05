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
        public string NetworkName { get; set; }
        public string IpAddress { get; set; }
        public string Username { get; set; }

        public override string ToString()
        {
            return string.Format("{0} [{1}, {2}]", Id, NetworkName, IpAddress);
        }
    }

    public class Script
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ArgumentFormat { get; set; }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Id, Name);
        }
    }
}
