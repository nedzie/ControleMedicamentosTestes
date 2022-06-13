using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloRequisicao;
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

        private const string sqlSelecionarPorId =
            @"SELECT        
                MED.ID AS M_ID, 
                MED.NOME AS M_NOME, 
                MED.DESCRICAO AS M_DESCRICAO, 
                MED.LOTE AS M_LOTE, 
                MED.VALIDADE AS M_VALIDADE, 
                MED.QUANTIDADEDISPONIVEL AS M_QUANTIDADEDISPONIVEL, 
                FORN.ID AS F_ID, 
                FORN.NOME AS F_NOME, 
                FORN.TELEFONE AS F_TELEFONE, 
                FORN.EMAIL AS F_EMAIL, 
                FORN.CIDADE AS F_CIDADE, 
                FORN.ESTADO AS F_ESTADO
                    FROM            
                TBMEDICAMENTO AS MED INNER JOIN
                         TBFORNECEDOR AS FORN ON MED.FORNECEDOR_ID = FORN.ID
                WHERE
                    MED.ID = @ID";

        private const string sqlSelecionarRequisicoes =
            @"SELECT         
                TBREQUISICAO.ID AS R_ID,  
                TBREQUISICAO.QUANTIDADEMEDICAMENTO AS R_QUANTIDADEMEDICAMENTO,  
                TBREQUISICAO.DATA AS R_DATA,  
                TBFUNCIONARIO.ID AS F_ID,  
                TBFUNCIONARIO.NOME AS F_NOME, 
                TBFUNCIONARIO.LOGIN AS F_LOGIN,  
                TBFUNCIONARIO.SENHA AS F_SENHA,  
                TBPACIENTE.ID AS P_ID,  
                TBPACIENTE.NOME AS P_NOME,  
                TBPACIENTE.CARTAOSUS AS P_CARTAOSUS
                    FROM             
                        TBREQUISICAO INNER JOIN
                          TBFUNCIONARIO ON TBREQUISICAO.FUNCIONARIO_ID = TBFUNCIONARIO.ID INNER JOIN
                          TBPACIENTE ON  TBREQUISICAO.PACIENTE_ID =  TBPACIENTE.ID
                    WHERE
                        TBREQUISICAO.MEDICAMENTO_ID = @ID";

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

        public Medicamento SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();

            SqlDataReader leitorMedicamento = comandoSelecao.ExecuteReader();

            Medicamento med = null;
            if (leitorMedicamento.Read())
                med = ConverterParaMedicamento(leitorMedicamento);

            conexaoComBanco.Close();

            return med;
        }

        public void CarregarRequisicoes(Medicamento med)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarRequisicoes, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", med.Numero);

            conexaoComBanco.Open();

            SqlDataReader leitorRequisicoes = comandoSelecao.ExecuteReader();

            while (leitorRequisicoes.Read())
            {
                Requisicao req = ConverterParaRequisicao(leitorRequisicoes, med);
                med.Requisicoes.Add(req);
            }
        }

        private Requisicao ConverterParaRequisicao(SqlDataReader leitorRequisicoes, Medicamento med)
        {
            throw new NotImplementedException();
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

        private Medicamento ConverterParaMedicamento(SqlDataReader leitorMedicamento)
        {
            int idMed = Convert.ToInt32(leitorMedicamento["M_ID"]);
            string nomeMed = leitorMedicamento["M_NOME"].ToString();
            string descricao = leitorMedicamento["M_DESCRICAO"].ToString();
            string lote = leitorMedicamento["M_LOTE"].ToString();
            DateTime validade = Convert.ToDateTime(leitorMedicamento["M_VALIDADE"]);
            int qtde = Convert.ToInt32(leitorMedicamento["M_QUANTIDADEDISPONIVEL"]);

            int idForn = Convert.ToInt32(leitorMedicamento["F_ID"]);
            string nomeForn = leitorMedicamento["F_NOME"].ToString();
            string telefone = leitorMedicamento["F_TELEFONE"].ToString();
            string email = leitorMedicamento["F_EMAIL"].ToString();
            string cidade = leitorMedicamento["F_CIDADE"].ToString();
            string estado = leitorMedicamento["F_CIDADE"].ToString();

            return new Medicamento
            {
                Numero = idMed,
                Nome = nomeMed,
                Descricao = descricao,
                Lote = lote,
                Validade = validade,
                QuantidadeDisponivel = qtde,
                Fornecedor = new Fornecedor
                {
                    Numero = idForn,
                    Nome = nomeForn,
                    Telefone = telefone,
                    Email = email,
                    Cidade = cidade,
                    Estado = estado
                }
            };
        }
    }
}
