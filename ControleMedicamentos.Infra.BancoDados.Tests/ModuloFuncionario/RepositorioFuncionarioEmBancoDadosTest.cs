using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFuncionario
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDadosTest
    {
        Funcionario func;
        RepositorioFuncionarioEmBancoDados _repositorioFuncionario;
        public RepositorioFuncionarioEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");

            _repositorioFuncionario = new();
            func = new("Gevásio", "gege123", "criptografada");
        }
        [TestMethod]
        public void DeveInserirFuncionario()
        {
            _repositorioFuncionario.Inserir(func);

            var funcionarioEncontrado = _repositorioFuncionario.SelecionarPorId(func.Numero);

            Assert.IsNotNull(funcionarioEncontrado);

            Assert.AreEqual(funcionarioEncontrado, func);
        }
        [TestMethod]
        public void DeveEditarFuncionario()
        {
            _repositorioFuncionario.Inserir(func);

            func.Nome = "Josias";
            func.Login = "jojo987";
            func.Senha = "descriptografada";

            _repositorioFuncionario.Editar(func);

            var funcionarioEncontrado = _repositorioFuncionario.SelecionarPorId(func.Numero);

            Assert.IsNotNull(funcionarioEncontrado);

            Assert.AreEqual(func, funcionarioEncontrado);
        }

        [TestMethod]
        public void DeveExcluirFuncionario()
        {
            _repositorioFuncionario.Inserir(func);

            _repositorioFuncionario.Excluir(func);

            var funcionarioEncontrado = _repositorioFuncionario.SelecionarPorId(func.Numero);

            Assert.IsNull(funcionarioEncontrado);
        }

        [TestMethod]
        public void DeveSelecionarApenasUm()
        {
            _repositorioFuncionario.Inserir(func);

            var funcionarioEncontrado = _repositorioFuncionario.SelecionarPorId(func.Numero);

            Assert.IsNotNull(funcionarioEncontrado);

            Assert.AreEqual(funcionarioEncontrado, func);
        }

        [TestMethod]
        public void DeveSelecionarTodos()
        {
            int quantidade = 3;
            for (int i = 0; i < quantidade; i++)
                _repositorioFuncionario.Inserir(func);

            var funcionarios = _repositorioFuncionario.SelecionarTodos();

            Assert.AreEqual(quantidade, funcionarios.Count);
        }
    }
}
