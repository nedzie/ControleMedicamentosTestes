using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
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
        public RepositorioMedicamentoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");

            med = new("Teste", "10 caracteres", "Teste", DateTime.MaxValue);

            _repositorioMed = new();
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
            ValidationResult vr =_repositorioMed.Inserir(med);

            Assert.IsTrue(vr.IsValid);
        }
    }
}
