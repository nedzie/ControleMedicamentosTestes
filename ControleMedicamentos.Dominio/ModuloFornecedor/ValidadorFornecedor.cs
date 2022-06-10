using FluentValidation;
using FluentValidation.Validators;

namespace ControleMedicamentos.Dominio.ModuloFornecedor
{
    public class ValidadorFornecedor : AbstractValidator<Fornecedor>
    {
        public ValidadorFornecedor()
        {
            RuleFor(x => x.Nome)
                .NotNull().NotEmpty().MinimumLength(5);
            RuleFor(x => x.Telefone)
                .NotNull().NotEmpty().MinimumLength(10).MaximumLength(11);
            RuleFor(x => x.Email)
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible).NotNull().NotEmpty();
        }
    }
}
