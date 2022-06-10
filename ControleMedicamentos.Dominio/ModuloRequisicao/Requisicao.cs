using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloRequisicao
{
    public class Requisicao : EntidadeBase<Requisicao>
    {   
        public Medicamento Medicamento { get; set; }
        public Paciente Paciente { get; set; }
        public Funcionario Funcionario { get; set; }
        public int QtdMedicamento { get; set; }
        public DateTime Data { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Requisicao requisicao &&
                   Numero == requisicao.Numero &&
                   EqualityComparer<Medicamento>.Default.Equals(Medicamento, requisicao.Medicamento) &&
                   EqualityComparer<Paciente>.Default.Equals(Paciente, requisicao.Paciente) &&
                   EqualityComparer<Funcionario>.Default.Equals(Funcionario, requisicao.Funcionario) &&
                   QtdMedicamento == requisicao.QtdMedicamento &&
                   Data == requisicao.Data;
        }
    }
}
