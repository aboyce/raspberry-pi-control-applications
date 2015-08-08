
namespace RasPi_Controller.Models
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
}
