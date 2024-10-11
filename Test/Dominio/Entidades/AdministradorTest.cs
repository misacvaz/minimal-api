
using MinimalApi.Dominio.Entidades;



using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinimalApi.Dominio.Entidade; // Certifique-se de ter este import


namespace MinimalApi.Dominio.Entidades;

    [TestClass] // Adicione este atributo
    public class AdministradorTest
    {
        [TestMethod]
        public void TestarGetSetPropriedades()
        {
            //arrange
            var adm = new Administrador();

            //Act
            adm.Id = 1;
            adm.Email = "test@test.com";
            adm.Senha = "test";
            adm.Perfil = "Adm";

            //assert
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("test@test.com", adm.Email);
            Assert.AreEqual("test", adm.Senha);
            Assert.AreEqual("Adm", adm.Perfil);
        }
    }


