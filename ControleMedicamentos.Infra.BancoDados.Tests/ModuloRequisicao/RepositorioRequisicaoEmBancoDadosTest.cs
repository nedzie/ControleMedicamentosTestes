using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.Compartilhado;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloRequisicao
{
    [TestClass]
    public class RepositorioRequisicaoEmBancoDadosTest
    {
        Requisicao req;
        Fornecedor forn;
        Funcionario func;
        Paciente pac;
        Medicamento med;
        RepositorioRequisicaoEmBancoDados _repositorioReq;
        RepositorioMedicamentoEmBancoDados _repositorioMed;
        RepositorioFornecedorEmBancoDados _repositorioForn;
        RepositorioPacienteEmBancoDados _repositorioPac;
        RepositorioFuncionarioEmBancoDados _repositorioFunc;

        public RepositorioRequisicaoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBREQUISICAO; DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)");

            med = new("Teste", "10 caracteres", "Teste", DateTime.Today);
            forn = new("Fornecedor", "49998287261", "contato@gmail.com", "Lages", "SC");
            func = new("Gevásio", "gege123", "criptografada");
            pac = new("Tobias de Antunes", "1234567890123");

            req = new(med, pac, func, 5, DateTime.Today);

            _repositorioReq = new();
            _repositorioMed = new();
            _repositorioForn = new();
            _repositorioPac = new();
            _repositorioFunc = new();
        }

        [TestMethod]
        public void DeveInserirRequisicao()
        {
            _repositorioForn.Inserir(forn);
            med.Fornecedor = forn;
            _repositorioMed.Inserir(med);

            _repositorioFunc.Inserir(func);

            _repositorioPac.Inserir(pac);

            _repositorioReq.Inserir(req);

            var requisicaoEncontrada = _repositorioReq.SelecionarPorId(req.Numero);

            Assert.IsNotNull(requisicaoEncontrada);

            Assert.AreEqual(req, requisicaoEncontrada);
        }
    }
}
