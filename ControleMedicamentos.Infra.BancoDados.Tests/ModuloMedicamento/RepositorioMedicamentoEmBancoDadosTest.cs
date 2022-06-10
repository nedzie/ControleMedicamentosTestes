using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {
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
        public void Deve_inserir_medicamento()
        {
            Medicamento med = new("Teste", "10 caracteres", "Teste", DateTime.MaxValue);

            RepositorioMedicamentoEmBancoDados _repositorioMed = new();

            ValidationResult vr =_repositorioMed.Inserir(med);

            Assert.IsTrue(vr.IsValid);
        }
    }
}
