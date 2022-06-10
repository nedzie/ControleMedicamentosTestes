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
            const string enderecoBanco =
            "Data Source=MARCOS;Initial Catalog=ControleMedicamento_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            const string ping = "SELECT 1";

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand teste = new SqlCommand(ping, conexaoComBanco);

            conexaoComBanco.Open();

            var res = teste.ExecuteScalar();

            Assert.AreEqual(1, Convert.ToInt32(res));
        }
        [TestMethod]
        public void Deve_inserir_medicamento()
        {

        }
    }
}
