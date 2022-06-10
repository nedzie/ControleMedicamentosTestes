using ControleMedicamentos.Dominio.ModuloFornecedor;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ValidationResult Inserir(Fornecedor novoFornecedor)
        {
            var validador = new ValidadorFornecedor();

            var resultadoValidacao = validador.Validate(novoFornecedor);

            if (!resultadoValidacao.IsValid)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosMedicamento(novoFornecedor, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoFornecedor.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }
    }
}
