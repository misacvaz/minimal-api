using MinimalApi.Dominio.Entidade;
using MinimalApi.Dominio.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Test.Mocks;

namespace MinimalApi.Test.Servicos;

    [TestClass]
    public class VeiculoServicoTest
    {
        private IVeiculoServico? veiculoServico;

        [TestInitialize]
        public void Setup()
        {
            veiculoServico = new VeiculoServicoMock(); // Instância do mock para cada teste
        }

        [TestMethod]
        public void TestandoIncluirVeiculo()
        {
            // Arrange
            var veiculo = new Veiculo
            {
                Nome = "Fusca",
                Marca = "Volkswagen",
                Ano = 1976
            };

            // Act
            veiculoServico?.Incluir(veiculo);

            // Assert
            Assert.AreEqual(3, veiculoServico?.Todos().Count); // Deve haver 3 veículos após a inclusão
        }

        [TestMethod]
        public void TestandoBuscaPorId()
        {
            // Act
            var veiculo = veiculoServico?.BuscaPorId(1);

            // Assert
            Assert.IsNotNull(veiculo);
            Assert.AreEqual("Corolla", veiculo?.Nome);
        }

        [TestMethod]
        public void TestandoAtualizarVeiculo()
        {
            // Arrange
            var veiculo = new Veiculo
            {
                Id = 1,
                Nome = "Corolla Atualizado",
                Marca = "Toyota",
                Ano = 2021
            };

            // Act
            veiculoServico?.Atualizar(veiculo);

            // Assert
            var veiculoAtualizado = veiculoServico?.BuscaPorId(1);
            Assert.AreEqual("Corolla Atualizado", veiculoAtualizado?.Nome);
        }

        [TestMethod]
        public void TestandoApagarVeiculo()
        {
            // Arrange
            var veiculo = new Veiculo
            {
                Id = 1,
                Nome = "Corolla",
                Marca = "Toyota",
                Ano = 2020
            };

            // Act
            veiculoServico?.Apagar(veiculo);

            // Assert
            Assert.AreEqual(1, veiculoServico?.Todos().Count); // Apenas um veículo deve restar após apagar o primeiro
        }

        [TestMethod]
        public void TestandoTodosVeiculos()
        {
            // Act
            var veiculos = veiculoServico?.Todos();

            // Assert
            Assert.AreEqual(2, veiculos?.Count); // Deve retornar 2 veículos mockados
        }
    }

