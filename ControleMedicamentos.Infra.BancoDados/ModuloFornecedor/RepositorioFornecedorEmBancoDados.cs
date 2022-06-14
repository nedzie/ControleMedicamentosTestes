using ControleMedicamentos.Dominio.ModuloFornecedor;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFornecedor
{
    public class RepositorioFornecedorEmBancoDados
    {
        private const string enderecoBanco =
            "Data Source=MARCOS;Initial Catalog=ControleMedicamento_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private const string sqlInserir =
            @"INSERT INTO 
                TBFORNECEDOR
                    (
                        NOME,
                        TELEFONE,
                        EMAIL,
                        CIDADE,
                        ESTADO
                    )
                        VALUES
                    (
                        @NOME,
                        @TELEFONE,
                        @EMAIL,
                        @CIDADE,
                        @ESTADO
                    ); SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE
                TBFORNECEDOR
                    SET
                        NOME = @NOME,
                        TELEFONE = @TELEFONE,
                        EMAIL = @EMAIL,
                        CIDADE = @CIDADE,
                        ESTADO = @ESTADO
                    WHERE
                        ID = @ID";

        private const string sqlExcluir =
            @"DELETE FROM
                TBFORNECEDOR
                    WHERE
                        ID = @ID";

        private const string sqlSelecionarPorId =
            @"SELECT
                    ID,
                    NOME,
                    TELEFONE,
                    EMAIL,
                    CIDADE,
                    ESTADO
                FROM
                    TBFORNECEDOR
                WHERE
                    ID = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT
                    ID,
                    NOME,
                    TELEFONE,
                    EMAIL,
                    CIDADE,
                    ESTADO
                FROM
                    TBFORNECEDOR";

        public ValidationResult Inserir(Fornecedor novoFornecedor)
        {
            var validador = new ValidadorFornecedor();

            var resultadoValidacao = validador.Validate(novoFornecedor);

            if (!resultadoValidacao.IsValid)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosFornecedor(novoFornecedor, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoFornecedor.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Fornecedor fornecedor)
        {
            var validador = new ValidadorFornecedor();

            var resultadoValidacao = validador.Validate(fornecedor);

            if (!resultadoValidacao.IsValid)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosFornecedor(fornecedor, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public void Excluir(Fornecedor fornecedor)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", fornecedor.Numero);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        public List<Fornecedor> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();

            SqlDataReader leitorFornecedor = comandoSelecao.ExecuteReader();

            List<Fornecedor> fornecedores = new List<Fornecedor>();

            while (leitorFornecedor.Read())
                fornecedores.Add(ConverterParaFornecedor(leitorFornecedor));

            conexaoComBanco.Close();

            return fornecedores;
        }

        public Fornecedor SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();

            SqlDataReader leitorFornecedor = comandoSelecao.ExecuteReader();

            Fornecedor fornecedor = null;
            if (leitorFornecedor.Read())
                fornecedor = ConverterParaFornecedor(leitorFornecedor);

            conexaoComBanco.Close();

            return fornecedor;
        }

        private Fornecedor ConverterParaFornecedor(SqlDataReader leitorFornecedor)
        {
            int id = Convert.ToInt32(leitorFornecedor["ID"]);
            string nome = leitorFornecedor["NOME"].ToString();
            string telefone = leitorFornecedor["TELEFONE"].ToString();
            string email = leitorFornecedor["EMAIL"].ToString();
            string cidade = leitorFornecedor["CIDADE"].ToString();
            string estado = leitorFornecedor["ESTADO"].ToString();

            return new Fornecedor
            {
                Numero = id,
                Nome = nome,
                Telefone = telefone,
                Email = email,
                Cidade = cidade,
                Estado = estado
            };
        }

        private void ConfigurarParametrosFornecedor(Fornecedor novoFornecedor, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("@ID", novoFornecedor.Numero);
            comando.Parameters.AddWithValue("@NOME", novoFornecedor.Nome);
            comando.Parameters.AddWithValue("@TELEFONE", novoFornecedor.Telefone);
            comando.Parameters.AddWithValue("@EMAIL", novoFornecedor.Email);
            comando.Parameters.AddWithValue("@CIDADE", novoFornecedor.Cidade);
            comando.Parameters.AddWithValue("@ESTADO", novoFornecedor.Estado);
        }
    }
}
