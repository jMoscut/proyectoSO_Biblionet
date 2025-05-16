using DigitalRepository.Server.Config.Entities;
using DigitalRepository.Server.Entities.Response;
using DigitalRepository.Server.Utils.Interfaces;
using Lombok.NET;
using Microsoft.Extensions.Options;

namespace DigitalRepository.Server.Utils
{
    [AllArgsConstructor]
    public partial class Resources : IResources
    {
        private readonly IOptions<Paths> _pathResources;
        private readonly ILogger<Resources> _logger;
        public Response<string> SaveDocumentInServer(IFormFile? file, string previousValue)
        {
            Response<string> response = new();

            try
            {
                if (file == null)
                {
                    response.Message = "El archivo no es valido o no se cargo ningun archivo";
                    response.Success = false;
                    response.Data = string.Empty;

                    return response;
                }

                var appSettings = _pathResources.Value;
                string guid = Guid.NewGuid().ToString();
                string name = $"document_{guid}_{file.FileName}";
                string filePath = Path.Combine(appSettings.SaveDocuments, Path.GetFileName(name));
                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);

                if (!string.IsNullOrEmpty(previousValue))
                {
                    string previousPath = Path.Combine(appSettings.SaveDocuments, previousValue);
                    if (File.Exists(previousPath))
                        File.Delete(previousPath);
                }

                response.Data = name;
                response.Message = "Se cargo el documento con exito";
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.Message = $"Ocurrio un error al subir el archivo ({ex.Message})";
                response.Success = false;
                response.Data = string.Empty;

                _logger.LogError(ex, "Ocurrio un error al subir el archivo");
            }

            return response;
        }
    }
}
