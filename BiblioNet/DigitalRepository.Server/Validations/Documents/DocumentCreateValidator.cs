using DigitalRepository.Server.Entities.Request;
using DigitalRepository.Server.Validations.Common;
using FluentValidation;

namespace DigitalRepository.Server.Validations.Documents
{
    public class DocumentCreateValidator : CreateValidator<DocumentRequest, long?>
    {
        public DocumentCreateValidator()
        {
            RuleFor(x => x.File)
                .NotEmpty().WithMessage("El Archivo no puede ser vacio")
                .NotNull().WithMessage("El Archivo no puede ser nulo");
            RuleFor(x => x.File).Cascade(CascadeMode.Stop)
                .Must(file => file is { ContentType: "application/pdf" })
                .When(x => x.File != null)
                .WithMessage("El archivo debe ser un PDF.")
                .Must(file => file != null && file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                .When(x => x.File != null)
                .WithMessage("La extensión del archivo debe ser .pdf.");
            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("El Numero de documento es requerido");
            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("El Propietario del documento es requerido");
            RuleFor(x => x.ElaborationDate)
                .NotEmpty().WithMessage("La Fecha de eleaboracion del documento es requerida");

        }
    }
}
