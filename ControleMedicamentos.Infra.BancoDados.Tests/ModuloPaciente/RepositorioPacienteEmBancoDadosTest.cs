using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloPaciente
{
    [TestClass]
    public class RepositorioPacienteEmBancoDadosTest
    {
        Paciente paciente;
        RepositorioPacienteEmBancoDados _repositorioPaciente;

        public RepositorioPacienteEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBREQUISICAO; DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBPACIENTE; DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");

            paciente = new("Tobias de Antunes", "1234567890123");
            _repositorioPaciente = new();
        }

        [TestMethod]
        public void DeveInserirPaciente()
        {
            _repositorioPaciente.Inserir(paciente);

            var pacienteEncontrado = _repositorioPaciente.SelecionarPorId(paciente.Numero);

            Assert.IsNotNull(pacienteEncontrado);

            Assert.AreEqual(paciente, pacienteEncontrado);
        }

        [TestMethod]
        public void DeveEditarPaciente()
        {
            _repositorioPaciente.Inserir(paciente);

            paciente.Nome = "Josefa";
            paciente.CartaoSUS = "00000000000000";

            _repositorioPaciente.Editar(paciente);

            var pacienteEncontrado = _repositorioPaciente.SelecionarPorId(paciente.Numero);

            Assert.IsNotNull(pacienteEncontrado);

            Assert.AreEqual(paciente, pacienteEncontrado);
        }

        [TestMethod]
        public void DeveExcluirPaciente()
        {
            _repositorioPaciente.Inserir(paciente);

            _repositorioPaciente.Excluir(paciente);

            var pacienteEncontrado = _repositorioPaciente.SelecionarPorId(paciente.Numero);

            Assert.IsNull(pacienteEncontrado);
        }

        [TestMethod]
        public void DeveSelecionarUm()
        {
            _repositorioPaciente.Inserir(paciente);

            var pacienteEncontrado = _repositorioPaciente.SelecionarPorId(paciente.Numero);

            Assert.IsNotNull(pacienteEncontrado);

            Assert.AreEqual(paciente, pacienteEncontrado);
        }

        [TestMethod]
        public void DeveSelecionarTodos()
        {
            int quantidade = 3;
            
            for (int i = 0; i < quantidade; i++)
                _repositorioPaciente.Inserir(paciente);

            var pacientes = _repositorioPaciente.SelecionarTodos();

            Assert.AreEqual(quantidade, pacientes.Count);
        }
    }
}
