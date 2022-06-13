using ControleMedicamentos.Dominio.ModuloMedicamento;
using FluentValidation.Results;
using System;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.ModuloMedicamento
{
    public class RepositorioMedicamentoEmBancoDados
    {
        private const string enderecoBanco =
            "Data Source=MARCOS;Initial Catalog=ControleMedicamento_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private const string sqlInserir =
            @"INSERT INTO 
                TBMEDICAMENTO
                    (
                        NOME,
                        DESCRICAO,
                        LOTE,
                        VALIDADE,
                        QUANTIDADEDISPONIVEL,
                        FORNECEDOR_ID
                    )
                        VALUES
                    (
                        @NOME,
                        @DESCRICAO,
                        @LOTE,
                        @VALIDADE,
                        @QUANTIDADEDISPONIVEL,
                        @FORNECEDOR_ID
                    ); SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE 
                TB_DISCIPLINA
                    SET
                        NOME = @NOME
                    WHERE 
                        NUMERO = @NUMERO";

        private const string sqlExcluir =
            @"DELETE FROM 
                TB_DISCIPLINA
                    WHERE
                        NUMERO = @NUMERO";

        private const string sqlSelecionarTodos =
            @"SELECT 
                    NUMERO, NOME 
                FROM 
                    TB_DISCIPLINA";

        private const string sqlSelecionarPorNumero =
            @"SELECT 
                    NUMERO,
                    NOME 
                FROM 
                    TB_DISCIPLINA
                WHERE
                    NUMERO = @NUMERO";

        public ValidationResult Inserir(Medicamento novoMedicamento)
        {
            var validador = new ValidadorMedicamento();

            var resultadoValidacao = validador.Validate(novoMedicamento);

            if (!resultadoValidacao.IsValid)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosMedicamento(novoMedicamento, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoMedicamento.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        private void ConfigurarParametrosMedicamento(Medicamento novoMedicamento, SqlCommand comandoInsercao)
        {
            comandoInsercao.Parameters.AddWithValue("@NOME", novoMedicamento.Nome);
            comandoInsercao.Parameters.AddWithValue("@DESCRICAO", novoMedicamento.Descricao);
            comandoInsercao.Parameters.AddWithValue("@LOTE", novoMedicamento.Lote);
            comandoInsercao.Parameters.AddWithValue("@VALIDADE", novoMedicamento.Validade);
            comandoInsercao.Parameters.AddWithValue("@QUANTIDADEDISPONIVEL", novoMedicamento.QuantidadeDisponivel);
            comandoInsercao.Parameters.AddWithValue("@FORNECEDOR_ID", novoMedicamento.Fornecedor.Numero);
        }
    }
}
