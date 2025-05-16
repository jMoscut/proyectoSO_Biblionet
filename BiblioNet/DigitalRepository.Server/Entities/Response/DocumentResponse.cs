
namespace DigitalRepository.Server.Entities.Response
{
    public class DocumentResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public long Size { get; set; }
        public string ElaborationDate { get; set; } = string.Empty;
        public string LoadDate { get; set; } = string.Empty;
        public string UserIp { get; set; } = string.Empty;
        public int State { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string? UpdatedAt { get; set; }
    }
}
