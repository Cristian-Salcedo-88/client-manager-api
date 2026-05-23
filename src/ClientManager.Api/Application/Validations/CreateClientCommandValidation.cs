using ClientManager.Api.Application.Commands;
using FluentValidation;

namespace ClientManager.Api.Application.Validations
{
    public class CreateClientCommandValidation : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100).WithMessage("El nombre es obligatorio y no debe superar los 100 caracteres");
            RuleFor(x => x.IdentificationNumber).NotEmpty().MaximumLength(50).WithMessage("El número de identificación es obligatorio y no debe superar los 50 caracteres");
            RuleFor(x => x.Phone).NotEmpty().Matches("^[0-9]*$").WithMessage("El teléfono es obligatorio y solo debe contener números");
            RuleFor(x => x.Address).NotEmpty().MaximumLength(200).WithMessage("La dirección es obligatoria y no debe superar los 200 caracteres");
        }
    }
}
