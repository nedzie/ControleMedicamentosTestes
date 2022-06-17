using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
                    ); SELECT SCOPE_IDENTITY()";

        private const string sqlSelecionarTodos =
            @"SELECT        
                REQ.ID AS REQ_ID, 
                REQ.QUANTIDADEMEDICAMENTO AS REQ_QTD, 
                REQ.DATA AS REQ_DATA, 

                FUNC.ID AS FUNC_ID, 
                FUNC.NOME AS FUNC_NOME, 
                FUNC.LOGIN AS FUNC_LOGIN, 
                FUNC.SENHA AS FUNC_SENHA, 

                PAC.ID AS PAC_ID, 
                PAC.NOME AS PAC_NOME, 
                PAC.CARTAOSUS AS PAC_CARTAOSUS, 

                MED.ID AS MED_ID, 
                MED.NOME AS MED_NOME, 
                MED.DESCRICAO AS MED_DESC, 
                MED.LOTE AS MED_LOTE, 
                MED.VALIDADE AS MED_VAL, 
                MED.QUANTIDADEDISPONIVEL AS MED_QTD, 

                FORN.ID AS FORN_ID, 
                FORN.NOME AS FORN_NOME, 
                FORN.TELEFONE AS FORN_TEL, 
                FORN.EMAIL AS FORN_EMAIL, 
                FORN.CIDADE AS FORN_CID, 
                FORN.ESTADO AS FORN_EST
            FROM            
                TBREQUISICAO AS REQ INNER JOIN
                TBFUNCIONARIO AS FUNC ON REQ.FUNCIONARIO_ID = FUNC.ID INNER JOIN
                TBPACIENTE AS PAC ON REQ.PACIENTE_ID = PAC.ID INNER JOIN
                TBMEDICAMENTO AS MED ON REQ.MEDICAMENTO_ID = MED.ID INNER JOIN
                TBFORNECEDOR AS FORN ON MED.FORNECEDOR_ID = FORN.ID";

        private const string sqlSelecionarPorId =
            @"SELECT        
                REQ.ID AS REQ_ID, 
                REQ.QUANTIDADEMEDICAMENTO AS REQ_QTD, 
                REQ.DATA AS REQ_DATA, 

                FUNC.ID AS FUNC_ID, 
                FUNC.NOME AS FUNC_NOME, 
                FUNC.LOGIN AS FUNC_LOGIN, 
                FUNC.SENHA AS FUNC_SENHA, 

                PAC.ID AS PAC_ID, 
                PAC.NOME AS PAC_NOME, 
                PAC.CARTAOSUS AS PAC_CARTAOSUS, 

                MED.ID AS MED_ID, 
                MED.NOME AS MED_NOME, 
                MED.DESCRICAO AS MED_DESC, 
                MED.LOTE AS MED_LOTE, 
                MED.VALIDADE AS MED_VAL, 
                MED.QUANTIDADEDISPONIVEL AS MED_QTD, 

                FORN.ID AS FORN_ID, 
                FORN.NOME AS FORN_NOME, 
                FORN.TELEFONE AS FORN_TEL, 
                FORN.EMAIL AS FORN_EMAIL, 
                FORN.CIDADE AS FORN_CID, 
                FORN.ESTADO AS FORN_EST
            FROM            
                TBREQUISICAO AS REQ INNER JOIN
                TBFUNCIONARIO AS FUNC ON REQ.FUNCIONARIO_ID = FUNC.ID INNER JOIN
                TBPACIENTE AS PAC ON REQ.PACIENTE_ID = PAC.ID INNER JOIN
                TBMEDICAMENTO AS MED ON REQ.MEDICAMENTO_ID = MED.ID INNER JOIN
                TBFORNECEDOR AS FORN ON MED.FORNECEDOR_ID = FORN.ID
            WHERE
                REQ.ID = @ID";

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

        public List<Requisicao> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();

            SqlDataReader leitorRequisicoes = comandoSelecao.ExecuteReader();

            List<Requisicao> requisicoes = new List<Requisicao>();

            while (leitorRequisicoes.Read())
                requisicoes.Add(ConverterParaRequisicao(leitorRequisicoes));

            conexaoComBanco.Close();

            return requisicoes;
        }

        public Requisicao SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("@ID", id);

            conexaoComBanco.Open();

            SqlDataReader leitorRequisicao = comandoSelecao.ExecuteReader();

            Requisicao req = null;
            if (leitorRequisicao.Read())
                req = ConverterParaRequisicao(leitorRequisicao);

            conexaoComBanco.Close();

            return req;
        }

        private Requisicao ConverterParaRequisicao(SqlDataReader leitorRequisicao)
        {
            int id = Convert.ToInt32(leitorRequisicao["REQ_ID"]);
            int qtde = Convert.ToInt32(leitorRequisicao["REQ_QTD"]);
            DateTime dataReq = Convert.ToDateTime(leitorRequisicao["REQ_DATA"]);

            Funcionario f = ConveterParaFuncionario(leitorRequisicao);
            Medicamento m = ConveterParaMedicamento(leitorRequisicao);
            Paciente p = ConverterParaPaciente(leitorRequisicao);

            return new Requisicao
            {
                Numero = id,
                QtdMedicamento = qtde,
                Data = dataReq,
                Funcionario = f,
                Medicamento = m,
                Paciente = p
            };
        }

        private void ConfigurarParametrosRequisicao(Requisicao novaRequisicao, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("@REQ_ID", novaRequisicao.Numero);
            comando.Parameters.AddWithValue("@FUNCIONARIO_ID", novaRequisicao.Funcionario.Numero);
            comando.Parameters.AddWithValue("@PACIENTE_ID", novaRequisicao.Paciente.Numero);
            comando.Parameters.AddWithValue("@MEDICAMENTO_ID", novaRequisicao.Medicamento.Numero);
            comando.Parameters.AddWithValue("@QUANTIDADEMEDICAMENTO", novaRequisicao.QtdMedicamento);
            comando.Parameters.AddWithValue("@DATA", novaRequisicao.Data);
        }
        private Funcionario ConveterParaFuncionario(SqlDataReader leitorRequisicao)
        {
            int id = Convert.ToInt32(leitorRequisicao["FUNC_ID"]);
            string nome = leitorRequisicao["FUNC_NOME"].ToString();
            string login = leitorRequisicao["FUNC_LOGIN"].ToString();
            string senha = leitorRequisicao["FUNC_SENHA"].ToString();

            return new Funcionario
            {
                Numero = id,
                Nome = nome,
                Login = login,
                Senha = senha
            };
        }

        private Paciente ConverterParaPaciente(SqlDataReader leitorRequisicao)
        {
            int id = Convert.ToInt32(leitorRequisicao["PAC_ID"]);
            string nome = leitorRequisicao["PAC_NOME"].ToString();
            string cartaoSUS = leitorRequisicao["PAC_CARTAOSUS"].ToString();

            return new Paciente
            {
                Numero = id,
                Nome = nome,
                CartaoSUS = cartaoSUS
            };
        }

        private Medicamento ConveterParaMedicamento(SqlDataReader leitorRequisicao)
        {
            int idMed = Convert.ToInt32(leitorRequisicao["MED_ID"]);
            string nomeMed = leitorRequisicao["MED_NOME"].ToString();
            string descricao = leitorRequisicao["MED_DESC"].ToString();
            string lote = leitorRequisicao["MED_LOTE"].ToString();
            DateTime validade = Convert.ToDateTime(leitorRequisicao["MED_VAL"]);
            int qtde = Convert.ToInt32(leitorRequisicao["MED_QTD"]);

            return new Medicamento
            {
                Numero = idMed,
                Nome = nomeMed,
                Descricao = descricao,
                Lote = lote,
                Validade = validade,
                QuantidadeDisponivel = qtde,
                Requisicoes = new(),
                Fornecedor = ConverterParaFornecedor(leitorRequisicao)
            };

        }

        private Fornecedor ConverterParaFornecedor(SqlDataReader leitorRequisicao)
        {
            int idForn = Convert.ToInt32(leitorRequisicao["FORN_ID"]);
            string nomeForn = leitorRequisicao["FORN_NOME"].ToString();
            string telefone = leitorRequisicao["FORN_TEL"].ToString();
            string email = leitorRequisicao["FORN_EMAIL"].ToString();
            string cidade = leitorRequisicao["FORN_CID"].ToString();
            string estado = leitorRequisicao["FORN_EST"].ToString();

            return new Fornecedor
            {
                Numero = idForn,
                Nome = nomeForn,
                Telefone = telefone,
                Email = email,
                Cidade = cidade,
                Estado = estado
            };
        }



    }
}
