using MinimalApi.Dominio.Entidade;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Infraestrutura.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalApi.Dominio.Servicos;
using System.Reflection;

namespace MinimalApi.Dominio.Entidades;

    [TestClass]
    public class AdministradorServicoTest
    {
        private DbContexto CriarContextoDeTeste()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

            var builder = new ConfigurationBuilder()
                .SetBasePath(path ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            return new DbContexto(configuration);
        }

        [TestMethod]
        public void TestandoSalvarAdministrador()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");
            var adm = new Administrador
            {
                Email = "text@gmail.com",
                Senha = "text",
                Perfil = "Adm"
            };

            var administradorServico = new AdministradorServico(context);

            // Act
            administradorServico.Incluir(adm);

            // Assert
            Assert.AreEqual(1, administradorServico.Todos(1).Count());
        }

        [TestMethod]
        public void TestandoBuscaPorId()
        {
            // Arrange
            var context = CriarContextoDeTeste();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");
            var adm = new Administrador
            {
                Email = "text@gmail.com",
                Senha = "text",
                Perfil = "Adm"
            };

            var administradorServico = new AdministradorServico(context);

            // Act
            administradorServico.Incluir(adm);
            var admDoBanco = administradorServico.BuscaPorId(adm.Id);

            // Assert
            Assert.AreEqual(adm.Id, admDoBanco?.Id);
        }

    public void TestandoTodosAdministradores()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");
        var adm = new Administrador
        {
            Email = "test@gmail.com",
            Senha = "123456",
            Perfil = "Adm"
        };

        var adm1 = new Administrador
        {
            
            Email = "editor@test.com",
            Senha = "123456",
            Perfil = "editor"
        };

        var administradorServico = new AdministradorServico(context);

        // Act
        administradorServico.Incluir(adm);
        administradorServico.Incluir(adm1);
        var administradores = administradorServico.Todos(1); // Testa a obtenção de todos os administradores

        // Assert
        Assert.AreEqual(2, administradores.Count());
    }



}



