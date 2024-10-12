using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidade;
using MinimalApi.Dominio.Interfaces;

namespace Test.Mocks;

public class AdministradorServicoMocks : IAdministradorServico
{

    private static List<Administrador> administradores = new List<Administrador>(){

        new Administrador{
            
            Id = 1,
            Email = "adm@test.com",
            Senha = "123456",
            Perfil = "Adm"
        },

         new Administrador{

            Id = 2,
            Email = "editor@test.com",
            Senha = "123456",
            Perfil = "editor"
        }
    };
    public Administrador? BuscaPorId(int id)
    {
        return administradores.Find(a => a.Id == id);
    }

    public Administrador? Incluir(Administrador administrador)
    {

        administrador.Id = administradores.Count() + 1;
        administradores.Add(administrador);

        return administrador;
    }

    public Administrador? Login(LoginDTO loginDTO)
    {
        return administradores.Find(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);
    }

        

    public List<Administrador> Todos(int? pagina)
    {
        return administradores;
    }


    //para atualizar administrador

    public Administrador? Atualizar(Administrador administrador)
    {
        var adminExistente = BuscaPorId(administrador.Id);
        if(adminExistente != null)
        {
            adminExistente.Email = adminExistente.Email;
            adminExistente.Senha = adminExistente.Senha;
            adminExistente.Perfil = adminExistente.Perfil;

            return adminExistente;

        }

        return null;
    }


     /*public bool Apagar(int id)
     {
        var administrador = BuscaPorId(id);
        if(administrador != null)
        {
            administradores.Remove(administrador);
            return true;
        }
        return false;
     }*/
}
