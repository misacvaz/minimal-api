using MinimalApi.Dominio.Entidades;
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


