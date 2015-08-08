
namespace RasPi_Controller.Models
{
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
