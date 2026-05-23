using ClientManager.Api.Application.Commands;
using FluentValidation;

namespace ClientManager.Api.Application.Validations
{
    public class UpdateClientCommandValidation : AbstractValidator<UpdateClientCommand>
    {
        public UpdateClientCommandValidation()
        {
            RuleFor(x => x.IdentificationNumber).NotEmpty().WithMessage("El número de identificación es obligatorio");
            RuleFor(x => x.Body).NotNull().WithMessage("El cuerpo de la solicitud es obligatorio");
            RuleFor(x => x.Body.Name).MaximumLength(100).When(x => x.Body != null).WithMessage("El nombre no debe superar los 100 caracteres");
            RuleFor(x => x.Body.Phone).Matches("^[0-9]*$").When(x => x.Body != null && !string.IsNullOrEmpty(x.Body.Phone)).WithMessage("El teléfono solo debe contener números");
            RuleFor(x => x.Body.Address).MaximumLength(200).When(x => x.Body != null).WithMessage("La dirección no debe superar los 200 caracteres");
        }
    }
}
