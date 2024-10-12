using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MinimalApi.Dominio.DTOs;
using Test.Helpers;
using MinimalApi.Dominio.ModelViews;
using System.Net.Http.Json;

namespace MinimalApi.Test.Request
{
    [TestClass]
    public class VeiculoRequestTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Setup.ClassInit(testContext); // Inicia o contexto de teste
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.ClassCleanup(); // Limpa os recursos após os testes
        }

        [TestMethod]
       
        public async Task TestarCriarVeiculo()
        {
            // Arrange
            var token = await GerarToken(); // Obter o token JWT válido
            if (token == null)
            {
                Assert.Fail("Falha ao obter token JWT válido");
            }

            var veiculoDTO = new VeiculoDTO
            {
                Nome = "Civic",
                Marca = "Honda",
                Ano = 2018
            };

            var content = new StringContent(JsonSerializer.Serialize(veiculoDTO), Encoding.UTF8, "application/json");

            // Act
            var client = Setup.client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Incluir token JWT no cabeçalho
            var response = await client.PostAsync("/veiculos", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }


        [TestMethod]
        public async Task TestarListarVeiculos()
        {
            // Arrange
            var token = await GerarToken();
            if (token == null)
            {
                Assert.Fail("Falha ao obter token JWT válido");
            }

            var client = Setup.client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.GetAsync("/veiculos");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var veiculos = await response.Content.ReadFromJsonAsync<List<VeiculoDTO>>();
            Assert.IsTrue(veiculos?.Count > 0 ); // Verifica se há veículos na lista
        }


        [TestMethod]
        public async Task TestarBuscarVeiculoPorId()
        {
            // Arrange
            var token = await GerarToken();
            if (token == null)
            {
                Assert.Fail("Falha ao obter token JWT válido");
            }

            var client = Setup.client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.GetAsync("/veiculos/1"); // Substitua pelo ID que deseja testar

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var veiculo = await response.Content.ReadFromJsonAsync<VeiculoDTO>();
            Assert.IsNotNull(veiculo);
            Assert.AreEqual("Corolla", veiculo.Nome); // Verifique se os detalhes estão corretos
        }


        [TestMethod]
        public async Task TestarAtualizarVeiculo()
        {
            // Arrange
            var token = await GerarToken();
            if (token == null)
            {
                Assert.Fail("Falha ao obter token JWT válido");
            }

            var veiculoDTO = new VeiculoDTO
            {
                Nome = "Civic Atualizado",
                Marca = "Honda",
                Ano = 2021
            };

            var content = new StringContent(JsonSerializer.Serialize(veiculoDTO), Encoding.UTF8, "application/json");

            var client = Setup.client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.PutAsync("/veiculos/1", content); // Substitua pelo ID do veículo que deseja atualizar

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        [TestMethod]
        public async Task TestarApagarVeiculo()
        {
            // Arrange
            var token = await GerarToken();
            if (token == null)
            {
                Assert.Fail("Falha ao obter token JWT válido");
            }

            var client = Setup.client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.DeleteAsync("/veiculos/1"); // Substitua pelo ID do veículo que deseja apagar

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }






        private async Task<string?> GerarToken()
        {
            var loginDto = new LoginDTO { Email = "adm@test.com", Senha = "123456" };
            var response = await Setup.client.PostAsJsonAsync("/administradores/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AdministradorLogado>();
                return result?.Token;
            }

            // Log adicional
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Falha ao gerar token: {response.StatusCode}, {responseContent}");
            return null; // Retorna null em vez de lançar uma exceção
        }


    }
}
