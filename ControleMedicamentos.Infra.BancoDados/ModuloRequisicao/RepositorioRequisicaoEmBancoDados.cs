using ControleMedicamentos.Dominio.ModuloRequisicao;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public class RepositorioRequisicaoEmBancoDados
    {
        private const string enderecoBanco =
            "Data Source=MARCOS;Initial Catalog=ControleMedicamento_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private const string sqlInserir =
            @"INSERT INTO
                TBREQUISICAO
                    (
                        FUNCIONARIO_ID,
                        PACIENTE_ID,
                        MEDICAMENTO_ID,
                        QUANTIDADEMEDICAMENTO,
                        DATA
                    )
                    VALUES
                    (
                        @FUNCIONARIO_ID,
                        @PACIENTE_ID,
                        @MEDICAMENTO_ID,
                        @QUANTIDADEMEDICAMENTO,
                        @DATA
                    )";

        private const string sqlSelecionarRequisicaoPorId =
            @"SELECT        
                REQ.ID AS REQ_ID, 
                REQ.QUANTIDADEMEDICAMENTO, 
                REQ.DATA, FUNC.ID AS FUNC_ID, 

                FUNC.NOME, 
                FUNC.LOGIN, 
                FUNC.SENHA, 

                PAC.ID AS PAC_ID, 
                PAC.NOME AS PAC_NOME, 
                PAC.CARTAOSUS, 

                MED.ID AS MED_ID, 
                MED.NOME AS MED_NOME, 
                MED.DESCRICAO, 
                MED.LOTE, 
                MED.VALIDADE, 
                MED.QUANTIDADEDISPONIVEL, 

                FORN.ID AS FORN_ID, 
                FORN.NOME AS FORN_NOME, 
                FORN.TELEFONE, 
                FORN.EMAIL, 
                FORN.CIDADE, 
                FORN.ESTADO
            FROM            
                TBREQUISICAO AS REQ INNER JOIN
                TBFUNCIONARIO AS FUNC ON REQ.FUNCIONARIO_ID = FUNC.ID INNER JOIN
                TBPACIENTE AS PAC ON REQ.PACIENTE_ID = PAC.ID INNER JOIN
                TBMEDICAMENTO AS MED ON REQ.MEDICAMENTO_ID = MED.ID INNER JOIN
                TBFORNECEDOR AS FORN ON MED.FORNECEDOR_ID = FORN.ID";

        public ValidationResult Inserir(Requisicao novaRequisicao)
        {
            var validador = new ValidadorRequisicao();

            var resultadoValidacao = validador.Validate(novaRequisicao);

            if (!resultadoValidacao.IsValid)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosRequisicao(novaRequisicao, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novaRequisicao.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public Requisicao SelecionarPorId(int id)
        {



            return new Requisicao();
        }

        private void ConfigurarParametrosRequisicao(Requisicao novaRequisicao, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("@FUNCIONARIO_ID", novaRequisicao.Funcionario.Numero);
            comando.Parameters.AddWithValue("@PACIENTE_ID", novaRequisicao.Paciente.Numero);
            comando.Parameters.AddWithValue("@MEDICAMENTO_ID", novaRequisicao.Medicamento.Numero);
            comando.Parameters.AddWithValue("QUANTIDADEMEDICAMENTO", novaRequisicao.QtdMedicamento);
            comando.Parameters.AddWithValue("@DATA", novaRequisicao.Data);
        }
    }
}
