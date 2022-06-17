using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]
    public class RepositorioFornecedorEmBancoDadosTest
    {
        RepositorioFornecedorEmBancoDados _repositorioFornecedor;
        Fornecedor fornecedor;
        public RepositorioFornecedorEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFORNECEDOR; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");

            fornecedor = new("Fornecedor", "49998287261", "contato@empresa.com", "Lages", "Santa Catarina");

            _repositorioFornecedor = new();
        }
        [TestMethod]
        public void DeveInserirFornecedor()
        {
            _repositorioFornecedor.Inserir(fornecedor);

            var fornecedorEncontrado = _repositorioFornecedor.SelecionarPorId(fornecedor.Numero);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor, fornecedorEncontrado);
        }

        [TestMethod]
        public void DeveEditarFornecedor()
        {
            _repositorioFornecedor.Inserir(fornecedor);

            fornecedor.Nome = "Josias";
            fornecedor.Email = "m@m.com";
            fornecedor.Telefone = "49998411948";
            fornecedor.Cidade = "Cerrito";
            fornecedor.Estado = "SC";

            _repositorioFornecedor.Editar(fornecedor);

            var fornecedorEncontrado = _repositorioFornecedor.SelecionarPorId(fornecedor.Numero);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor, fornecedorEncontrado);
        }

        [TestMethod]
        public void DeveExcluirFornecedor()
        {
            _repositorioFornecedor.Inserir(fornecedor);

            _repositorioFornecedor.Excluir(fornecedor);

            var fornecedorEncontrado = _repositorioFornecedor.SelecionarPorId(fornecedor.Numero);

            Assert.IsNull(fornecedorEncontrado);
        }

        [TestMethod]
        public void DeveSelecionarApenasUm()
        {
            _repositorioFornecedor.Inserir(fornecedor);

            var fornecedorEncontrado = _repositorioFornecedor.SelecionarPorId(fornecedor.Numero);

            Assert.IsNotNull(fornecedorEncontrado);
            Assert.AreEqual(fornecedor, fornecedorEncontrado);
        }

        [TestMethod]
        public void DeveSelecionarTodos()
        {
            Fornecedor f1 = new Fornecedor("Joãooooo", "49998287261", "1d112dd21d1j@j.com", "Andrada do Sul", "MG");
            Fornecedor f2 = new Fornecedor("Betooooo", "49998287261", "adawd42t2fb@b.com", "Pindamonhangaba", "SP");
            Fornecedor f3 = new Fornecedor("Sôniaaaaa", "49998287261", "sadquesefodawawdadw@s.com", "Peciri", "AL");

            _repositorioFornecedor.Inserir(f1);
            _repositorioFornecedor.Inserir(f2);
            _repositorioFornecedor.Inserir(f3);

            var fornecedores = _repositorioFornecedor.SelecionarTodos();

            Assert.AreEqual(3, fornecedores.Count);

            Assert.AreEqual(f1.Nome, fornecedores[0].Nome);
            Assert.AreEqual(f2.Nome, fornecedores[1].Nome);
            Assert.AreEqual(f3.Nome, fornecedores[2].Nome);
        }
    }
}
