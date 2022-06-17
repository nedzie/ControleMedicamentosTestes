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


namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloRequisicao
{
    [TestClass]
    public class RepositorioRequisicaoEmBancoDadosTest
    {
        Requisicao req;
        Fornecedor forn;
        Funcionario func;
        Funcionario funcEditar;
        Paciente pac;
        Paciente pacEditar;
        Medicamento med;
        RepositorioRequisicaoEmBancoDados _repositorioReq;
        RepositorioMedicamentoEmBancoDados _repositorioMed;
        RepositorioFornecedorEmBancoDados _repositorioForn;
        RepositorioPacienteEmBancoDados _repositorioPac;
        RepositorioFuncionarioEmBancoDados _repositorioFunc;

        public RepositorioRequisicaoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBREQUISICAO; DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFORNECEDOR; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBPACIENTE; DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");
            

            med = new("Teste", "10 caracteres", "Teste", DateTime.Today);
            forn = new("Fornecedor", "49998287261", "contato@gmail.com", "Lages", "SC");
            func = new("Gevásio", "gege123", "criptografada");
            funcEditar = new("Editado", "aleatorio", "aleatoria");
            pac = new("Tobias de Antunes", "1234567890123");
            pacEditar = new("Editado", "0000000000000");

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

        [TestMethod]
        public void DeveEditarRequisicao()
        {
            _repositorioForn.Inserir(forn);
            med.Fornecedor = forn;
            _repositorioMed.Inserir(med);

            _repositorioFunc.Inserir(func);

            _repositorioPac.Inserir(pac);

            _repositorioReq.Inserir(req);

            _repositorioFunc.Inserir(funcEditar);

            _repositorioPac.Inserir(pacEditar);

            req.QtdMedicamento = 15;
            req.Paciente = pacEditar;
            req.Funcionario = funcEditar;

            _repositorioReq.Editar(req);

            var requisicaoEncontrada = _repositorioReq.SelecionarPorId(req.Numero);

            Assert.IsNotNull(requisicaoEncontrada);

            Assert.AreEqual(req, requisicaoEncontrada);
        }

        [TestMethod]
        public void DeveExcluirRequisicao()
        {
            _repositorioForn.Inserir(forn);
            med.Fornecedor = forn;
            _repositorioMed.Inserir(med);

            _repositorioFunc.Inserir(func);

            _repositorioPac.Inserir(pac);

            _repositorioReq.Inserir(req);

            _repositorioReq.Excluir(req);

            var requisicaoEncontrada = _repositorioReq.SelecionarPorId(req.Numero);

            Assert.IsNull(requisicaoEncontrada);
        }

        [TestMethod]
        public void DeveSelecionarTodas()
        {
            _repositorioForn.Inserir(forn);

            med.Fornecedor = forn;
            
            _repositorioMed.Inserir(med);

            _repositorioFunc.Inserir(func);

            _repositorioPac.Inserir(pac);

            int quantidade = 3;

            for (int i = 0; i < quantidade; i++)
            {
                req.QtdMedicamento = i;
                _repositorioReq.Inserir(req);
            }

            var requisicoes = _repositorioReq.SelecionarTodos();

            Assert.IsNotNull(requisicoes);

            Assert.AreEqual(0, requisicoes[0].QtdMedicamento);
            Assert.AreEqual(1, requisicoes[1].QtdMedicamento);
            Assert.AreEqual(2, requisicoes[2].QtdMedicamento);
        }

        [TestMethod]
        public void DeveSelecionarApenasUm()
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
