using DigitalRepository.Server.Entities.Interfaces;

namespace DigitalRepository.Server.Entities.Request
{
    public class DocumentRequest : IRequest<long?>
    {
        public long? Id { get; set; }
        public string? Author { get; set; }
        public string? DocumentNumber { get; set; }
        public string? ElaborationDate { get; set; }
        public IFormFile? File { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
