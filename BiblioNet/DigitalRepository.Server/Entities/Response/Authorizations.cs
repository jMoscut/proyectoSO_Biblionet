using System.Reflection;

namespace DigitalRepository.Server.Entities.Response
{
    using Models;
    public class Authorizations
    {
        public Module?                Module     { get; set; }
        public ICollection<Operation> Operations { get; set; } = new List<Operation>();
    }
}
