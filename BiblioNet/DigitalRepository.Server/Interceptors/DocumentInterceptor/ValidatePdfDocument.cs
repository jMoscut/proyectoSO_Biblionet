using DigitalRepository.Server.Entities.Models;
using DigitalRepository.Server.Entities.Request;
using DigitalRepository.Server.Entities.Response;
using DigitalRepository.Server.Interceptors.Interfaces;
using DigitalRepository.Server.Services.Core;
using DigitalRepository.Server.Utils;
using FluentValidation.Results;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Xobject;
using Lombok.NET;

namespace DigitalRepository.Server.Interceptors.DocumentInterceptor
{
    [AllArgsConstructor]
    [Order(1)]
    public partial class ValidatePdfDocument : IEntityBeforeCreateInterceptor<Document, DocumentRequest>
    {
        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<EntityService<Document, DocumentRequest, long>> _logger;

        /// <summary>
        /// The Execute
        /// </summary>
        /// <param name="response">The response<see cref="Response{TEntity}"/></param>
        /// <param name="request">The request<see cref="DocumentRequest"/></param>
        /// <returns>The <see cref="Response{Signature, List{ValidationFailure}}"/></returns>
        public Response<Document, List<ValidationFailure>> Execute(Response<Document, List<ValidationFailure>> response,
            DocumentRequest request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                {
                    response.Data = null;
                    response.Success = false;
                    response.Message = "El Pdf no puede estar vacío o nulo";
                    return response;
                }

                //open doc in read view to have it in memory 
                using var stream = request.File!.OpenReadStream();

                PdfReader reader = new PdfReader(stream);
                PdfDocument? pdfDoc = null;

                try
                {
                   pdfDoc = new PdfDocument(reader);
                }
                catch (PdfException)
                {
                    response.Data = null;
                    response.Success = false;
                    response.Message = "El Pdf está encriptado, carga un documento que sea valido para cargar.";
                    return response;
                }

                int totalPages = pdfDoc.GetNumberOfPages();
                if (totalPages == 0)
                {
                    response.Data = null;
                    response.Success = false;
                    response.Message = "El Pdf no tiene páginas";
                    return response;
                }

                for (int i = 1; i <= totalPages; i++)
                {
                    var page = pdfDoc.GetPage(i);
                    var content = PdfTextExtractor.GetTextFromPage(page);

                    if (!string.IsNullOrWhiteSpace(content)) break;

                    var resources = page.GetResources();
                    var xObjectNames = resources.GetResourceNames();

                    bool hasImage = false;
                    foreach (var name in xObjectNames)
                    {
                        string image = name.GetValue().ToLower();
                        if (image.Contains("image"))
                        {
                            hasImage = true;
                        }
                    }

                    if (hasImage)
                    {
                        break;
                    }

                    response.Data = null;
                    response.Success = false;
                    response.Message = "El Pdf no tiene contenido";
                    return response;
                }


                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Error al guardar el documento antes de crear en la bd";

                _logger.LogError(e,
                    "Error al guardar del documento antes de crear en la bd {order}, Usuario: {user} Error: {error}",
                    response.Data?.Id, response.Data?.CreatedBy, e.Message);

                return response;
            }
        }

    }
}
