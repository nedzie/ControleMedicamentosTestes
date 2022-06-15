using FluentValidation;

namespace ControleMedicamentos.Dominio.ModuloRequisicao
{
    public class ValidadorRequisicao : AbstractValidator<Requisicao>
    {
        public ValidadorRequisicao()
        {
            RuleFor(x => x.Paciente)
                .NotNull();
            RuleFor(x => x.Funcionario)
                .NotNull();
            RuleFor(x => x.Medicamento)
                .NotNull();
            RuleFor(x => x.QtdMedicamento)
                .NotNull();
            RuleFor(x => x.Data)
                .NotNull().NotEmpty();
        }
    }
}
