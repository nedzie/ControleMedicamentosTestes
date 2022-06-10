using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleMedicamentos.Dominio.Tests.ModuloMedicamento
{
    [TestClass]
    public class MedicamentoTest
    {
        [TestMethod]
        public void DeveRetornarQuantiaRequisicoes()
        {
            // arrange
            Medicamento medicamento = new("Nome", "Descrição", "Lote", DateTime.MaxValue);
            Requisicao req = new();

            // action | mude aqui
            int quantia = 7;

            for (int i = 0; i < quantia; i++)
                medicamento.Requisicoes.Add(req);

            // assert
            Assert.AreEqual(quantia, medicamento.QuantidadeRequisicoes);
        }

        [TestMethod]
        public void DeveRetornarIgualidade()
        {
            // arrange
            Medicamento medicamento = new("Nome", "Descrição", "Lote", DateTime.MaxValue);
            // action
            Medicamento medicamento2 = medicamento;

            // assert
            Assert.AreEqual(medicamento, medicamento2);
        }
    }
}
