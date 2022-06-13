using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.Compartilhado
{
    public class Db
    {
        private static string enderecoBanco =
            @"Data Source=MARCOS;Initial Catalog=ControleMedicamento_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void ExecutarSql(string sql)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comando = new SqlCommand(sql, conexaoComBanco);

            conexaoComBanco.Open();
            comando.ExecuteNonQuery();
            conexaoComBanco.Close();
        }
    }
}
