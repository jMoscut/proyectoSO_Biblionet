using DigitalRepository.Server.Entities.Request;
using DigitalRepository.Server.Validations.Common;
using FluentValidation;

namespace DigitalRepository.Server.Validations.Documents
{
    public class DocumentUpdateValidator : UpdateValidator<DocumentRequest, long?>
    {
        public DocumentUpdateValidator()
        {
            RuleFor(x => x.File)
                .NotEmpty().WithMessage("El Archivo no puede ser vacio")
                .NotNull().WithMessage("El Archivo no puede ser nulo")
                .Must(x => x.Length > 0).WithMessage("El Archivo no puede ser vacio");
        }
    }
}
