using DigitalRepository.Server.Entities.Interfaces;

namespace DigitalRepository.Server.Entities.Models
{
    public class Document : IEntity<long>
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public long   Size { get; set; }
        public DateTime ElaborationDate { get; set; }
        public DateTime LoadDate { get; set; }
        public string UserIp { get; set; } = string.Empty;
        public int State { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public User? User { get; set; }
        public virtual ICollection<Download> Downloads { get; set; } = new List<Download>();
    }
}
