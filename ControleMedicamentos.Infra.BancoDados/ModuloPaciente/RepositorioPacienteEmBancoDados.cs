using ControleMedicamentos.Dominio.ModuloPaciente;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamentos.Infra.BancoDados.ModuloPaciente
{
    public class RepositorioPacienteEmBancoDados
    {
        private const string enderecoBanco =
            "Data Source=MARCOS;Initial Catalog=ControleMedicamento_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private const string sqlInserir =
            @"INSERT INTO
                TBPACIENTE
                    (
                        NOME, 
                        CARTAOSUS
                    )
                    VALUES
                    (
                        @NOME, 
                        @CARTAOSUS
                    ); SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
            @"UPDATE
                TBPACIENTE
                    SET
                        NOME = @NOME,
                        CARTAOSUS = @CARTAOSUS
                    WHERE
                        ID = @ID";

        private const string sqlExcluir =
            @"DELETE 
                FROM
                    TBPACIENTE
                WHERE
                    ID = @ID";

        private const string sqlSelecionarTodos =
            @"SELECT
                    ID,
                    NOME,
                    CARTAOSUS
                FROM
                    TBPACIENTE";

        private const string sqlSelecionarPorId =
            @"SELECT
                    ID,
                    NOME,
                    CARTAOSUS
                FROM
                    TBPACIENTE
                WHERE
                    ID = @ID";

        public ValidationResult Inserir(Paciente novoPaciente)
        {
            var validador = new ValidadorPaciente();

            var resultadoValidacao = validador.Validate(novoPaciente);

            if (!resultadoValidacao.IsValid)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosPaciente(novoPaciente, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoPaciente.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Paciente paciente)
        {
            var validador = new ValidadorPaciente();

            var resultadoValidacao = validador.Validate(paciente);

            if (!resultadoValidacao.IsValid)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosPaciente(paciente, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public void Excluir(Paciente paciente)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("@ID", paciente.Numero);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        public List<Paciente> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();

            SqlDataReader leitorPacientes = comandoSelecao.ExecuteReader();

            List<Paciente> pacientes = new List<Paciente>();

            while (leitorPacientes.Read())
                pacientes.Add(ConverterParaPaciente(leitorPacientes));

            conexaoComBanco.Close();

            return pacientes;
        }

        public Paciente SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("@ID", id);

            conexaoComBanco.Open();

            SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

            Paciente paciente = null;

            if (leitorPaciente.Read())
                paciente = ConverterParaPaciente(leitorPaciente);

            conexaoComBanco.Close();

            return paciente;
        }

        private Paciente ConverterParaPaciente(SqlDataReader leitorPaciente)
        {
            int id = Convert.ToInt32(leitorPaciente["ID"]);
            string nome = leitorPaciente["NOME"].ToString();
            string cartaoSUS = leitorPaciente["CARTAOSUS"].ToString();

            return new Paciente
            {
                Numero = id,
                Nome = nome,
                CartaoSUS = cartaoSUS
            };
        }

        private void ConfigurarParametrosPaciente(Paciente novoPaciente, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("@ID", novoPaciente.Numero);
            comando.Parameters.AddWithValue("@NOME", novoPaciente.Nome);
            comando.Parameters.AddWithValue("@CARTAOSUS", novoPaciente.CartaoSUS);
        }
    }
}
