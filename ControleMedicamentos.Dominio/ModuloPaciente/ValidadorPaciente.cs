using FluentValidation;

namespace ControleMedicamentos.Dominio.ModuloPaciente
{
    public class ValidadorPaciente : AbstractValidator<Paciente>
    {
        public ValidadorPaciente()
        {
            RuleFor(x => x.Nome)
                .NotNull().NotEmpty().MinimumLength(5);

            RuleFor(x => x.CartaoSUS)
                .NotNull().NotEmpty().MinimumLength(10);
        }
    }
}
