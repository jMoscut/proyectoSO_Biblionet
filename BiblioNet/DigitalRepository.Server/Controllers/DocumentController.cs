using DigitalRepository.Server.Config.Entities;
using DigitalRepository.Server.Services.Interfaces;
using Lombok.NET;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DigitalRepository.Server.Entities.Models;
using DigitalRepository.Server.Entities.Request;
using DigitalRepository.Server.Entities.Response;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using DigitalRepository.Server.Context;

namespace DigitalRepository.Server.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllArgsConstructor]
    [Authorize]
    public partial class DocumentController : CommonController
    {
        private readonly IEntityService<Document, DocumentRequest, long> _documentService;
        private readonly IOptions<Paths> _configPaths;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [HttpGet]
        public ActionResult Get(string? filters, bool thenInclude, int pageNumber = 1, int pageSize = 30)
        {
            var response = _documentService.GetAll(filters, thenInclude, pageNumber, pageSize);

            if (response.Success)
            {
                Response<List<DocumentResponse>> successResponse = new()
                {
                    Data = _mapper.Map<List<Document>, List<DocumentResponse>>(response.Data!),
                    Success = response.Success,
                    Message = response.Message,
                    TotalResults = response.TotalResults,
                };

                return Ok(successResponse);

            }

            Response<List<ValidationFailure>> errorResponse = new()
            {
                Data = response.Errors,
                Success = response.Success,
                Message = response.Message
            };

            return BadRequest(errorResponse);
        }

        [HttpGet("Download")]
        public IActionResult DownloadDocuments(string filters)
        {
            var response = _documentService.GetAll(filters,false,1,100);

            if (response.Success)
            {
                if (response.Data!.Count == 0)
                {
                    Response<List<ValidationFailure>> responseCount = new()
                    {
                        Data = response.Errors,
                        Success = response.Success,
                        Message = "Se requiere al menos un archivo para descargar"
                    };

                    return BadRequest(responseCount);
                }

                using var zipMemoryStream = new MemoryStream();
                // Crear un archivo zip en memoria
                using (var archive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var document in response.Data)
                    {
                        var documentPath = Path.Combine(_configPaths.Value.SaveDocuments, document.Path);

                        if (!System.IO.File.Exists(documentPath))
                        {

                            Response<List<ValidationFailure>> errorDocument = new()
                            {
                                Data = response.Errors,
                                Success = response.Success,
                                Message = $"El archivo {documentPath} no existe."
                            };

                            return BadRequest(errorDocument);
                        }

                        try
                        {
                            var fileName = Path.GetFileName(documentPath);
                            var zipEntry = archive.CreateEntry(fileName);

                            using var fileStream = new FileStream(documentPath, FileMode.Open, FileAccess.Read);
                            using var zipEntryStream = zipEntry.Open();
                            fileStream.CopyTo(zipEntryStream);

                            var download = new Download
                            {
                                UserIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ??
                                         "IP no disponible",
                                CreatedBy = GetUserId(),
                                State = 1,
                                CreatedAt = DateTime.UtcNow,
                                DocumentId = document.Id,
                                OperationType = 1,
                            };

                            _context.Downloads.Add(download);
                          
                        }
                        catch (Exception ex)
                        {
                            return StatusCode(500,
                                $"Error al agregar el archivo {documentPath} al archivo zip: {ex.Message}");
                        }
                    }
                }
                _context.SaveChanges();
                zipMemoryStream.Seek(0, SeekOrigin.Begin); 
                return File(zipMemoryStream.ToArray(), "application/zip", "documents.zip");
            }

            Response<List<ValidationFailure>> errorResponse = new()
            {
                Data = response.Errors,
                Success = response.Success,
                Message = response.Message
            };

            return BadRequest(errorResponse);
        }

        [HttpGet("Download/{id}")]
        public IActionResult DownloadDocument(long id)
        {
            var response = _documentService.GetById(id);

            if (response.Success)
            {
                string documentPath = Path.Combine(_configPaths.Value.SaveDocuments, response.Data!.Path);

                if (!System.IO.File.Exists(documentPath))
                {

                    Response<List<ValidationFailure>> errorDocument = new()
                    {
                        Data = response.Errors,
                        Success = response.Success,
                        Message = "El Documento no existe."
                    };

                    return BadRequest(errorDocument);
                }

                var download = new Download
                {
                    UserIp = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ??
                             "IP no disponible",
                    CreatedBy = GetUserId(),
                    State = 1,
                    CreatedAt = DateTime.UtcNow,
                    DocumentId = id,
                    OperationType = 1,
                };

                _context.Downloads.Add(download);
                _context.SaveChanges();


                var fileBytes = System.IO.File.ReadAllBytes(documentPath);
                var fileName = Path.GetFileName(documentPath);

                return File(fileBytes, "application/pdf", fileName);
            }

            Response<List<ValidationFailure>> errorResponse = new()
            {
                Data = response.Errors,
                Success = response.Success,
                Message = response.Message
            };

            return BadRequest(errorResponse);
        }

        [HttpGet("{id}")]
        public IActionResult GetDocument(long id)
        {
            var response = _documentService.GetById(id);

            if (response.Success)
            {
                Response<DocumentResponse> successResponse = new()
                {
                    Data = _mapper.Map<Document, DocumentResponse>(response.Data!),
                    Success = response.Success,
                    Message = response.Message
                };

                return Ok(successResponse);
            }

            Response<List<ValidationFailure>> errorResponse = new()
            {
                Data = response.Errors,
                Success = response.Success,
                Message = response.Message
            };

            return BadRequest(errorResponse);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateDocument([FromForm] DocumentRequest signatureRequest)
        {
            signatureRequest.CreatedBy = GetUserId();
            var response = _documentService.Create(signatureRequest);

            if (response.Success)
            {
                Response<DocumentResponse> successResponse = new()
                {
                    Data = _mapper.Map<Document, DocumentResponse>(response.Data!),
                    Success = response.Success,
                    Message = response.Message
                };

                return Ok(successResponse);
            }

            Response<List<ValidationFailure>> errorResponse = new()
            {
                Data = response.Errors,
                Success = response.Success,
                Message = response.Message
            };

            return BadRequest(errorResponse);
        }

        [HttpPut]
        public IActionResult UpdateDocument([FromForm] DocumentRequest signatureRequest)
        {
            signatureRequest.UpdatedBy = GetUserId();
            var response = _documentService.Update(signatureRequest);

            if (response.Success)
            {
                Response<DocumentResponse> successResponse = new()
                {
                    Data = _mapper.Map<Document, DocumentResponse>(response.Data!),
                    Success = response.Success,
                    Message = response.Message
                };

                return Ok(successResponse);
            }

            Response<List<ValidationFailure>> errorResponse = new()
            {
                Data = response.Errors,
                Success = response.Success,
                Message = response.Message
            };

            return BadRequest(errorResponse);
        }

        [HttpPatch]
        public IActionResult PartialUpdateDocument([FromForm] DocumentRequest signatureRequest)
        {
            signatureRequest.UpdatedBy = GetUserId();
            var response = _documentService.PartialUpdate(signatureRequest);

            if (response.Success)
            {
                Response<DocumentResponse> successResponse = new()
                {
                    Data = _mapper.Map<Document, DocumentResponse>(response.Data!),
                    Success = response.Success,
                    Message = response.Message
                };

                return Ok(successResponse);
            }

            Response<List<ValidationFailure>> errorResponse = new()
            {
                Data = response.Errors,
                Success = response.Success,
                Message = response.Message
            };

            return BadRequest(errorResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDocument(long id)
        {
            var response = _documentService.Delete(id, GetUserId());

            if (response.Success)
            {
                Response<DocumentResponse> successResponse = new()
                {
                    Data = _mapper.Map<Document, DocumentResponse>(response.Data!),
                    Success = response.Success,
                    Message = response.Message
                };

                return Ok(successResponse);
            }

            Response<List<ValidationFailure>> errorResponse = new()
            {
                Data = response.Errors,
                Success = response.Success,
                Message = response.Message
            };

            return BadRequest(errorResponse);
        }

    }
}
