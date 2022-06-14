using ControleMedicamentos.Dominio.ModuloFuncionario;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFuncionario
{
    public class RepositorioFuncionarioEmBancoDados
    {
        private const string enderecoBanco =
            "Data Source=MARCOS;Initial Catalog=ControleMedicamento_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private const string sqlInserir =
            @"INSERT INTO
                TBFUNCIONARIO
                    (
                        NOME,
                        LOGIN,
                        SENHA
                    )
                    VALUES
                    (
                    @NOME,
                    @LOGIN,
                    @SENHA
                    ); SELECT SCOPE_IDENTITY();";

        private const string sqlEdicao =
            @"UPDATE
                TBFUNCIONARIO
                    SET 
                        NOME = @NOME,
                        LOGIN = @LOGIN,
                        SENHA = @SENHA
                    WHERE
                        ID = @ID";

        private const string sqlExcluir =
            @"DELETE
                FROM
                    TBFUNCIONARIO
                WHERE
                    ID = @ID";

        public const string sqlSelecionarTodos =
            @"SELECT
                ID,
                NOME,
                LOGIN,
                SENHA
            FROM
                TBFUNCIONARIO";

        private const string sqlSelecionarPorId =
            @"SELECT
                ID,
                NOME,
                LOGIN,
                SENHA
            FROM
                TBFUNCIONARIO
            WHERE
                ID = @ID";

        public ValidationResult Inserir(Funcionario novoFuncionario)
        {
            var validador = new ValidadorFuncionario();

            var resultadoValidacao = validador.Validate(novoFuncionario);

            if (!resultadoValidacao.IsValid)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosFuncionario(novoFuncionario, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoFuncionario.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Funcionario funcionario)
        {
            var validador = new ValidadorFuncionario();

            var resultadoValidacao = validador.Validate(funcionario);

            if (!resultadoValidacao.IsValid)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEdicao, conexaoComBanco);

            ConfigurarParametrosFuncionario(funcionario, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public void Excluir (Funcionario funcionario)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", funcionario.Numero);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        public List<Funcionario> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();

            SqlDataReader leitorFornecedor = comandoSelecao.ExecuteReader();

            List<Funcionario> funcionarios = new();

            while(leitorFornecedor.Read())
                funcionarios.Add(ConverterParaFuncionario(leitorFornecedor));

            conexaoComBanco.Close();

            return funcionarios;
        }

        public Funcionario SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();

            SqlDataReader leitorFuncionario = comandoSelecao.ExecuteReader();

            Funcionario func = null;

            if (leitorFuncionario.Read())
                func = ConverterParaFuncionario(leitorFuncionario);

            conexaoComBanco.Close();

            return func;
        }

        private Funcionario ConverterParaFuncionario(SqlDataReader leitorFuncionario)
        {
            int id = Convert.ToInt32(leitorFuncionario["ID"]);
            string nome = leitorFuncionario["NOME"].ToString();
            string login = leitorFuncionario["LOGIN"].ToString();
            string senha = leitorFuncionario["SENHA"].ToString();

            return new Funcionario
            {
                Numero = id,
                Nome = nome,
                Login = login,
                Senha = senha
            };
        }

        private void ConfigurarParametrosFuncionario(Funcionario novoFuncionario, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("@ID", novoFuncionario.Numero);
            comando.Parameters.AddWithValue("@NOME", novoFuncionario.Nome);
            comando.Parameters.AddWithValue("@LOGIN", novoFuncionario.Login);
            comando.Parameters.AddWithValue("@SENHA", novoFuncionario.Senha);
        }
    }
}
