using DigitalRepository.Server.Entities.Models;
using DigitalRepository.Server.Entities.Request;
using DigitalRepository.Server.Entities.Response;
using DigitalRepository.Server.Interceptors.Interfaces;
using DigitalRepository.Server.Services.Core;
using DigitalRepository.Server.Utils;
using DigitalRepository.Server.Utils.Interfaces;
using FluentValidation.Results;
using Lombok.NET;
using MapsterMapper;

namespace DigitalRepository.Server.Interceptors.DocumentInterceptor
{
    [AllArgsConstructor]
    [Order(2)]
    public partial class SaveDocumentServer : IEntityBeforeCreateInterceptor<Document, DocumentRequest>
    {
        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<EntityService<Document, DocumentRequest, long>> _logger;
        private readonly IResources _resources;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        /// <summary>
        /// The Execute
        /// </summary>
        /// <param name="response">The response<see cref="Response{TEntity}"/></param>
        /// <param name="request">The request<see cref="DocumentRequest"/></param>
        /// <returns>The <see cref="Response{Signature, List{ValidationFailure}}"/></returns>
        public Response<Document, List<ValidationFailure>> Execute(Response<Document, List<ValidationFailure>> response, DocumentRequest request)
        {
            try
            {
                Response<string> image = _resources.SaveDocumentInServer(request.File, "");

                if (!image.Success)
                {
                    response.Success = false;
                    response.Message = "Error al guardar documento en el servidor";
                    return response;
                }

                response.Success = true;
                response.Message = "Documento guardado correctamente";
                response.Data = _mapper.Map<DocumentRequest, Document>(request);
                response.Data.Path = image.Data!;
                response.Data.UserIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ??
                                       "IP no disponible";

                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Error al guardar el documento antes de crear en la bd";

                _logger.LogError(e, "Error al guardar del documento antes de crear en la bd {order}, Usuario: {user} Error: {error}", response.Data?.Id, response.Data?.CreatedBy, e.Message);

                return response;
            }
        }
    }
}
