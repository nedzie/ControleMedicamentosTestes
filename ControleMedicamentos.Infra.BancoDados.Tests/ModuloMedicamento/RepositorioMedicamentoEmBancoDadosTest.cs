using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {
        Medicamento med;
        RepositorioMedicamentoEmBancoDados _repositorioMed;
        RepositorioFornecedorEmBancoDados _repositorioForn;
        Fornecedor forn;
        Fornecedor fornEditar;

        public RepositorioMedicamentoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");

            med = new("Teste", "10 caracteres", "Teste", DateTime.Today);
            forn = new("Fornecedor", "49998287261", "contato@gmail.com", "Lages", "SC");
            fornEditar = new("Fornecedor de editar", "49998000000", "editar@editar.com", "Editar", "ED");

            _repositorioMed = new();
            _repositorioForn = new();
        }
        [TestMethod]
        public void DeveConectarComBanco()
        {
            // arrange
            const string enderecoBanco =
            "Data Source=MARCOS;Initial Catalog=ControleMedicamento_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            const string ping = "SELECT 1";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand teste = new SqlCommand(ping, conexaoComBanco);

            // action
            conexaoComBanco.Open();

            var res = teste.ExecuteScalar();

            // assert
            Assert.AreEqual(1, Convert.ToInt32(res));
        }
        [TestMethod]
        public void DeveInserirMedicamento()
        {
            _repositorioForn.Inserir(forn);

            med.Fornecedor = forn;

            _repositorioMed.Inserir(med);

            var medicamentoEncontrado = _repositorioMed.SelecionarPorId(med.Numero);

            medicamentoEncontrado.Validade = DateTime.SpecifyKind(medicamentoEncontrado.Validade, DateTimeKind.Local); // Para ficar como "Local" ao invés de "Unespecified"

            Assert.IsNotNull(medicamentoEncontrado);

            Assert.AreEqual(med, medicamentoEncontrado);
        }

        [TestMethod]
        public void DeveEditarMedicamento()
        {
            _repositorioForn.Inserir(forn);
            _repositorioForn.Inserir(fornEditar);

            med.Fornecedor = forn;

            _repositorioMed.Inserir(med);

            med.Nome = "Dorflexxxxxx";
            med.Fornecedor = fornEditar;

            _repositorioMed.Editar(med);

            var medicamentoEncontrado = _repositorioMed.SelecionarPorId(med.Numero);

            Assert.IsNotNull(medicamentoEncontrado);

            Assert.AreEqual(med, medicamentoEncontrado);
        }

        [TestMethod]
        public void DeveExcluirMedicamento()
        {
            _repositorioForn.Inserir(forn);

            med.Fornecedor = forn;

            _repositorioMed.Inserir(med);

            _repositorioMed.Excluir(med.Numero);

            var medicamentoEncontrado = _repositorioMed.SelecionarPorId(med.Numero);

            Assert.IsNull(medicamentoEncontrado);
        }

        [TestMethod]
        public void DeveSelecionarApenasUm()
        {
            _repositorioForn.Inserir(forn);

            med.Fornecedor = forn;

            _repositorioMed.Inserir(med);

            var medicamentoEncontrado = _repositorioMed.SelecionarPorId(med.Numero);

            medicamentoEncontrado.Validade = DateTime.SpecifyKind(medicamentoEncontrado.Validade, DateTimeKind.Local); // Para ficar como "Local" ao invés de "Unespecified"

            Assert.IsNotNull(medicamentoEncontrado);

            Assert.AreEqual(med, medicamentoEncontrado);
        }

        [TestMethod]
        public void DeveSelecionarTodos()
        {
            _repositorioForn.Inserir(forn);

            med.Fornecedor = forn;

            int quantidade = 3;
            for (int i = 0; i < quantidade; i++)
                _repositorioMed.Inserir(med);

            var medicamentos = _repositorioMed.SelecionarTodos();

            foreach (var medicamento in medicamentos)
                medicamento.Validade = DateTime.SpecifyKind(medicamento.Validade, DateTimeKind.Local);

            Assert.IsNotNull(medicamentos);

            Assert.AreEqual(quantidade, medicamentos.Count);
        }
    }
}
