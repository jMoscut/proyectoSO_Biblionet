using DigitalRepository.Server.Entities.Response;

namespace DigitalRepository.Server.Utils.Interfaces
{
    public interface IResources
    {
        public Response<string> SaveDocumentInServer(IFormFile? file, string previousValue);
    }
}
