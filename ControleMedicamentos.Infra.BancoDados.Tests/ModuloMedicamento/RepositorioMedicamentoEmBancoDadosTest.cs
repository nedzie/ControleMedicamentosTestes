using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using FluentValidation.Results;
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

        public RepositorioMedicamentoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");

            med = new("Teste", "10 caracteres", "Teste", DateTime.Today);
            forn = new("Fornecedor", "49998287261", "contato@gmail.com", "Lages", "SC");


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

            medicamentoEncontrado.Validade = DateTime.SpecifyKind(medicamentoEncontrado.Validade, DateTimeKind.Local);

            Assert.IsNotNull(medicamentoEncontrado);

            Assert.AreEqual(medicamentoEncontrado, med);
        }
    }
}
