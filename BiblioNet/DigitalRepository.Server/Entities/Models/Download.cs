using DigitalRepository.Server.Entities.Interfaces;

namespace DigitalRepository.Server.Entities.Models
{
    public class Download : IEntity<long>
    {
        public long Id { get; set; }
        public long DocumentId { get; set; }
        public int  OperationType { get; set; }
        public string UserIp { get; set; } = string.Empty;
        public int State { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Document? Document { get; set; }
    }
}
