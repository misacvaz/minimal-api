using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidade;

namespace MinimalApi.Infraestrutura.Db;

public class DbContexto : DbContext
{

    private readonly IConfiguration _configuracaoAppSettings;

    public DbContexto(IConfiguration configuracaoAppSettings)
    {
        _configuracaoAppSettings = configuracaoAppSettings;
    }
 
    public DbSet<Administrador> Administradores { set; get; } = default!;
    public DbSet<Veiculo> Veiculos { set; get; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    
    {
        modelBuilder.Entity<Administrador>().HasData(
            new Administrador{

                Id = 1,
                Email = "administrador@test.com",
                Senha = "123456",
                Perfil = "adm"
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        if(!optionsBuilder.IsConfigured)
        {
            var stringConexao = _configuracaoAppSettings.GetConnectionString("MySql")?.ToString();

            if(!string.IsNullOrEmpty(stringConexao))
            {
                optionsBuilder.UseMySql(stringConexao,
                ServerVersion.AutoDetect(stringConexao));
            }
        }
    
    }

}
