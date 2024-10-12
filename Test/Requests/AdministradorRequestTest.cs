/*using MinimalApi.Dominio.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Helpers;
using MinimalApi.Dominio.DTOs;
using System.Text.Json;
using System.Text;
using System.Net;
using MinimalApi.Dominio.ModelViews;

namespace MinimalApi.Test.Request;

[TestClass]
public class AdministradorRequestTest
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
    public async Task TestarGetSetPropriedades()
    {
        // Arrange
        var loginDTO = new LoginDTO
        {
            Email = "adm@test.com",
            Senha = "123456"
        };

        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

        // Act
        var response = await Setup.client.PostAsync("/administradores/login", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();

        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admLogado?.Email, "O campo Email não deve ser nulo.");
        Assert.IsNotNull(admLogado?.Perfil, "O campo Perfil não deve ser nulo.");
        Assert.IsNotNull(admLogado?.Token, "O campo Token não deve ser nulo.");


        Console.WriteLine("Token: ", admLogado?.Token);
    }
}
*/

using MinimalApi.Dominio.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Helpers;
using MinimalApi.Dominio.DTOs;
using System.Text.Json;
using System.Text;
using System.Net;
using MinimalApi.Dominio.ModelViews;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using MinimalApi.Dominio;
using Test.Mocks;

namespace MinimalApi.Test.Request
{
    [TestClass]
    public class AdministradorRequestTest
    {
       public static string Nome{ get; } = "Administrador";

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
        public async Task TestarGetSetPropriedades()
        {
            // Arrange
            var loginDTO = new LoginDTO
            {
                Email = "adm@test.com",
                Senha = "123456"
            };

            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

            // Act
            var response = await Setup.client.PostAsync("/administradores/login", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();

            var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(admLogado?.Email, "O campo Email não deve ser nulo.");
            Assert.IsNotNull(admLogado?.Perfil, "O campo Perfil não deve ser nulo.");
            Assert.IsNotNull(admLogado?.Token, "O campo Token não deve ser nulo.");

            Console.WriteLine("Token: " + admLogado?.Token);
        }

        [TestMethod]
        public async Task TestarCriarAdministrador()
        {

            var token = await GerarToken();
            if(token == null)
            {
                Assert.Fail("falha ao obter token JKT válido");
            }
            // Arrange
            var novoAdm = new AdministradorDTO
            {
                Email = "adm@test.com",
                Senha = "123456",
                Perfil = Perfil.Adm
            };

            var content = new StringContent(JsonSerializer.Serialize(novoAdm), Encoding.UTF8, "application/json");

            // Act
            var responseLogin = await Setup.client.PostAsync("/administradores/login", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, responseLogin.StatusCode);

            var result = await responseLogin.Content.ReadAsStringAsync();
            var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions{

                PropertyNameCaseInsensitive = true
            });

            Setup.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", admLogado?.Token);
        }

        [TestMethod]
        public async Task TestarListarAdministradores()
        {
            // Arrange
            await TestarCriarAdministrador();
            var token = await GerarToken();
            if (token == null)
            {
                Assert.Fail("Falha ao obter token JWT válido");
            }

            var client = Setup.client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.GetAsync("/administradores");

            // Assert
            response.EnsureSuccessStatusCode(); // Verifica se a resposta foi bem-sucedida
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Verifique se o JSON está correto
            Assert.IsNotNull(jsonResponse);
        }
        public async Task TestarAtualizarAdministrador()
        {
            // Arrange
            var token = await GerarToken();
            if (token == null)
            {
                Assert.Fail("Falha ao obter token JWT válido");
            }

            // Cria um novo administrador para garantir que existe um ID 1
            var novoAdmin = new AdministradorDTO
            {
                Email = "admin_inicial@test.com",
                Senha = "123456",
                Perfil = Perfil.Adm
            };

            var client = Setup.client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Adiciona o administrador
            var novoAdminContent = new StringContent(JsonSerializer.Serialize(novoAdmin), Encoding.UTF8, "application/json");
            await client.PostAsync("/administradores", novoAdminContent);

            // Atualiza o administrador
            var atualizadoAdm = new AdministradorDTO
            {
                Email = "adm@test.com",
                Senha = "123456",
                Perfil = Perfil.Adm
            };

            var content = new StringContent(JsonSerializer.Serialize(atualizadoAdm), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync("/administradores/1", content); // Substitua pelo ID do administrador que deseja atualizar

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }


        /*[TestMethod]
        public async Task TestarApagarAdministrador()
        {
            // Arrange
            await TestarCriarAdministrador();
            var token = await GerarToken();
            if (token == null)
            {
                Assert.Fail("Falha ao obter token JWT válido");
            }

            var client = Setup.client;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await client.DeleteAsync("/administradores/1"); // ID fixo para o administrador a ser apagado

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }*/

        /**/




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


